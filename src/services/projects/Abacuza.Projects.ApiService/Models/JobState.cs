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

namespace Abacuza.Projects.ApiService.Models
{
    /// <summary>
    /// Represents the state of a job that exactly matches the job status
    /// defined in the Jobs API service.
    /// </summary>
    internal enum JobState
    {
        Unknown,
        Created,
        Initializing,
        Running,
        Busy,
        Completed,
        Cancelled,
        Failed
    }
}