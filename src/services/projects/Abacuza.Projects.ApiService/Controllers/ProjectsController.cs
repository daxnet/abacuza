using Abacuza.Common.DataAccess;
using Abacuza.Projects.ApiService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        #region Private Fields

        private readonly IDataAccessObject _dao;
        private readonly ILogger<ProjectsController> _logger;

        #endregion Private Fields

        #region Public Constructors

        public ProjectsController(IDataAccessObject dao, ILogger<ProjectsController> logger)
        {
            _dao = dao;
            _logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProjectByIdAsync(Guid id)
        {
            var project = await _dao.GetByIdAsync<ProjectEntity>(id);
            if (project == null)
            {
                return NotFound($"The project {id} doesn't exist.");
            }

            await _dao.DeleteByIdAsync<ProjectEntity>(id);
            return Ok();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchProjectAsync(Guid id, [FromBody] JsonPatchDocument<ProjectEntity> patchDoc)
        {
            var updatingEntity = await _dao.GetByIdAsync<ProjectEntity>(id);
            if (updatingEntity == null)
            {
                return NotFound($"Project {id} doesn't exist.");
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

        [HttpGet("{projectId}/revisions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectRevisionsAsync(Guid projectId)
        {
            var project = await _dao.GetByIdAsync<ProjectEntity>(projectId);
            if (project == null)
            {
                return NotFound($"Project {projectId} doesn't exist.");
            }

            var revisions = await _dao.FindBySpecificationAsync<RevisionEntity>(r => r.ProjectId == projectId);
            return Ok(revisions);
        }

        #endregion Public Methods
    }
}
