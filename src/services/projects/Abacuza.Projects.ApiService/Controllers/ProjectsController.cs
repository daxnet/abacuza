using Abacuza.Common.DataAccess;
using Abacuza.Projects.ApiService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Projects.ApiService.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly IDataAccessObject _dao;

        public ProjectsController(IDataAccessObject dao, ILogger<ProjectsController> logger)
        {
            _dao = dao;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectEntity[]))]
        public async Task<IActionResult> GetAllProjectsAsync()
            => Ok(await _dao.GetAllAsync<ProjectEntity>());

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectEntity))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectByIdAsync(Guid id)
        {
            var project = await _dao.GetByIdAsync<ProjectEntity>(id);
            if (project == null)
            {
                return NotFound($"The project {id} doesn't exist.");
            }

            return Ok(project);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectEntity projectEntity)
        {
            var existingModel = await _dao.FindBySpecificationAsync<ProjectEntity>(p => p.Name == projectEntity.Name);
            if (existingModel.FirstOrDefault() != null)
            {
                return Conflict($"Project '{projectEntity.Name}' already exists.");
            }

            projectEntity.DateCreated = DateTime.UtcNow;

            await _dao.AddAsync(projectEntity);

            return CreatedAtAction(nameof(GetProjectByIdAsync), new { id = projectEntity.Id }, projectEntity.Id);
        }
    }
}
