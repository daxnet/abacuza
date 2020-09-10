using Abacuza.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Clusters.Common
{
    public class ClusterJobException : AbacuzaException
    {
        public ClusterJobException()
            : base()
        { }

        public ClusterJobException(string message)
            : base(message)
        { }

        public ClusterJobException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
