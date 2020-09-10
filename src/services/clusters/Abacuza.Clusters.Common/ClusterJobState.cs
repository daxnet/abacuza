using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Clusters.Common
{
    public enum ClusterJobState
    {
        Created,
        Initializing,
        Running,
        Completed,
        Cancelled,
        Failed,
        Unknown
    }
}
