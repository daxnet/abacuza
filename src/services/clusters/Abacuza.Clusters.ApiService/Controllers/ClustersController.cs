using Abacuza.Clusters.ApiService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Clusters.ApiService.Controllers
{
    [ApiController]
    [Route("api/clusters")]
    public class ClustersController : ControllerBase
    {
        private readonly ILogger<ClustersController> _logger;
        private readonly ClusterCollection _clusterImplementations;

        public ClustersController(ClusterCollection clusterImplementations,
            ILogger<ClustersController> logger)
        {
            _clusterImplementations = clusterImplementations;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllClusters()
            => Ok(_clusterImplementations.Select(ci => new
            {
                id = ci.Id,
                name = ci.Name,
                description = ci.Description,
                type = ci.Type
            }));
    }
}
