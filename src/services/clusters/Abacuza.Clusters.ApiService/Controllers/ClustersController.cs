using Abacuza.Clusters.ApiService.Models;
using Abacuza.Common.DataAccess;
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
        private readonly IDataAccessObject _dao;

        public ClustersController(ClusterCollection clusterImplementations,
            ILogger<ClustersController> logger,
            IDataAccessObject dao)
        {
            _clusterImplementations = clusterImplementations;
            _logger = logger;
            _dao = dao;
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


        [HttpGet("state/{connectionName}")]
        public async Task<IActionResult> GetClusterStateAsync(string connectionName)
        {
            var connectionEntity = (await _dao.FindBySpecificationAsync<ClusterConnectionEntity>(ce => ce.Name == connectionName)).FirstOrDefault();
            if (connectionEntity == null)
            {
                return NotFound($"The cluster connection {connectionName} doesn't exist.");
            }

            var clusterImplementation = _clusterImplementations.FirstOrDefault(ci => ci.Type == connectionEntity.ClusterType);
            if (clusterImplementation == null)
            {
                return NotFound($"The cluster with the type of {connectionEntity.ClusterType} does not exist.");
            }

            // connectionEntity.Create
            return Ok();
        }
    }
}
