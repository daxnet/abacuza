// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020 by daxnet. All rights reserved.
// Licensed under LGPL-v3
// ==============================================================

using Abacuza.Clusters.ApiService.Models;
using Abacuza.Common.DataAccess;
using DnsClient.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Abacuza.Clusters.ApiService.Controllers
{
    [ApiController]
    [Route("api/cluster-connections")]
    public class ClusterConnectionsController : ControllerBase
    {
        #region Private Fields

        private readonly ClusterCollection _clusterImplementations;
        private readonly IDataAccessObject _dao;
        private readonly ILogger<ClusterConnectionsController> _logger;

        #endregion Private Fields

        #region Public Constructors

        public ClusterConnectionsController(ILogger<ClusterConnectionsController> logger,
            ClusterCollection clusterImplementations,
            IDataAccessObject dao)
        {
            _logger = logger;
            _clusterImplementations = clusterImplementations;
            _dao = dao;
        }

        #endregion Public Constructors

        #region Public Methods

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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteClusterConnectionAsync(Guid id)
        {
            _logger.LogDebug($"Deleting cluster connection {id}.");
            var clusterConnection = await _dao.GetByIdAsync<ClusterConnectionEntity>(id);
            if (clusterConnection == null)
            {
                return NotFound($"Cluster connection {id} doesn't exist.");
            }

            await _dao.DeleteByIdAsync<ClusterConnectionEntity>(id);
            return Ok();
        }

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

        [HttpGet]
        [ProducesResponseType(typeof(ClusterConnectionEntity[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClusterConnectionsAsync()
            => Ok(await _dao.GetAllAsync<ClusterConnectionEntity>());

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchClusterConnectionAsync(Guid id, [FromBody] JsonPatchDocument<ClusterConnectionEntity> patchDoc)
        {
            var updatingEntity = await _dao.GetByIdAsync<ClusterConnectionEntity>(id);
            if (updatingEntity == null)
            {
                return NotFound($"Cluster connection {id} doesn't exist.");
            }

            if (patchDoc != null)
            {
                patchDoc.ApplyTo(updatingEntity, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _dao.UpdateByIdAsync(id, updatingEntity);

                return Ok(updatingEntity);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion Public Methods
    }
}