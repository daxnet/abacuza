using Abacuza.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK
{
    public sealed class SparkRunnerException : AbacuzaException
    {
        public SparkRunnerException() { }

        public SparkRunnerException(string message)
            : base(message)
        { }

        public SparkRunnerException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
