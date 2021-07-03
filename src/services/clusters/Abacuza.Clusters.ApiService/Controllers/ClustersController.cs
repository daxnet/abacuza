// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020-2021 by daxnet. All rights reserved.
// Apache License Version 2.0
// ==============================================================

using Abacuza.Clusters.ApiService.Models;
using Abacuza.Common.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Clusters.ApiService.Controllers
{
    [ApiController]
    [Route("api/clusters")]
    public class ClustersController : ControllerBase
    {
        #region Private Fields

        private readonly ClusterCollection _clusterImplementations;
        private readonly IDataAccessObject _dao;
        private readonly ILogger<ClustersController> _logger;

        #endregion Private Fields

        #region Public Constructors

        public ClustersController(ClusterCollection clusterImplementations,
            ILogger<ClustersController> logger,
            IDataAccessObject dao) => (_clusterImplementations, _dao, _logger) = (clusterImplementations, dao, logger);

        #endregion Public Constructors

        #region Public Methods

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
            if (clusterConnection != null)
            {
                var clusterState = await clusterImplementation.GetStateAsync(clusterConnection);
                return Ok(clusterState);
            }

            return BadRequest($"Cannot create the cluster connection by type {clusterImplementation.ConnectionType}");
        }

        #endregion Public Methods
    }
}