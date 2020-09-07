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
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ILogger<ClustersController> _logger;
        private readonly ClusterCollection _clusterImplementations;
        private readonly IDataAccessObject _dao;

        public JobsController(ClusterCollection clusterImplementations,
            ILogger<ClustersController> logger,
            IDataAccessObject dao)
        {
            _clusterImplementations = clusterImplementations;
            _logger = logger;
            _dao = dao;
        }

        [HttpPost("submit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Job))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExecuteJobAsync([FromBody] ExecuteJobRequest request)
        {
            var clusterConnectionEntities = await _dao.FindBySpecificationAsync<ClusterConnectionEntity>(ce => ce.ClusterType == request.ClusterType);
            var availableClusters = new List<(ICluster, IClusterConnection)>();
            foreach (var clusterConnectionEntity in clusterConnectionEntities)
            {
                var clusterImplementation = _clusterImplementations.FirstOrDefault(ci => ci.Type == clusterConnectionEntity.ClusterType);
                var clusterConnection = clusterConnectionEntity.Create(clusterImplementation.ConnectionType);
                var state = await clusterImplementation.GetStateAsync(clusterConnection);
                if (state == ClusterState.Online)
                {
                    availableClusters.Add((clusterImplementation, clusterConnection));
                }
            }

            if (availableClusters.Count > 0)
            {
                // TODO: cluster scheduling
                var (cluster, connection) = availableClusters[0];
                try
                {
                    var job = await cluster.SubmitJobAsync(connection, request.Properties);
                    return Ok(job);
                }
                catch (JobException je)
                {
                    return BadRequest(je.ToString());
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.ToString());
                }
            }

            return BadRequest($"There is no available cluster whose type is '{request.ClusterType}' that can serve the job execution request.");
        }
    }
}
