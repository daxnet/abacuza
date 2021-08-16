using Abacuza.Common.DataAccess;
using Abacuza.Projects.ApiService.Models;
using Abacuza.Projects.ApiService.Services;
using DnsClient.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Abacuza.Projects.ApiService.Controllers
{
    [Route("api/revisions")]
    [ApiController]
    public class RevisionsController : ControllerBase
    {
        private readonly IDataAccessObject _dao;
        private readonly ILogger<RevisionsController> _logger;
        private readonly JobsApiService _jobsApiService;

        public RevisionsController(IDataAccessObject dao, ILogger<RevisionsController> logger, JobsApiService jobsApiService)
            => (_dao, _logger, _jobsApiService) = (dao, logger, jobsApiService);

        [HttpGet("{id}", Name = "GetRevisionById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Revision))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRevisionByIdAsync(Guid id)
        {
            var revision = await _dao.GetByIdAsync<Revision>(id);
            if (revision == null)
            {
                return NotFound($"The revision {id} doesn't exist.");
            }

            return Ok(revision);
        }

        [HttpGet("{id}/logs")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Revision))]
        public async Task<IActionResult> GetRevisionLogsByIdAsync(Guid id)
        {
            var revision = await _dao.GetByIdAsync<Revision>(id);
            if (revision == null)
            {
                return NotFound($"The revision {id} doesn't exist.");
            }

            if (!string.IsNullOrEmpty(revision.JobSubmissionName))
            {
                return Ok(await _jobsApiService.GetJobLogsBySubmissionName(revision.JobSubmissionName));
            }

            return BadRequest($"Revision {id} doesn't have a job submission name.");
        }
    }
}
