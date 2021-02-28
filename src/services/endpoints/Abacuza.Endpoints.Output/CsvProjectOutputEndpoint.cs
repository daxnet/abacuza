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
    [Endpoint(
        "endpoint.output.csv.projectfile",
        "CSV/TSV Files Saved to Projects",
        EndpointType.Output,
        Description = "The output endpoint that saves the processing results as CSV/TSV files to the project sub-folder.")]
    public class CsvProjectOutputEndpoint : CsvEndpoint, IOutputEndpoint
    {
        #region Public Properties

        [Checkbox("chkEscapeQuotes",
            "Escape quotes",
            DefaultValue = "true",
            Ordinal = 120,
            Tooltip = "A flag indicating whether values containing quotes should always be enclosed in quotes.")]
        public bool EscapeQuotes { get; set; }

        [Checkbox("chkQuoteAll",
            "Quote all",
            DefaultValue = "false",
            Ordinal = 115,
            Tooltip = "A flag indicating whether all values should always be enclosed in quotes.")]
        public bool QuoteAll { get; set; }

        #endregion Public Properties
    }
}