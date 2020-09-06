using Abacuza.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Clusters.Common
{
    public class ClusterException : AbacuzaException
    {
        public ClusterException()
            : base()
        { }

        public ClusterException(string message)
            : base(message)
        { }


    }
}
