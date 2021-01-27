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

using System;

namespace Abacuza.Endpoints
{
    /// <summary>
    /// Represents that the decorated classes are endpoints.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class EndpointAttribute : Attribute
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <c>EndpointAttribute</c> class.
        /// </summary>
        /// <param name="name">The name of the endpoint.</param>
        /// <param name="displayName">The display name of the endpoint that will be shown on the UI.</param>
        /// <param name="type">The type of the endpoint.</param>
        public EndpointAttribute(string name, string displayName, EndpointType type)
        {
            Name = name;
            DisplayName = displayName;
            Type = type;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the description of the current endpoint.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the display name of the current endpoint.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Gets the name of the current endpoint.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the current endpoint.
        /// </summary>
        public EndpointType Type { get; }

        #endregion Public Properties
    }
}