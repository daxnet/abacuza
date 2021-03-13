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

using Abacuza.Common.UIComponents;

namespace Abacuza.Endpoints.Output
{
    /// <summary>
    /// Represents the output endpoint for CSV/TSV files.
    /// </summary>
    /// <remarks>For a complete list of the options, please refer to https://spark.apache.org/docs/latest/api/java/org/apache/spark/sql/DataFrameWriter.html.
    /// </remarks>
    [Endpoint("endpoints.output.csv.generic", "CSV/TSV Files", EndpointType.Output)]
    public class CsvGenericOutputEndpoint : CsvProjectOutputEndpoint, IOutputEndpoint
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the path to which the CSV should be written.
        /// </summary>
        [TextBox("txtCsvOutputPath",
            "Path",
            Required = true,
            Ordinal = 500,
            Tooltip = "The output path.")]
        public string? Path { get; set; }

        #endregion Public Properties
    }
}