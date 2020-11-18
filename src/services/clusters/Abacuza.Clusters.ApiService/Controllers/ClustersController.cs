using Abacuza.Clusters.ApiService.Models;
using Abacuza.Clusters.Common;
using Abacuza.Common.DataAccess;
using Microsoft.AspNetCore.Http;
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
            IDataAccessObject dao) => (_clusterImplementations, _dao, _logger) = (clusterImplementations, dao, logger);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllClusters()
            => Ok(_clusterImplementations.Select(ci => new
            {
                id = ci.Id,
                name = ci.Name,
                description = ci.Description,
                type = ci.Type
            }));

        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllClusterTypes()
            => Ok(_clusterImplementations.Select(ci => ci.Type));


        [HttpGet("state/{connectionName}")]
        public async Task<IActionResult> GetClusterStateAsync(string connectionName)
        {
            if (string.IsNullOrEmpty(connectionName))
            {
                return BadRequest("The connectionName is not specified.");
            }

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

            var clusterConnection = connectionEntity.Create(clusterImplementation.ConnectionType);
            var clusterState = await clusterImplementation.GetStateAsync(clusterConnection);
            return Ok(clusterState);
        }
    }
}
