// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020-2021 by daxnet. All rights reserved.
// Apache License Version 2.0
// ==============================================================

using Abacuza.Endpoints;
using Abacuza.JobRunners.Spark.SDK.InputReaders;
using Abacuza.JobRunners.Spark.SDK.OutputWriters;
using Microsoft.Spark;
using Microsoft.Spark.Sql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK
{
    /// <summary>
    /// Represents the base class for all job runners that executes
    /// on .NET for Spark tech stack and can be schedule by Abacuza.
    /// </summary>
    public abstract class SparkRunnerBase
    {

        #region Protected Fields

        protected readonly string[] _args;

        #endregion Protected Fields

        #region Private Fields

        private const string AppNameKey = "appname";
        private const string InputEndpointDefinitionsKey = "input_defs";
        private const string OutputEndpointDefinitionsKey = "output_defs";
        private const string ProjectContextKey = "project_context";

        private static readonly Lazy<IEnumerable<Type>> _inputEndpointTypes = new(() => DiscoverDerivedTypes<IInputEndpoint>(typeof(EndpointAttribute)));

        private static readonly Lazy<IEnumerable<Type>> _inputReaderTypes = new(() => DiscoverDerivedTypes<IInputReader>());

        private static readonly Lazy<IEnumerable<Type>> _outputEndpointTypes = new(() => DiscoverDerivedTypes<IOutputEndpoint>(typeof(EndpointAttribute)));

        private static readonly Lazy<IEnumerable<Type>> _outputWriterTypes = new(() => DiscoverDerivedTypes<IOutputWriter>());

        #endregion Private Fields

        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SparkRunnerBase"/> class.
        /// </summary>
        /// <param name="args">The arguments used for running the job.</param>
        protected SparkRunnerBase(string[] args)
        {
            _args = args;
        }

        #endregion Protected Constructors

        #region Public Methods

        /// <summary>
        /// Runs the data transformation on the Spark cluster.
        /// </summary>
        public void Run()
        {
            if (!TryParseSingleBase64Value(_args, InputEndpointDefinitionsKey, out var inputEndpointDefinitionsJson))
            {
                throw new SparkRunnerException("Input endpoint definitions are not specified in the argument list.");
            }

            if (!TryParseSingleBase64Value(_args, OutputEndpointDefinitionsKey, out var outputEndpointDefinitionsJson))
            {
                throw new SparkRunnerException("Output endpoint definitions are not specified in the argument list.");
            }

            if (!TryParseSingleValue(_args, ProjectContextKey, out var projectContextValue))
            {
                throw new SparkRunnerException("Project context value is not specified in the argument list.");
            }

            Console.WriteLine($"**** Project Context: {projectContextValue}");

            var projectContext = JsonConvert.DeserializeObject<ProjectContext>(projectContextValue);
            if (projectContext == null)
            {
                throw new SparkRunnerException("Project context is failed to be initialized.");
            }

            var builder = SparkSession.Builder();
            if (TryParseSingleValue(_args, AppNameKey, out var appName))
            {
                builder = builder.AppName(appName);
            }

            if (TryParseSparkConfig(_args, out var conf))
            {
                builder = builder.Config(conf);
            }

            var sparkSession = builder.GetOrCreate();
            var inputEndpoints = CreateInputEndpoints(inputEndpointDefinitionsJson);
            DataFrame? dataFrame = null;
            foreach(var inputEndpoint in inputEndpoints)
            {
                var reader = CreateInputReader(inputEndpoint.Name);
                if (dataFrame == null)
                {
                    dataFrame = reader.ReadFrom(sparkSession, inputEndpoint, projectContext);
                }
                else
                {
                    dataFrame = dataFrame.UnionByName(reader.ReadFrom(sparkSession, inputEndpoint, projectContext));
                }
            }

            if (dataFrame == null)
            {
                throw new SparkRunnerException("Unable to initialize DataFrame, no input endpoint that could be inferred from command line arguments.");
            }

            var dataFrameResult = RunInternal(sparkSession, dataFrame);

            var outputEndpoint = CreateOutputEndpoint(outputEndpointDefinitionsJson);
            if (outputEndpoint == null)
            {
                throw new SparkRunnerException("Unable to activate the output endpoint instance.");
            }

            var outputWriter = CreateOutputWriter(outputEndpoint.Name);
            outputWriter.WriteTo(dataFrameResult, outputEndpoint, projectContext);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Runs the data processing based on the data frame.
        /// </summary>
        /// <param name="sparkSession">The spark session.</param>
        /// <param name="dataFrame">The data frame.</param>
        protected abstract DataFrame RunInternal(SparkSession sparkSession, DataFrame dataFrame);

        #endregion Protected Methods

        #region Private Methods

        private static IEnumerable<IInputEndpoint> CreateInputEndpoints(string inputEndpointDefinitionsJson)
        {
            var inputEndpointDefinitionsArray = JArray.Parse(inputEndpointDefinitionsJson);
            foreach (var definition in inputEndpointDefinitionsArray)
            {
                var inputEndpointName = definition["Name"]?.ToObject<string>();
                var inputEndpointSettings = definition["Settings"]?.ToObject<string>();
                if (!string.IsNullOrEmpty(inputEndpointName))
                {
                    var inputEndpointType = _inputEndpointTypes.Value.FirstOrDefault(iet => iet.GetCustomAttribute<EndpointAttribute>().Name == inputEndpointName);
                    if (inputEndpointType != null)
                    {
                        var inputEndpoint = (IInputEndpoint)Activator.CreateInstance(inputEndpointType);
                        if (!string.IsNullOrEmpty(inputEndpointSettings))
                        {
                            inputEndpoint.ApplySettings(inputEndpointSettings);
                        }

                        yield return inputEndpoint;
                    }
                }
            }
        }

        private static IInputReader CreateInputReader(string inputEndpointName)
        {
            var inputEndpointType = _inputEndpointTypes.Value.FirstOrDefault(t => t.GetCustomAttribute<EndpointAttribute>().Name == inputEndpointName);
            if (inputEndpointType == null)
            {
                throw new SparkRunnerException($"Can't find the input endpoint {inputEndpointName}.");
            }

            var inputReaderType = (from type in _inputReaderTypes.Value
                                   where type.BaseType?.IsGenericType ?? false &&
                                   type.BaseType?.GetGenericTypeDefinition() == typeof(InputReader<>)
                                   let genericArguments = type.BaseType?.GetGenericArguments()
                                   where genericArguments?.Length == 1 && genericArguments[0] == inputEndpointType
                                   select type).FirstOrDefault();
            if (inputReaderType == null)
            {
                throw new SparkRunnerException($"Can't find the input reader for input endpoint {inputEndpointName}.");
            }

            return (IInputReader)Activator.CreateInstance(inputReaderType);
        }

        private static IOutputEndpoint? CreateOutputEndpoint(string outputEndpointDefinitionJson)
        {
            var outputEndpointDefinition = JObject.Parse(outputEndpointDefinitionJson);
            var outputEndpointName = outputEndpointDefinition["Name"]?.ToObject<string>();
            var outputEndpointSettings = outputEndpointDefinition["Settings"]?.ToObject<string>();

            if (!string.IsNullOrEmpty(outputEndpointName))
            {
                var outputEndpointType = _outputEndpointTypes.Value.FirstOrDefault(oet => oet.GetCustomAttribute<EndpointAttribute>().Name == outputEndpointName);
                if (outputEndpointType != null)
                {
                    var outputEndpoint = (IOutputEndpoint)Activator.CreateInstance(outputEndpointType);
                    if (!string.IsNullOrEmpty(outputEndpointSettings))
                    {
                        // As for each project, there will be only one output endpoint settings defined, so the 
                        // endpoint settings JSON passed in is a JSON object rather than an array. However, the
                        // ApplySettings method requires that the JSON should be an array, so append the `[` and `]`
                        // to the outputEndpointSettings JSON string.
                        outputEndpointSettings = $"[{outputEndpointSettings}]";
                        outputEndpoint.ApplySettings(outputEndpointSettings);
                    }

                    return outputEndpoint;
                }
            }

            return null;
        }

        private static IOutputWriter CreateOutputWriter(string outputEndpointName)
        {
            var outputEndpointType = _outputEndpointTypes.Value.FirstOrDefault(t => t.GetCustomAttribute<EndpointAttribute>().Name == outputEndpointName);
            if (outputEndpointType == null)
            {
                throw new SparkRunnerException($"Can't find the input endpoint {outputEndpointName}.");
            }

            var outputWriterType = (from type in _outputWriterTypes.Value
                                    where type.BaseType?.IsGenericType ?? false &&
                                    type.BaseType?.GetGenericTypeDefinition() == typeof(OutputWriter<>)
                                    let genericArguments = type.BaseType?.GetGenericArguments()
                                    where genericArguments?.Length == 1 && genericArguments[0] == outputEndpointType
                                    select type).FirstOrDefault();
            if (outputWriterType == null)
            {
                throw new SparkRunnerException($"Can't find the output writer for output endpoint {outputEndpointName}.");
            }

            return (IOutputWriter)Activator.CreateInstance(outputWriterType);
        }

        private static IEnumerable<Type> DiscoverDerivedTypes<T>(Type? attributeType = null)
        {
            var types = new List<Type>();
            var path = Path.GetDirectoryName(typeof(SparkRunnerBase).Assembly.Location);
            Console.WriteLine($"*** Discovering Path: {path}");
            var assemblyFiles = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories);
            foreach (var assemblyFile in assemblyFiles)
            {
                if (Path.GetFileName(assemblyFile).StartsWith("System"))
                {
                    // Bypassing the system assemblies.
                    continue;
                }

                try
                {
                    var assembly = Assembly.LoadFrom(assemblyFile);
                    var query = from p in assembly.GetTypes()
                                where p.IsClass && !p.IsAbstract && typeof(T).IsAssignableFrom(p)
                                select p;
                    if (attributeType != null)
                    {
                        query = query.Where(p => p.IsDefined(attributeType, false));
                    }

                    types.AddRange(query);
                }
                catch
                {
                }
            }

            return types;
        }

        private static bool TryParseSingleBase64Value(string[] args, string key, out string value, Encoding? encoding = null)
        {
            if (TryParseSingleValue(args, key, out var base64))
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                value = encoding!.GetString(Convert.FromBase64String(base64));
                return true;
            }

            value = string.Empty;
            return false;
        }

        private static bool TryParseSingleValue(string[] args, string key, out string value)
        {
            var item = args?.FirstOrDefault(x => x.StartsWith($"{key}:", StringComparison.InvariantCultureIgnoreCase));
            if (!string.IsNullOrEmpty(item))
            {
                var colonIndex = item.IndexOf(':');
                value = item.Substring(colonIndex + 1, item.Length - colonIndex - 1);
                return true;
            }

            value = string.Empty;
            return false;
        }
        private static bool TryParseSparkConfig(string[] args, out SparkConf? conf)
        {
            var configEntries = args.Where(a => a.StartsWith("config:", StringComparison.InvariantCultureIgnoreCase))
                .Select(a =>
                {
                    var colonIndex = a.IndexOf(':');
                    return a.Substring(colonIndex + 1, a.Length - colonIndex - 1);
                });
            if (configEntries.Count() > 0)
            {
                conf = new SparkConf();
                foreach (var entry in configEntries)
                {
                    var keyvalue = entry.Split('=', StringSplitOptions.RemoveEmptyEntries);
                    conf = conf.Set(keyvalue[0], keyvalue[1]);
                }

                return true;
            }

            conf = null;
            return false;
        }

        #endregion Private Methods

    }
}