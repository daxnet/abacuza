// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020 by daxnet. All rights reserved.
// Licensed under LGPL-v3
// ==============================================================

using Abacuza.Endpoints;
using Abacuza.Endpoints.Input;
using Abacuza.JobRunners.Spark.SDK.InputReaders;
using Microsoft.Spark;
using Microsoft.Spark.Sql;
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

        private static readonly Lazy<IEnumerable<Type>> _inputEndpointTypes = new Lazy<IEnumerable<Type>>(() =>
            from p in typeof(EmptyInputEndpoint).Assembly.GetExportedTypes()
            where p.IsClass && !p.IsAbstract && typeof(IInputEndpoint).IsAssignableFrom(p) && p.IsDefined(typeof(EndpointAttribute), false)
            select p
        );

        private static readonly Lazy<IEnumerable<Type>> _inputReaderTypes = new Lazy<IEnumerable<Type>>(() =>
            from p in typeof(IInputReader).Assembly.GetExportedTypes()
            where p.IsClass && !p.IsAbstract && typeof(IInputReader).IsAssignableFrom(p)
            select p
        );

        #endregion Private Fields

        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <c>SparkRunnerBase</c> class.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
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

            if (!TryParseSingleValue(_args, InputEndpointSettingsKey, out var inputEndpointSettings))
            {
                throw new SparkRunnerException("Input endpoint settings is not specified in the argument list.");
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
            var inputEndpoint = CreateInputEndpoint(inputEndpointName, inputEndpointSettings);
            var inputReader = CreateInputReader(inputEndpointName);
            var dataFrame = inputReader.ReadFrom(sparkSession, inputEndpoint);

            RunInternal(sparkSession, dataFrame);
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void RunInternal(SparkSession sparkSession, DataFrame dataFrame);

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