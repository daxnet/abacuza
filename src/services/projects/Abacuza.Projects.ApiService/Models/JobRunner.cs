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

using Abacuza.Common.Models;
using System;
using System.Collections.Generic;

namespace Abacuza.Projects.ApiService.Models
{
    /// <summary>
    /// Represents the Job Runner data that is retreived from the Jobs API.
    /// </summary>
    internal record JobRunner(Guid Id,
        string Name,
        string Description,
        string ClusterType,
        string PayloadTemplate,
        IEnumerable<S3File> BinaryFiles);
}