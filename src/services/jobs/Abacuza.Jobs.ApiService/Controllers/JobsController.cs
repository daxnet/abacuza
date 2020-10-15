using Abacuza.Common.DataAccess;
using Abacuza.JobSchedulers.Models;
using Abacuza.JobSchedulers.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private const string JobGroupName = "job-execution";
        private readonly IScheduler _quartzScheduler;
        private readonly IDataAccessObject _dao;
        private readonly ILogger<JobsController> _logger;

        public JobsController(IScheduler quartzScheduler, IDataAccessObject dao, ILogger<JobsController> logger)
        {
            _quartzScheduler = quartzScheduler;
            _dao = dao;
            _logger = logger;
        }
        
        [HttpPost("submit")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SubmitJobAsync([FromBody] SubmitJobRequest request)
        {
            if (string.IsNullOrEmpty(request.Type))
            {
                return BadRequest($"The type of the job is not specified.");
            }

            var jobName = $"job-submit-{DateTime.UtcNow:yyyyMMddHHmmss-fff}";


            var jobDetail = JobBuilder.Create<JobSubmitExecutor>()
                .WithIdentity(new JobKey(jobName, JobGroupName))
                .WithDescription("")
                .SetJobData(new JobDataMap
                {
                    { "clusterType", request.Type },
                    { "properties", request.Properties }
                })
                .Build();

            var jobTrigger = TriggerBuilder.Create()
                .WithIdentity(new TriggerKey($"trigger-{jobName}", JobGroupName))
                .StartNow() // TODO: Apply advanced scheduling mechanism.
                .Build();

            await _quartzScheduler.ScheduleJob(jobDetail, jobTrigger);
            var message = $"The job has been submitted successfully, scheduled job submission ID: {jobName}";
            _logger.LogInformation(message);
            return Ok(message);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobEntity[]))]
        public async Task<IActionResult> GetAllJobsAsync()
            => Ok(await _dao.GetAllAsync<JobEntity>()); // TODO: Pagination should be supported.

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobEntity))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetJobByIdAsync(Guid id)
        {
            var jobEntity = await _dao.GetByIdAsync<JobEntity>(id);
            if (jobEntity == null)
            {
                return NotFound($"The job {id} doesn't exist.");
            }

            return Ok(jobEntity);
        }

        [HttpGet("submissions/{submissionName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobEntity[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetJobBySubmissionNameAsync(string submissionName)
        {
            var jobEntities = await _dao.FindBySpecificationAsync<JobEntity>(je =>
                je.SubmissionName == submissionName);
            if (jobEntities == null)
            {
                return NotFound($"The job submission {submissionName} doesn't exist.");
            }

            return Ok(jobEntities);
        }
    }
}