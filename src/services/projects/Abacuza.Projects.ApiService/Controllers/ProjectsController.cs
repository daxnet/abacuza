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
// Licensed under LGPL-v3
// ==============================================================

using Abacuza.Common.DataAccess;
using Abacuza.Common.Utilities;
using Abacuza.Projects.ApiService.Models;
using Abacuza.Projects.ApiService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Abacuza.Projects.ApiService.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        #region Private Fields

        private const string MaxReservedRevisionsConfigKey = "options:maxReservedRevisions";
        private const int DefaultMaxReservedRevisions = 20;

        private readonly IDataAccessObject _dao;
        private readonly JobsApiService _jobsApiService;
        private readonly ILogger<ProjectsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly int maxReservedRevisions;

        private readonly static IEnumerable<Func<string, ProjectEntity, JobRunner, string>> PayloadTemplateFuncs
             = new List<Func<string, ProjectEntity, JobRunner, string>>
             {
                 // replace binary file names defined in the job runner.
                 (input, project, jobRunner) =>
                 {
                     var regex = new Regex(@"\${jr:binaries:(?<filename>[\w\._-]+)}");
                     var matches = regex.Matches(input);
                     foreach (Match m in matches)
                     {
                         var matchedFileName = m.Groups["filename"].Value;
                         var s3File = jobRunner.BinaryFiles.FirstOrDefault(f => f.File == matchedFileName);
                         if (s3File != null)
                         {
                             input = input.Replace(m.Value, s3File.ToString());
                         }
                     }

                     return input;
                 },

                 // replace the input endpoint name.
                 (input, project, jobRunner) =>
                    input.Replace("${proj:input-endpoint}", $"input_endpoint:{project.InputEndpointName}"),

                 // replace the input endpoint settings.
                 (input, project, jobRunner) =>
                    input.Replace("${proj:input-endpoint-settings}", $"input_endpoint_settings:{project.InputEndpointSettings.Replace("\"", "\\\"").Replace("\r\n", "")}")
             };

        #endregion Private Fields

        #region Public Constructors

        public ProjectsController(IDataAccessObject dao,
            ILogger<ProjectsController> logger,
            JobsApiService jobsApiService,
            IConfiguration configuration)
        {
            (_jobsApiService, _dao, _logger, _configuration) = (jobsApiService, dao, logger, configuration);
            maxReservedRevisions = configuration[MaxReservedRevisionsConfigKey] == null ? 
                DefaultMaxReservedRevisions : 
                Convert.ToInt32(configuration[MaxReservedRevisionsConfigKey]);
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

        [HttpPost("{projectId}/revisions")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateProjectRevisionAsync(Guid projectId)
        {
            var project = await _dao.GetByIdAsync<ProjectEntity>(projectId);
            if (project == null)
            {
                return NotFound($"Project {projectId} doesn't exist.");
            }

            var revision = new RevisionEntity
            {
                ProjectId = projectId,
                CreatedDate = DateTime.UtcNow
            };

            // firstly get the job runner and then the payload template.
            var jobRunner = await _jobsApiService.GetJobRunnerByIdAsync(project.JobRunnerId);
            var payload = jobRunner.PayloadTemplate;

            // then normalize the payload.
            foreach (var payloadTemplateFunc in PayloadTemplateFuncs)
            {
                payload = payloadTemplateFunc(payload, project, jobRunner);
            }

            // and then create the job and get the job submission name.
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload);
            var jobName = await _jobsApiService.SubmitJobAsync(jobRunner.ClusterType, dict);
            revision.JobSubmissionName = jobName;

            // save the revision.
            await _dao.AddAsync(revision);
            return CreatedAtRoute("GetRevisionById", new
            {
                id = revision.Id
            }, revision.Id);
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

        [HttpGet("{projectId}/revisions/jobs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectRevisionJobsAsync(Guid projectId)
        {
            var project = await _dao.GetByIdAsync<ProjectEntity>(projectId);
            if (project == null)
            {
                return NotFound($"Project {projectId} doesn't exist.");
            }

            var revisions = await _dao.FindBySpecificationAsync<RevisionEntity>(r => r.ProjectId == projectId);
            var jobSubmissionNames = revisions.Select(r => r.JobSubmissionName).Where(n => !string.IsNullOrEmpty(n));
            return Ok(await _jobsApiService.GetJobsBySubmissionNames(jobSubmissionNames));
        }

        [HttpGet("{projectId}/revisions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectRevisionsAsync(Guid projectId, [FromQuery(Name = "job-info")] bool jobInfo = false)
        {
            var project = await _dao.GetByIdAsync<ProjectEntity>(projectId);
            if (project == null)
            {
                return NotFound($"Project {projectId} doesn't exist.");
            }

            var revisions = (await _dao.FindBySpecificationAsync<RevisionEntity>(r => r.ProjectId == projectId))
                .OrderByDescending(r => r.CreatedDate)
                .Take(maxReservedRevisions);
            if (jobInfo)
            {
                var jobSubmissionNames = revisions.Select(r => r.JobSubmissionName).Where(n => !string.IsNullOrEmpty(n));
                var jobs = await _jobsApiService.GetJobsBySubmissionNames(jobSubmissionNames);
                var revisionsWithJobs = from revision in revisions
                                        join j in jobs on revision.JobSubmissionName equals j.SubmissionName into g
                                        from job in g.DefaultIfEmpty()
                                        select new
                                        {
                                            revision.Id,
                                            revision.ProjectId,
                                            revision.JobSubmissionName,
                                            revision.CreatedDate,
                                            JobCancelledDate = job?.CancelledDate,
                                            JobCompletedDate = job?.CompletedDate,
                                            JobConnectionId = job?.ConnectionId,
                                            JobCreatedDate = job?.CreatedDate,
                                            JobFailedDate = job?.FailedDate,
                                            JobId = job?.Id,
                                            JobStatusName = job?.JobStatusName ?? Enum.GetName(JobState.Created),
                                            JobName = job?.Name,
                                            JobState = job?.State ?? JobState.Created,
                                        };
                return Ok(revisionsWithJobs);
            }
            else
            {
                return Ok(revisions);
            }
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

        #endregion Public Methods
    }
}