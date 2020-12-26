using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Endpoints.Output
{
    [Endpoint("endpoint.output.console", "Console", EndpointType.Output)]
    public class ConsoleOutputEndpoint : Endpoint, IOutputEndpoint
    {

    }
}
