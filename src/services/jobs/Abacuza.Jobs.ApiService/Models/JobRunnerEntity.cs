// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020 by daxnet. All rights reserved.
// Licensed under LGPL-v3
// ==============================================================

using Abacuza.Common;
using Abacuza.Common.DataAccess;
using Abacuza.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Abacuza.Jobs.ApiService.Models
{
    /// <summary>
    /// Represents a Job Runner registry in Abacuza.
    /// </summary>
    [StorageModel("jobrunners")]
    public sealed class JobRunnerEntity : IEntity
    {
        #region Public Properties

        public List<S3File> BinaryFiles { get; set; }

        [Required]
        public string ClusterType { get; set; }
        public string Description { get; set; }
        
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string PayloadTemplate { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override bool Equals(object obj)
        {
            return obj is JobRunnerEntity entity &&
                   Id.Equals(entity.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString() => Name;

        #endregion Public Methods
    }
}
