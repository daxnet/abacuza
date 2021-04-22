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

using Abacuza.Common.UIComponents;

namespace Abacuza.Endpoints.Input
{
    [Endpoint("endpoints.input.pathbased",
        "Path",
        EndpointType.Input,
        Description = "The endpoint that reads data from given paths.")]
    public sealed class PathBasedInputEndpoint : Endpoint, IInputEndpoint
    {
        #region Public Properties

        [TextBox("txtFormats", "Formats", DefaultValue = "csv,json", Ordinal = 1500, Tooltip = "Comma-separated list of formats.")]
        public string Formats { get; set; }

        [TextArea("txtPaths", "Paths", Ordinal = 1000, Tooltip = "Specify the paths, one per line.")]
        public string Paths { get; set; }

        #endregion Public Properties
    }
}