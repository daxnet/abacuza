using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abacuza.Common.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Abacuza.JobSchedulers.Controllers
{
    [Route("api/cluster-connections")]
    [ApiController]
    public class ClusterConnectionsController : ControllerBase
    {
        private readonly ILogger<ClusterConnectionsController> _logger;
        private readonly IDataAccessObject _daoConnections;

        public ClusterConnectionsController(ILogger<ClusterConnectionsController> logger,
            IDataAccessObject daoConnections)
        {
            _logger = logger;
            _daoConnections = daoConnections;
        }


    }
}
