using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abacuza.JobSchedulers.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Abacuza.JobSchedulers.Controllers
{
    /// <summary>
    /// Represents the APIs for Clusters.
    /// </summary>
    [Route("api/clusters")]
    [ApiController]
    public class ClustersController : ControllerBase
    {
        private readonly ILogger<ClustersController> _logger;
        private readonly IEnumerable<ICluster> _clusters;

        public ClustersController(ILogger<ClustersController> logger, IEnumerable<ICluster> clusters)
        {
            _logger = logger;
            _clusters = clusters;
        }

        /// <summary>
        /// Gets all clusters.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllClusters()
            => Ok(_clusters.Select(c => new { c.Id, c.Name, c.Description }));
    }
}
