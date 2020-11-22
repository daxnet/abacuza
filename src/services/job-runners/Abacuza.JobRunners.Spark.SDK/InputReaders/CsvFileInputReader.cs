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
    public sealed class CsvFileInputReader : InputReader<CsvInputEndpoint>
    {
        #region Protected Methods

        /// <summary>
        /// Reads the data sets from the given <see cref="T:Abacuza.Endpoints.IInputEndpoint" /> and under the
        /// specified <see cref="T:Microsoft.Spark.Sql.SparkSession" />.
        /// </summary>
        /// <param name="sparkSession">The <see cref="T:Microsoft.Spark.Sql.SparkSession" /> which creates the <see cref="T:Microsoft.Spark.Sql.DataFrame" />.</param>
        /// <param name="inputEndpoint">The <see cref="T:Abacuza.Endpoints.IInputEndpoint" /> instance which provides the information
        /// of the input data sets.</param>
        /// <returns>
        /// The <see cref="T:Microsoft.Spark.Sql.DataFrame" /> for data processing.
        /// </returns>
        /// <exception cref="SparkRunnerException">No files could be read by the JsonFileInputReader.</exception>
        protected override DataFrame ReadFromInternal(SparkSession sparkSession, CsvInputEndpoint inputEndpoint)
        {
            var options = new Dictionary<string, string>();
            if (inputEndpoint.HasHeaderRecord)
            {
                options.Add("header", "true");
            }

            if (inputEndpoint.InferSchema)
            {
                options.Add("inferSchema", "true");
            }

            var delimiter = inputEndpoint.SeparatorCharacter switch
            {
                "Tab" => "\t",
                "Comma" => ",",
                "Pipe" => "|",
                "Space" => " ",
                _ => ","
            };
            options.Add("delimiter", delimiter);

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