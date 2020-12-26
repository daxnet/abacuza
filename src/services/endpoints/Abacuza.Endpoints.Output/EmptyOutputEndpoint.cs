using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Endpoints.Output
{
    [Endpoint("endpoints.output.empty",
        "Empty",
        EndpointType.Output,
        Description = "The endpoint that doesn't write to any data storage.")]
    public sealed class EmptyOutputEndpoint : Endpoint, IOutputEndpoint
    {
    }
}
