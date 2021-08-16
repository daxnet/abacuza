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

namespace Abacuza.Projects.ApiService.Models
{
    [StorageModel("revisions")]
    public sealed class Revision : IEntity
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the date on which the revision was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the id of the revision.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the job submission.
        /// </summary>
        public string? JobSubmissionName { get; set; }

        /// <summary>
        /// Gets or sets the id of the project to which the current revision
        /// belongs.
        /// </summary>
        public Guid ProjectId { get; set; }

        #endregion Public Properties
    }
}