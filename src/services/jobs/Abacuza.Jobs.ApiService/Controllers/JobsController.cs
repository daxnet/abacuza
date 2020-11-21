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

using Abacuza.Common.DataAccess;
using Abacuza.JobSchedulers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        #region Private Fields

        private const string JobGroupName = "job-execution";
        private readonly IDataAccessObject _dao;
        private readonly ILogger<JobsController> _logger;
        private readonly IScheduler _quartzScheduler;

        #endregion Private Fields

        #region Public Constructors

        public JobsController(IScheduler quartzScheduler, IDataAccessObject dao, ILogger<JobsController> logger)
            => (_quartzScheduler, _dao, _logger) = (quartzScheduler, dao, logger);

        #endregion Public Constructors

        #region Public Methods

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobEntity[]))]
        public async Task<IActionResult> GetAllJobsAsync()
            => Ok(await _dao.GetAllAsync<JobEntity>());

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

        // TODO: Pagination should be supported.
        [HttpGet("submissions/{submissionName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobEntity[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetJobBySubmissionNameAsync(string submissionName)
        {
            var jobEntity = (await _dao.FindBySpecificationAsync<JobEntity>(je =>
                je.SubmissionName == submissionName)).FirstOrDefault();
            if (jobEntity == null)
            {
                return NotFound($"The job submission {submissionName} doesn't exist.");
            }

            return Ok(jobEntity);
        }

        // TODO: Pagination should be supported.
        [HttpPost("submissions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobEntity[]))]
        public async Task<IActionResult> GetJobsBySubmissionNamesAsync([FromBody] IEnumerable<string> submissionNames)
        {
            Expression finalExpression = Expression.Constant(false);
            var parameterExpression = Expression.Parameter(typeof(JobEntity), "p");
            var propertyExpression = Expression.Property(parameterExpression, "SubmissionName");

            foreach (var name in submissionNames)
            {
                var equalsExpression = Expression.Equal(propertyExpression, Expression.Constant(name));
                finalExpression = Expression.OrElse(finalExpression, equalsExpression);
            }

            var lambda = Expression.Lambda<Func<JobEntity, bool>>(finalExpression, parameterExpression);
            var entities = await _dao.FindBySpecificationAsync(lambda);

            return Ok(entities);
        }

        [HttpPost("submit")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
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
            return CreatedAtAction(nameof(GetJobBySubmissionNameAsync), new { submissionName = jobName }, jobName);
        }

        #endregion Public Methods
    }
}