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
using Abacuza.JobRunners.Spark.SDK;
using Abacuza.JobRunners.Spark.SDK.InputReaders;
using Microsoft.Spark.Sql;
using System;
using System.Linq;

namespace Abacuza.JobRunners.Spark.SDK.IO.InputReaders
{
    /// <summary>
    /// Represents the input reader that reads data from JSON files.
    /// </summary>
    public sealed class JsonFileInputReader : InputReader<JsonInputEndpoint>
    {
        #region Protected Methods

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
        /// <exception cref="SparkRunnerException">No files could be read by the JsonFileInputReader</exception>
        protected override DataFrame ReadFromInternal(SparkSession sparkSession, JsonInputEndpoint inputEndpoint, ProjectContext projectContext)
        {
            var jsonFiles = inputEndpoint
                .Files
                .Select(f => $"s3a://{f.Bucket}/{f.Key}/{f.File}")
                .ToArray();

            if (jsonFiles?.Length == 0)
            {
                throw new SparkRunnerException("No files could be read by the JsonFileInputReader.");
            }

            return sparkSession.Read().Json(jsonFiles);
        }

        #endregion Protected Methods
    }
}