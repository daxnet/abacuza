using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Abacuza.Common.DataAccess;
using Abacuza.JobSchedulers.Common;
using Abacuza.JobSchedulers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Abacuza.JobSchedulers.Controllers
{
    [Route("api/connections")]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {
        private readonly ILogger<ConnectionsController> _logger;
        private readonly IDataAccessObject _daoConnections;
        private readonly IEnumerable<ICluster> _clusters;

        public ConnectionsController(ILogger<ConnectionsController> logger,
            IDataAccessObject daoConnections,
            IEnumerable<ICluster> clusters)
        {
            _logger = logger;
            _daoConnections = daoConnections;
            _clusters = clusters;
        }

        /// <summary>
        /// Creates a new cluster connection.
        /// </summary>
        /// <param name="payload">The request body to be posted.</param>
        /// <returns>The create connection result.</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateConnectionAsync([FromBody] CreateConnectionRequest payload)
        {
            if (string.IsNullOrEmpty(payload?.Name))
            {
                return BadRequest("payload.Name is not specified.");
            }

            if (string.IsNullOrEmpty(payload?.Type))
            {
                return BadRequest("payload.ClusterType is not specified.");
            }

            if (string.IsNullOrEmpty(payload?.Settings))
            {
                return BadRequest("payload.Settings is not specified.");
            }

            var cluster = _clusters.FirstOrDefault(c => c.Name == payload.Type);
            if (cluster == null)
            {
                return NotFound($"The cluster '{payload.Type}' is not found.");
            }

            var clusterConnection = cluster.CreateConnection(payload.Name, payload.Settings);
            await _daoConnections.AddAsync(clusterConnection);
            return CreatedAtAction(nameof(GetConnectionAsync), new { id = clusterConnection.Id }, clusterConnection.Id);
        }

        /// <summary>
        /// Retrieves a cluster connection by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the connection.</param>
        /// <returns>The connection.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConnectionAsync(Guid id)
        {
            var connection = await _daoConnections.GetByIdAsync<ClusterConnection>(id);
            if (connection == null)
            {
                return NotFound();
            }

            return Ok(connection);
        }
    }
}
