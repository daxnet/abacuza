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
            IDataAccessObject dao) => (_clusterImplementations, _dao, _logger) = (clusterImplementations, dao, logger);

        [HttpPost("submit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClusterJob))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExecuteJobAsync([FromBody] ExecuteJobRequest request)
        {
            var clusterImplementation = _clusterImplementations.FirstOrDefault(ci => ci.Type == request.ClusterType);
            if (clusterImplementation == null)
            {
                return NotFound($"The cluster whose type is {request.ClusterType} does not exist.");
            }

            var clusterConnectionEntities = await _dao.FindBySpecificationAsync<ClusterConnectionEntity>(ce => ce.ClusterType == request.ClusterType);
            var availableClusters = new List<(ICluster, IClusterConnection)>();
            foreach (var clusterConnectionEntity in clusterConnectionEntities)
            {
                var clusterConnection = clusterConnectionEntity.Create(clusterImplementation.ConnectionType);
                var state = await clusterImplementation.GetStateAsync(clusterConnection);

                _logger.LogDebug($"Cluster connection: {clusterConnectionEntity.Name}, State: {state}");

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
                catch (ClusterJobException je)
                {
                    _logger.LogError($"Failed submitting the job. {je}");
                    return BadRequest(je.ToString());
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.ToString());
                }
            }

            return BadRequest($"There is no available cluster whose type is '{request.ClusterType}' that can serve the job execution request.");
        }

        [HttpPost("statuses")]
        public async Task<IActionResult> GetJobStatusesAsync([FromBody] GetJobStatusesRequest request)
        {
            var response = new GetJobStatusesResponse();

            foreach (var requestItem in request)
            {
                var clusterConnectionEntity = await _dao.GetByIdAsync<ClusterConnectionEntity>(requestItem.ConnectionId);
                if (clusterConnectionEntity == null)
                {
                    response.AddRange(
                        requestItem.LocalJobIdentifiers.Select(jobid =>
                            new GetJobStatusesResponseItem
                            {
                                ConnectionId = requestItem.ConnectionId,
                                LocalJobId = jobid,
                                Succeeded = false,
                                ErrorMessage = $"The connection {requestItem.ConnectionId} does not exist.",
                                State = ClusterJobState.Unknown
                            }));
                    continue;
                }

                var cluster = _clusterImplementations.FirstOrDefault(ci => ci.Type == clusterConnectionEntity.ClusterType);
                if (cluster == null)
                {
                    response.AddRange(
                        requestItem.LocalJobIdentifiers.Select(jobid =>
                            new GetJobStatusesResponseItem
                            {
                                ConnectionId = requestItem.ConnectionId,
                                LocalJobId = jobid,
                                Succeeded = false,
                                ErrorMessage = $"The cluster whose type is {clusterConnectionEntity.ClusterType} does not exist.",
                                State = ClusterJobState.Unknown
                            }));
                    continue;
                }

                var connection = clusterConnectionEntity.Create(cluster.ConnectionType);
                foreach (var localJobId in requestItem.LocalJobIdentifiers)
                {
                    var responseItem = new GetJobStatusesResponseItem
                    {
                        ConnectionId = requestItem.ConnectionId,
                        LocalJobId = localJobId
                    };

                    try
                    {
                        var localJob = await cluster.GetJobAsync(connection, localJobId);
                        responseItem.Succeeded = true;
                        responseItem.State = localJob.State;
                        responseItem.Logs = localJob.Logs;
                    }
                    catch (Exception ex)
                    {
                        responseItem.Succeeded = false;
                        responseItem.ErrorMessage = ex.ToString();
                    }

                    response.Add(responseItem);
                }
            }

            return Ok(response);
        }
    }
}
