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
using System.Collections.Generic;

namespace Abacuza.Projects.ApiService.Models
{
    /// <summary>
    /// Represents the Job data that is retrieved from the Jobs API.
    /// </summary>
    internal record Job(Guid Id,
        Guid? ConnectionId,
        string Name,
        DateTime? CreatedDate,
        DateTime? CompletedDate,
        DateTime? FailedDate,
        DateTime? CancelledDate,
        string SubmissionName,
        JobState State)
    {
        /// <summary>
        /// Gets the name of the job status.
        /// </summary>
        public string? JobStatusName => Enum.GetName(typeof(JobState), State);
    }
}
