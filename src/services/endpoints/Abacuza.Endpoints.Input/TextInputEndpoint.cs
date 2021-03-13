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
    /// Represents the input endpoint that reads data from text files.
    /// </summary>
    [Endpoint("endpoints.input.text",
        "Text Files",
        EndpointType.Input,
        Description = "The endpoint that reads data from text files.")]
    public sealed class TextInputEndpoint : Endpoint, IInputEndpoint
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the text files.
        /// </summary>
        [FilePicker("fpTextFiles", "Text Files", AllowedExtensions = ".txt", AllowMultipleSelection = true, Tooltip = "Choose text files.")]
        public List<S3File>? Files { get; set; }

        #endregion Public Properties
    }
}