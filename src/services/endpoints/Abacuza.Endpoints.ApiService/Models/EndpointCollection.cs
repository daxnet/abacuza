using Abacuza.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Endpoints.ApiService.Models
{
    public class EndpointCollection : ItemCollection<IEndpoint>
    {
        public IEnumerable<IEndpoint> GetEndpointsByType(EndpointType? endpointType)
        {
            if (endpointType != null)
            {
                return _items.Where(i => i.Type == endpointType.Value);
            }

            return _items;
        }
    }
}
