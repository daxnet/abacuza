using Abacuza.Common.DataAccess;
using Abacuza.JobSchedulers.Common;
using Abacuza.JobSchedulers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IEnumerable<ICluster> _clusters;
        private readonly IDataAccessObject _dao;

        public JobsController(IEnumerable<ICluster> clusters,
            IDataAccessObject dao)
        {
            _clusters = clusters;
            _dao = dao;
        }
        
        [HttpPost("submit")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SubmitJobAsync([FromBody] SubmitJobRequest request)
        {
            if (string.IsNullOrEmpty(request.ClusterConnectionName))
            {
                return BadRequest($"The connection of the cluster that is used for submitting the job is not specified.");
            }

            var clusterConnection = (await _dao.FindBySpecificationAsync<ClusterConnectionStorageModel>(cc => cc.Name == request.ClusterConnectionName)).FirstOrDefault();
            if (clusterConnection == null)
            {
                return NotFound($"The cluster connection '{request.ClusterConnectionName}' does not exist.");
            }

            var cluster = _clusters.FirstOrDefault(c => c.Name == clusterConnection.Type);
            if (cluster == null)
            {
                return NotFound($"The cluster '{clusterConnection.Type}' does not exist.");
            }

            if (!cluster.ValidateJobParameters(request.JobParameters))
            {
                return BadRequest($"The cluster '{cluster.Name}' cannot accept the job parameters specified.");
            }



            return Ok();
        }
    }
}