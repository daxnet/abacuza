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
    /// <summary>
    /// Represents the input endpoint that accepts a JSON text that represents
    /// the settings of multiple data sources.
    /// </summary>
    [Endpoint("endpoints.input.multids", "Multi-Data Source", EndpointType.Input, Description = "The input endpoint that reads data from multiple data sources.")]
    public sealed class MultiDataSourcesInputEndpoint : Endpoint, IInputEndpoint
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the JSON text that contains the settings of multiple data sources.
        /// </summary>
        [JsonTextArea("txtDataSourceSettings", "Data source settings")]
        public string Json { get; set; }

        #endregion Public Properties
    }
}