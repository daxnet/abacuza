using Abacuza.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK.InputReaders
{
    public class InputReaderException : AbacuzaException
    {
        public InputReaderException() { }

        public InputReaderException(string message)
            : base(message)
        { }

        public InputReaderException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
