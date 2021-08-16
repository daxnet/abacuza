using System.Threading;
using System.Threading.Tasks;
using Abacuza.Jobs.ApiService.Models;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

namespace Abacuza.Jobs.ApiService.Services
{
    public class JobSchedulingHostedService : IHostedService
    {
        private readonly IScheduler _scheduler;
        private static readonly JobKey JobUpdateExecutorJobKey = new JobKey("job-update-executor", "job-execution");
        private static readonly TriggerKey JobUpdateExecutorTriggerKey = new TriggerKey("job-update-executor-trigger", "job-execution");

        public JobSchedulingHostedService(IScheduler scheduler, IJobFactory jobFactory)
        {
            _scheduler = scheduler;
            _scheduler.JobFactory = jobFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var jobUpdateExecutorJobDetail = await _scheduler.GetJobDetail(JobUpdateExecutorJobKey, cancellationToken);
            if (jobUpdateExecutorJobDetail != null)
            {
                await _scheduler.DeleteJob(JobUpdateExecutorJobKey, cancellationToken);
            }

            jobUpdateExecutorJobDetail = JobBuilder.Create<JobUpdateExecutor>()
                .WithIdentity(JobUpdateExecutorJobKey)
                .Build();
            var jobUpdateExecutorTrigger = TriggerBuilder.Create()
                .WithIdentity(JobUpdateExecutorTriggerKey)
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
