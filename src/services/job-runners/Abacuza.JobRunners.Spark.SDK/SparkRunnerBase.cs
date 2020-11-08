using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Spark;
using Microsoft.Spark.Sql;
using System;
using System.Linq;

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

        #endregion Private Fields

        #region Protected Constructors

        protected SparkRunnerBase(string[] args)
        {
            _args = args;
        }

        #endregion Protected Constructors

        #region Public Methods

        public void Run()
        {
            string inputEndpointName, inputEndpointSettings;
            if (!TryParseSingleValue(_args, InputEndpointNameKey, out inputEndpointName))
            {
                throw new SparkRunnerException("Input endpoint name is not specified in the argument list.");
            }

            if (!TryParseSingleValue(_args, InputEndpointSettingsKey, out inputEndpointSettings))
            {
                throw new SparkRunnerException("Input endpoint settings is not specified in the argument list.");
            }

            // TODO: Create the InputReader instance

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

            RunInternal(sparkSession, null);
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void RunInternal(SparkSession sparkSession, DataFrame dataFrame);

        #endregion Protected Methods

        #region Private Methods

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
