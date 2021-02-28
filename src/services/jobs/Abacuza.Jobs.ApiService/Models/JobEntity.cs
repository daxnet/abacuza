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

namespace Abacuza.JobSchedulers.Models
{
    [StorageModel("jobs")]
    public sealed class JobEntity : IEntity
    {

        #region Public Constructors

        public JobEntity()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public DateTime? CancelledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public Guid? ConnectionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? FailedDate { get; set; }
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the job id that is specific to the cluster on which
        /// the job was run.
        /// </summary>
        public string LocalJobId { get; set; } = string.Empty;

        public List<string> Logs { get; set; } = new List<string>();
        public string Name { get; set; } = string.Empty;
        public JobState State { get; set; }

        /// <summary>
        /// Gets or sets the name of the job submission, this is usually
        /// the name of the job executor that submits the job.
        /// </summary>
        public string SubmissionName { get; set; } = string.Empty;

        public JobTraceability? Traceability { get; set; }

        public int? TracingFailures { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString() => SubmissionName;

        #endregion Public Methods
    }
}