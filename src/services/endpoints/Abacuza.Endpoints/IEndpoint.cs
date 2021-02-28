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

using System.Collections.Generic;

namespace Abacuza.Endpoints
{
    /// <summary>
    /// Represents that the implemented classes are endpoints that contain connection
    /// information to the external data sources or data stores.
    /// </summary>
    public interface IEndpoint
    {
        #region Public Properties

        /// <summary>
        /// Gets the configuration UI elements that provide the UI capabilities for users
        /// to configure the endpoint.
        /// </summary>
        /// <value>
        /// The configuration UI elements.
        /// </value>
        IEnumerable<IEnumerable<KeyValuePair<string, object>>> ConfigurationUIElements { get; }

        /// <summary>
        /// Gets the description of the endpoint.
        /// </summary>
        /// <value>
        /// The description of the endpoint.
        /// </value>
        string? Description { get; }

        /// <summary>
        /// Gets the display name of the endpoint.
        /// </summary>
        /// <value>
        /// The display name of the endpoint.
        /// </value>
        string DisplayName { get; }

        /// <summary>
        /// Gets the name of the endpoint.
        /// </summary>
        /// <value>
        /// The name of the endpoint.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the endpoint type.
        /// </summary>
        /// <value>
        /// The type of the endpoint.
        /// </value>
        EndpointType Type { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Applies the specified JSON setting value to the current endpoint.
        /// </summary>
        /// <param name="settings">The JSON setting to be applied to the current endpoint.</param>
        void ApplySettings(string settings);

        #endregion Public Methods
    }
}