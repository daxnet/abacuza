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
using System;
using System.Linq;

namespace Abacuza.JobRunners.Spark.SDK.InputReaders
{
    /// <summary>
    /// Represents the input reader that reads data from JSON files.
    /// </summary>
    public sealed class JsonFileInputReader : InputReader<JsonInputEndpoint>
    {
        #region Protected Methods

        protected override DataFrame ReadFromInternal(SparkSession sparkSession, JsonInputEndpoint inputEndpoint)
        {
            var jsonFiles = inputEndpoint
                .Files
                .Select(f => $"s3a://{f.Bucket}/{f.Key}/{f.File}")
                .ToArray();

            foreach(var jf in jsonFiles)
            {
                Console.WriteLine($"** [JsonFileInputReader] Read from file: {jf}");
            }

            if (jsonFiles?.Length == 0)
            {
                throw new SparkRunnerException("No files could be read by the JsonFileInputReader");
            }

            return sparkSession.Read().Json(jsonFiles);
        }

        #endregion Protected Methods
    }
}