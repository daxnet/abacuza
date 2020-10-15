using Abacuza.Common.DataAccess;
using Abacuza.Jobs.ApiService.Models;
using DnsClient.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Jobs.ApiService.Controllers
{
    [Route("api/job-runners")]
    [ApiController]
    public class JobRunnersController : ControllerBase
    {
        private readonly IDataAccessObject _dao;
        private readonly ILogger<JobRunnersController> _logger;

        public JobRunnersController(IDataAccessObject dao, ILogger<JobRunnersController> logger)
        {
            _dao = dao;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<JobRunnerEntity>))]
        public async Task<IActionResult> GetAllJobRunnersAsync()
            => Ok(await _dao.GetAllAsync<JobRunnerEntity>());

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobRunnerEntity))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetJobRunnerByIdAsync(Guid id)
        {
            var entity = await _dao.GetByIdAsync<JobRunnerEntity>(id);
            if (entity == null)
            {
                return NotFound($"The job runner {id} doesn't exist.");
            }

            return Ok(entity);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteJobRunnerByIdAsync(Guid id)
        {
            var entity = await _dao.GetByIdAsync<JobRunnerEntity>(id);
            if (entity == null)
            {
                return NotFound($"The job runner {id} doesn't exist.");
            }

            if (entity.BinaryFiles?.Count > 0)
            {
                // TODO: Delete all binary files in the entity.
                foreach (var file in entity.BinaryFiles)
                {

                }
            }

            await _dao.DeleteByIdAsync<JobRunnerEntity>(id);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateJobRunnerAsync([FromBody] JobRunnerEntity model)
        {
            var existingModel = await _dao.FindBySpecificationAsync<JobRunnerEntity>(x => x.Name == model.Name);
            if (existingModel.FirstOrDefault() != null)
            {
                return Conflict($"The name {model.Name} already exists.");
            }

            await _dao.AddAsync(model);

            return CreatedAtAction(nameof(GetJobRunnerByIdAsync), new { id = model.Id }, model.Id);
        }

    }
}
