﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Clusters.Common
{
    public enum ClusterJobState
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
