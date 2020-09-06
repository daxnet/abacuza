using Abacuza.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Clusters.Common
{
    public class JobException : AbacuzaException
    {
        public JobException()
            : base()
        { }

        public JobException(string message)
            : base(message)
        { }

        public JobException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
