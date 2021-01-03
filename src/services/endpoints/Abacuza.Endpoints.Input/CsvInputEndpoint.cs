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

using Abacuza.Common.Models;
using Abacuza.Common.UIComponents;
using System.Collections.Generic;

namespace Abacuza.Endpoints.Input
{
    /// <summary>
    /// Represents the input endpoint for CSV/TSV files.
    /// </summary>
    /// <remarks>For possible options please refer to https://spark.apache.org/docs/latest/api/java/org/apache/spark/sql/DataFrameReader.html
    /// </remarks>
    [Endpoint("endpoints.input.csv", "CSV/TSV Files", EndpointType.Input)]
    public sealed class CsvInputEndpoint : CsvEndpoint, IInputEndpoint
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a list of files, in particular, the files with CSV or TSV extensions.
        /// </summary>
        [FilePicker("fpFiles",
            "CSV/TSV Files",
            AllowedExtensions = ".csv,.tsv",
            AllowMultipleSelection = true,
            Ordinal = 500)]
        public List<S3File> Files { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value which indicates whether the schema should be inferred from the raw data.
        /// </summary>
        [Checkbox("chkInferSchema",
            "Infer schema",
            Ordinal = 186,
            Tooltip = "Whether the schema should be inferred.",
            DefaultValue = "true")]
        public bool InferSchema { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value which indicates whether a record in the file could span multiple lines.
        /// </summary>
        [Checkbox("chkMultiline",
            "Multi-line support",
            Ordinal = 160,
            Tooltip = "Whether a record can span multiple lines.",
            DefaultValue = "false")]
        public bool Multiline { get; set; }

        #endregion Public Properties
    }
}