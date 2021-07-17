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

using System;
using System.ComponentModel.DataAnnotations;

namespace Abacuza.Projects.ApiService.Models
{
    /// <summary>
    /// Represents the base class for the endpoint definition in a project.
    /// </summary>
    public abstract class ProjectEndpointDefinition
    {
        #region Public Properties

        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the endpoint, for example, endpoint.input.csv.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the JSON that represents the settings of the endpoint.
        /// </summary>
        public string? Settings { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets the string representation of the endpoint definition.
        /// </summary>
        /// <returns>The string representation of the endpoint definition.</returns>
        public override string ToString() => Name;

        #endregion Public Methods
    }
}