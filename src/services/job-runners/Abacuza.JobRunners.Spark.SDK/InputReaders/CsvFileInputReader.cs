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

using Abacuza.Endpoints.Input;
using Microsoft.Spark.Sql;
using System.Collections.Generic;
using System.Linq;

namespace Abacuza.JobRunners.Spark.SDK.InputReaders
{
    /// <summary>
    /// Represents the input reader that reads CSV files from the input endpoint.
    /// </summary>
    /// <seealso cref="InputReader{CsvInputEndpoint}" />
    public class CsvFileInputReader : InputReader<CsvInputEndpoint>
    {

        #region Protected Methods

        protected static Dictionary<string, string> ReadOptionsFromEndpoint(CsvInputEndpoint endpoint)
        {
            var options = new Dictionary<string, string>();
            if (endpoint.Header) options.Add("header", "true");
            if (endpoint.InferSchema) options.Add("inferSchema", "true");
            var delimiter = endpoint.Separator switch
            {
                "Tab" => "\t",
                "Comma" => ",",
                "Pipe" => "|",
                "Space" => " ",
                _ => ","
            };
            options.Add("sep", delimiter);

            if (!string.IsNullOrEmpty(endpoint.DateFormat)) options.Add("dateFormat", endpoint.DateFormat);
            if (!string.IsNullOrEmpty(endpoint.TimestampFormat)) options.Add("timestampFormat", endpoint.TimestampFormat);
            if (!string.IsNullOrEmpty(endpoint.Encoding)) options.Add("encoding", endpoint.Encoding);
            if (!string.IsNullOrEmpty(endpoint.Escape)) options.Add("escape", endpoint.Escape);
            if (endpoint.IgnoreLeadingWhiteSpace) options.Add("ignoreLeadingWhiteSpace", "true");
            if (endpoint.IgnoreTrailingWhiteSpace) options.Add("ignoreTrailingWhiteSpace", "true");
            if (!string.IsNullOrEmpty(endpoint.LineSep)) options.Add("lineSep", endpoint.LineSep);
            if (!string.IsNullOrEmpty(endpoint.Quote)) options.Add("quote", endpoint.Quote);
            if (endpoint.Multiline) options.Add("multiLine", "true");

            return options;
        }

        /// <summary>
        /// Reads the data sets from the given <see cref="T:Abacuza.Endpoints.IInputEndpoint" /> and under the
        /// specified <see cref="T:Microsoft.Spark.Sql.SparkSession" />.
        /// </summary>
        /// <param name="sparkSession">The <see cref="T:Microsoft.Spark.Sql.SparkSession" /> which creates the <see cref="T:Microsoft.Spark.Sql.DataFrame" />.</param>
        /// <param name="inputEndpoint">The <see cref="T:Abacuza.Endpoints.IInputEndpoint" /> instance which provides the information
        /// of the input data sets.</param>
        /// <param name="projectContext">The data that contains project and revision information.</param>
        /// <returns>
        /// The <see cref="T:Microsoft.Spark.Sql.DataFrame" /> for data processing.
        /// </returns>
        /// <exception cref="SparkRunnerException">No files could be read by the JsonFileInputReader.</exception>
        protected override DataFrame ReadFromInternal(SparkSession sparkSession, CsvInputEndpoint inputEndpoint, ProjectContext projectContext)
        {
            var options = ReadOptionsFromEndpoint(inputEndpoint);

            var csvFiles = inputEndpoint
                .Files
                .Select(f => $"s3a://{f.Bucket}/{f.Key}/{f.File}")
                .ToArray();

            if (csvFiles?.Length == 0)
            {
                throw new SparkRunnerException($"No files could be read by the {this.GetType().Name}.");
            }

            return sparkSession.Read().Options(options).Csv(csvFiles.ToArray());
        }

        #endregion Protected Methods
    }
}