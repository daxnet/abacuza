using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Endpoints
{
    public interface IEndpoint
    {
        string Name { get; }

        string DisplayName { get; }

        string Description { get; }

        EndpointType Type { get; }

        IEnumerable<IEnumerable<KeyValuePair<string, object>>> ConfigurationUIElements { get; }
    }
}
