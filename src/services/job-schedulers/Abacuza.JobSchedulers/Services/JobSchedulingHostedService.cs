using Abacuza.JobSchedulers.Models;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Services
{
    public class JobSchedulingHostedService : IHostedService
    {
        private readonly IScheduler _scheduler;
        private readonly IJobFactory _jobFactory;
        private static readonly JobKey jobUpdateExecutorJobKey = new JobKey("job-update-executor", "job-execution");
        private static readonly TriggerKey jobUpdateExecutorTriggerKey = new TriggerKey("job-update-executor-trigger", "job-execution");

        public JobSchedulingHostedService(IScheduler scheduler, IJobFactory jobFactory)
        {
            _scheduler = scheduler;
            _jobFactory = jobFactory;

            _scheduler.JobFactory = _jobFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var jobUpdateExecutorJobDetail = await _scheduler.GetJobDetail(jobUpdateExecutorJobKey, cancellationToken);
            if (jobUpdateExecutorJobDetail != null)
            {
                await _scheduler.DeleteJob(jobUpdateExecutorJobKey, cancellationToken);
            }

            jobUpdateExecutorJobDetail = JobBuilder.Create<JobUpdateExecutor>()
                .WithIdentity(jobUpdateExecutorJobKey)
                .Build();
            var jobUpdateExecutorTrigger = TriggerBuilder.Create()
                .WithIdentity(jobUpdateExecutorTriggerKey)
                .StartNow()
                .WithSimpleSchedule(s => s.WithIntervalInSeconds(15).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires())
                .Build();

            await _scheduler.ScheduleJob(jobUpdateExecutorJobDetail, jobUpdateExecutorTrigger, cancellationToken);

            await _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Shutdown(cancellationToken);
        }
    }
}
