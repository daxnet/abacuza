using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Abacuza.Common
{
    public sealed class ServiceInvocationException : AbacuzaException
    {
        public ServiceInvocationException() { }

        public ServiceInvocationException(string message)
            : base(message)
        { }

        public ServiceInvocationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public ServiceInvocationException(HttpStatusCode httpStatusCode)
            : base($"Service invocation failed, status code: {httpStatusCode}.")
        { }
    }
}
