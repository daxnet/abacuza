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

using Abacuza.Common.Models;
using Abacuza.Common.UIComponents;
using System.Collections.Generic;

namespace Abacuza.Endpoints.Input
{
    /// <summary>
    /// Represents the input endpoint that reads data from JSON files.
    /// </summary>
    [Endpoint("endpoints.input.json",
        "JSON Files",
        EndpointType.Input,
        Description = "The endpoint that reads data from JSON files.")]
    public sealed class JsonInputEndpoint : Endpoint, IInputEndpoint
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the JSON files.
        /// </summary>
        [FilePicker("fpJsonFiles", "JSON Files", AllowedExtensions = ".json", AllowMultipleSelection = true, Tooltip = "Choose JSON files.")]
        public List<S3File>? Files { get; set; }

        #endregion Public Properties
    }
}