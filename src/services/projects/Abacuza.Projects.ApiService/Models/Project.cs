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
    [StorageModel("projects")]
    public sealed class Project : IEntity
    {
        #region Public Properties

        public DateTime DateCreated { get; set; }
        public string? Description { get; set; }
        public Guid Id { get; set; }

        public List<InputEndpointDefinition> InputEndpoints { get; set; } = new List<InputEndpointDefinition>();

        [Required]
        public Guid JobRunnerId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public OutputEndpointDefinition OutputEndpoint { get; set; } = new OutputEndpointDefinition();

        #endregion Public Properties

        #region Public Methods

        public override string ToString() => Name;

        #endregion Public Methods
    }
}