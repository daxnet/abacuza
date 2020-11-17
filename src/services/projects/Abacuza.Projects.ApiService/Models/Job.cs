﻿// ==============================================================
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

using System;

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
        public string JobStatusName => Enum.GetName(typeof(JobState), State);
    }
}