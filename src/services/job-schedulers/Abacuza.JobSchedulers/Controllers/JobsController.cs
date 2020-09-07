using Abacuza.Common.DataAccess;
using Abacuza.JobSchedulers.Models;
using Abacuza.JobSchedulers.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        public JobsController(IScheduler quartzScheduler)
        {
            _quartzScheduler = quartzScheduler;
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

            var jobName = $"job-submit-{DateTime.UtcNow:yyyyMMddHHmmss}";


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
            return Ok();
        }
    }
}