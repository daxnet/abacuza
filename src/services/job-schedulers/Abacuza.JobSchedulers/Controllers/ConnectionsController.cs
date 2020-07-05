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
        public async Task<IActionResult> CreateConnectionAsync([FromBody] ClusterConnectionStorageModel payload)
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

            if ((await _daoConnections.FindBySpecificationAsync<ClusterConnectionStorageModel>(expr => expr.Name == payload.Name))?.Count() > 0)
            {
                return Conflict($"The cluster connection '{payload.Name}' already exists.");
            }

            payload.Id = Guid.NewGuid();
            await _daoConnections.AddAsync(payload);
            return CreatedAtAction(nameof(GetConnectionAsync), new { id = payload.Id }, payload.Id);
        }

        /// <summary>
        /// Retrieves a cluster connection by its identifier.
        /// </summary>
        /// <param name="id">The id of the connection.</param>
        /// <returns>The connection.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConnectionAsync(Guid id)
        {
            var connection = await _daoConnections.GetByIdAsync<ClusterConnectionStorageModel>(id);
            if (connection == null)
            {
                return NotFound($"The cluster connection '{id}' could not be found.");
            }

            return Ok(connection);
        }

        /// <summary>
        /// Retrieves a cluster connection by its name.
        /// </summary>
        /// <param name="name">The name of the connection.</param>
        /// <returns>The connection.</returns>
        [HttpGet("get-by-name/{name}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConnectionByNameAsync(string name)
        {
            var connection = (await _daoConnections.FindBySpecificationAsync<ClusterConnectionStorageModel>(m => m.Name == name)).FirstOrDefault();
            if (connection == null)
            {
                return NotFound($"The cluster connection '{name}' could not be found.");
            }

            return Ok(connection);
        }
    }
}
