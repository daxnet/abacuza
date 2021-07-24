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

using Abacuza.Common;
using Abacuza.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Abacuza.Projects.ApiService.Models
{
    /// <summary>
    /// Represents the project in Abacuza.
    /// </summary>
    [StorageModel("projects")]
    public sealed class Project : IEntity
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the date on which the project was created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the description of the project.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the id of the project.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a list of endpoint definitions that define the input of the data.
        /// </summary>
        public List<InputEndpointDefinition> InputEndpoints { get; set; } = new List<InputEndpointDefinition>();

        /// <summary>
        /// Gets or sets the id of the Job Runner on which the project data should be processed.
        /// </summary>
        [Required]
        public Guid JobRunnerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a list of endpoint definitions that define the output of the processed data. Note that only one of
        /// them will be used by the project, and the one being used is identified by the <c>SelectedOutputEndpointId</c> property.
        /// </summary>
        public List<OutputEndpointDefinition> OutputEndpoints { get; set; } = new List<OutputEndpointDefinition>();

        /// <summary>
        /// Gets or sets the id of the output endpoint that is used by the current project.
        /// </summary>
        [Required]
        public string SelectedOutputEndpointId { get; set; } = string.Empty;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets the string representation of the current project.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => Name;

        #endregion Public Methods
    }
}