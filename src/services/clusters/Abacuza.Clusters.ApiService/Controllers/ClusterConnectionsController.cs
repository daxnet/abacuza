using Abacuza.Clusters.ApiService.Models;
using Abacuza.Clusters.Common;
using Abacuza.Common.DataAccess;
using DnsClient.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Abacuza.Clusters.ApiService.Controllers
{
    [ApiController]
    [Route("api/cluster-connections")]
    public class ClusterConnectionsController : ControllerBase
    {
        private readonly ILogger<ClusterConnectionsController> _logger;
        private readonly ClusterCollection _clusterImplementations;
        private readonly IDataAccessObject _dao;

        public ClusterConnectionsController(ILogger<ClusterConnectionsController> logger,
            ClusterCollection clusterImplementations,
            IDataAccessObject dao)
        {
            _logger = logger;
            _clusterImplementations = clusterImplementations;
            _dao = dao;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ClusterConnectionEntity[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClusterConnectionsAsync()
            => Ok(await _dao.GetAllAsync<ClusterConnectionEntity>());

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClusterConnectionEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClusterConnectionByIdAsync(Guid id)
        {
            var entity = await _dao.GetByIdAsync<ClusterConnectionEntity>(id);
            if (entity == null)
            {
                return NotFound($"Cluster connection {id} was not found.");
            }

            return Ok(entity);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateClusterConnectionAsync([FromBody] ClusterConnectionEntity model)
        {
            var clusterImplementation = _clusterImplementations.FirstOrDefault(ci => ci.Type == model.ClusterType);
            if (clusterImplementation == null)
            {
                return BadRequest($"The cluster whose type is '{model.ClusterType}' was not found.");
            }

            var existingEntity = (await _dao.FindBySpecificationAsync<ClusterConnectionEntity>(x => x.Name == model.Name)).FirstOrDefault();
            if (existingEntity != null)
            {
                return Conflict($"Duplicated name '{model.Name}'.");
            }

            await _dao.AddAsync(model);

            return CreatedAtAction(nameof(GetClusterConnectionByIdAsync), new { id = model.Id }, model.Id);
        }

    }
}
