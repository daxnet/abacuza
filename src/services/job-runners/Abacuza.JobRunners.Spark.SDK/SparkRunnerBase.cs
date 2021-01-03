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
// Licensed under LGPL-v3
// ==============================================================

using Abacuza.Endpoints;
using Abacuza.Endpoints.Input;
using Abacuza.Endpoints.Output;
using Abacuza.JobRunners.Spark.SDK.InputReaders;
using Abacuza.JobRunners.Spark.SDK.OutputWriters;
using Microsoft.Spark;
using Microsoft.Spark.Sql;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        private const string InputEndpointNameKey = "input_endpoint";
        private const string InputEndpointSettingsKey = "input_endpoint_settings";
        private const string OutputEndpointNameKey = "output_endpoint";
        private const string OutputEndpointSettingsKey = "output_endpoint_settings";
        private const string ProjectContextKey = "project_context";

        private static readonly Lazy<IEnumerable<Type>> _inputEndpointTypes = new Lazy<IEnumerable<Type>>(() =>
            from p in typeof(EmptyInputEndpoint).Assembly.GetExportedTypes()
            where p.IsClass && !p.IsAbstract && typeof(IInputEndpoint).IsAssignableFrom(p) && p.IsDefined(typeof(EndpointAttribute), false)
            select p
        );

        private static readonly Lazy<IEnumerable<Type>> _outputEndpointTypes = new Lazy<IEnumerable<Type>>(() =>
            from p in typeof(EmptyOutputEndpoint).Assembly.GetExportedTypes()
            where p.IsClass && !p.IsAbstract && typeof(IOutputEndpoint).IsAssignableFrom(p) && p.IsDefined(typeof(EndpointAttribute), false)
            select p
        );

        private static readonly Lazy<IEnumerable<Type>> _inputReaderTypes = new Lazy<IEnumerable<Type>>(() =>
            from p in typeof(IInputReader).Assembly.GetExportedTypes()
            where p.IsClass && !p.IsAbstract && typeof(IInputReader).IsAssignableFrom(p)
            select p
        );

        private static readonly Lazy<IEnumerable<Type>> _outputWriterTypes = new Lazy<IEnumerable<Type>>(() =>
            from p in typeof(IOutputWriter).Assembly.GetExportedTypes()
            where p.IsClass && !p.IsAbstract && typeof(IOutputWriter).IsAssignableFrom(p)
            select p
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
            if (!TryParseSingleValue(_args, InputEndpointNameKey, out var inputEndpointName))
            {
                throw new SparkRunnerException("Input endpoint name is not specified in the argument list.");
            }

            Console.WriteLine($"**** Input Endpoint Name: {inputEndpointName}");

            if (!TryParseSingleValue(_args, InputEndpointSettingsKey, out var inputEndpointSettings))
            {
                throw new SparkRunnerException("Input endpoint settings is not specified in the argument list.");
            }

            Console.WriteLine($"**** Input Endpoint Settings: {inputEndpointSettings}");

            if (!TryParseSingleValue(_args, OutputEndpointNameKey, out var outputEndpointName))
            {
                throw new SparkRunnerException("Output endpoint name is not specified in the argument list.");
            }

            Console.WriteLine($"**** Output Endpoint Name: {outputEndpointName}");

            if (!TryParseSingleValue(_args, OutputEndpointSettingsKey, out var outputEndpointSettings))
            {
                throw new SparkRunnerException("Output endpoint settings is not specified in the argument list.");
            }

            Console.WriteLine($"**** Output Endpoint Settings: {outputEndpointSettings}");

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
            var inputEndpoint = CreateInputEndpoint(inputEndpointName, inputEndpointSettings);
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

        private static IInputEndpoint CreateInputEndpoint(string inputEndpointName, string inputEndpointSettings)
        {
            var inputEndpointType = _inputEndpointTypes.Value.FirstOrDefault(t => t.GetCustomAttribute<EndpointAttribute>().Name == inputEndpointName);
            if (inputEndpointType == null)
            {
                throw new SparkRunnerException($"Can't find the input endpoint {inputEndpointName}.");
            }

            var inputEndpoint = (IInputEndpoint)Activator.CreateInstance(inputEndpointType);
            inputEndpoint.ApplySettings(inputEndpointSettings);

            return inputEndpoint;
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

        private static IOutputEndpoint CreateOutputEndpoint(string outputEndpointName, string outputEndpointSettings)
        {
            var outputEndpointType = _outputEndpointTypes.Value.FirstOrDefault(t => t.GetCustomAttribute<EndpointAttribute>().Name == outputEndpointName);
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

        private static bool TryParseSingleValue(string[] args, string key, out string value)
        {
            var item = args?.FirstOrDefault(x => x.StartsWith($"{key}:", StringComparison.InvariantCultureIgnoreCase));
            if (!string.IsNullOrEmpty(item))
            {
                var colonIndex = item.IndexOf(':');
                value = item.Substring(colonIndex + 1, item.Length - colonIndex - 1);
                return true;
            }

            value = null;
            return false;
        }

        private static bool TryParseSparkConfig(string[] args, out SparkConf conf)
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