﻿// ==============================================================
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Linq;

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
        private const string InputEndpointsKey = "input_endpoints";
        private const string OutputEndpointsKey = "output_endpoints";
        private const string ProjectContextKey = "project_context";

        private static readonly Lazy<IEnumerable<Type>> InputEndpointTypes = new Lazy<IEnumerable<Type>>(() =>
            DiscoverDerivedTypes<IInputEndpoint>(typeof(EndpointAttribute))
        );

        private static readonly Lazy<IEnumerable<Type>> InputReaderTypes = new Lazy<IEnumerable<Type>>(() =>
            DiscoverDerivedTypes<IInputReader>()
        );

        private static readonly Lazy<IEnumerable<Type>> OutputEndpointTypes = new Lazy<IEnumerable<Type>>(() =>
                    DiscoverDerivedTypes<IOutputEndpoint>(typeof(EndpointAttribute))
        );

        private static readonly Lazy<IEnumerable<Type>> OutputWriterTypes = new Lazy<IEnumerable<Type>>(() =>
            DiscoverDerivedTypes<IOutputWriter>()
        );

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
            if (!TryParseBase64Value(_args, InputEndpointsKey, out var inputEndpointDefinitions))
            {
                throw new SparkRunnerException("Input endpoint definitions are not specified in the argument list.");
            }

            if (!TryParseBase64Value(_args, OutputEndpointsKey, out var outputEndpointDefinitions))
            {
                throw new SparkRunnerException("Output endpoint definitions are not specified in the argument list.");
            }

            if (!TryParseSingleValue(_args, ProjectContextKey, out var projectContextValue))
            {
                throw new SparkRunnerException("Project context value is not specified in the argument list.");
            }

            Console.WriteLine($"**** Project Context: {projectContextValue}");

            var projectContext = JsonConvert.DeserializeObject<ProjectContext>(projectContextValue);

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
            // var inputEndpoint = CreateInputEndpoint(inputEndpointName, inputEndpointSettings);
            var inputEndpoints = CreateInputEndpoints(inputEndpointDefinitions);
            var inputReader = CreateInputReader(inputEndpointName);
            var dataFrame = inputReader.ReadFrom(sparkSession, inputEndpoint, projectContext);

            var dataFrameResult = RunInternal(sparkSession, dataFrame);

            var outputEndpoint = CreateOutputEndpoint(outputEndpointName, outputEndpointSettings);
            var outputWriter = CreateOutputWriter(outputEndpointName);
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
            foreach (var inputEndpointDefinition in inputEndpointDefinitionsArray)
            {
                var curName = inputEndpointDefinition["Name"]?.Value<string>();
                var curSettings = inputEndpointDefinition["Settings"]?.Value<string>();
                if (!string.IsNullOrEmpty(curName) && !string.IsNullOrEmpty(curSettings))
                {
                    var inputEndpointType = InputEndpointTypes.Value.FirstOrDefault(t =>
                        t.GetCustomAttribute<EndpointAttribute>().Name == curName);
                    if (inputEndpointType != null)
                    {
                        var inputEndpoint = (IInputEndpoint)Activator.CreateInstance(inputEndpointType);
                        inputEndpoint.ApplySettings(curSettings);
                        yield return inputEndpoint;
                    }
                }
            }
        }

        private static IInputReader CreateInputReader(string inputEndpointName)
        {
            var inputEndpointType = InputEndpointTypes.Value.FirstOrDefault(t => t.GetCustomAttribute<EndpointAttribute>().Name == inputEndpointName);
            if (inputEndpointType == null)
            {
                throw new SparkRunnerException($"Can't find the input endpoint {inputEndpointName}.");
            }

            var inputReaderType = (from type in InputReaderTypes.Value
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

        private static IOutputEndpoint CreateOutputEndpoint(string outputEndpointName, string outputEndpointSettings)
        {
            var outputEndpointType = OutputEndpointTypes.Value.FirstOrDefault(t => t.GetCustomAttribute<EndpointAttribute>().Name == outputEndpointName);
            if (outputEndpointType == null)
            {
                throw new SparkRunnerException($"Can't find the output endpoint {outputEndpointName}.");
            }

            var outputEndpoint = (IOutputEndpoint)Activator.CreateInstance(outputEndpointType);
            outputEndpoint.ApplySettings(outputEndpointSettings);

            return outputEndpoint;
        }

        private static IOutputWriter CreateOutputWriter(string outputEndpointName)
        {
            var outputEndpointType = OutputEndpointTypes.Value.FirstOrDefault(t => t.GetCustomAttribute<EndpointAttribute>().Name == outputEndpointName);
            if (outputEndpointType == null)
            {
                throw new SparkRunnerException($"Can't find the input endpoint {outputEndpointName}.");
            }

            var outputWriterType = (from type in OutputWriterTypes.Value
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

        private static bool TryParseBase64Value(string[] args, string key, out string originalValue)
        {
            if (TryParseSingleValue(args, key, out var base64Value))
            {
                originalValue = Encoding.UTF8.GetString(Convert.FromBase64String(base64Value));
                return true;
            }

            originalValue = string.Empty;
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