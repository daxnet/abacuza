using Abacuza.Common.DataAccess;
using Abacuza.Projects.ApiService.Models;
using DnsClient.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abacuza.Projects.ApiService.Controllers
{
    [Route("api/revisions")]
    [ApiController]
    public class RevisionsController : ControllerBase
    {
        private readonly IDataAccessObject _dao;
        private readonly ILogger<RevisionsController> _logger;

        public RevisionsController(IDataAccessObject dao, ILogger<RevisionsController> logger)
            => (_dao, _logger) = (dao, logger);

        [HttpGet("{id}", Name = "GetRevisionById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RevisionEntity))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRevisionByIdAsync(Guid id)
        {
            var revision = await _dao.GetByIdAsync<RevisionEntity>(id);
            if (revision == null)
            {
                return NotFound($"The revision {id} doesn't exist.");
            }

            return Ok(revision);
        }
    }
}
