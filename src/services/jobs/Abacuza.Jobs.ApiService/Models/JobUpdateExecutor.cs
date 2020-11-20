using Abacuza.Common.DataAccess;
using Abacuza.JobSchedulers.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Models
{
    [DisallowConcurrentExecution]
    public sealed class JobUpdateExecutor : IJob
    {
        private readonly IDataAccessObject _dao;
        private readonly ILogger<JobUpdateExecutor> _logger;
        private readonly ClusterApiService _clusterService;

        public JobUpdateExecutor(IDataAccessObject dao, ILogger<JobUpdateExecutor> logger, ClusterApiService clusterService)
            => (_dao, _logger, _clusterService) = (dao, logger, clusterService);

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var jobEntities = await _dao.FindBySpecificationAsync<JobEntity>(je =>
                    je.State < JobState.Completed &&
                    je.Traceability == JobTraceability.Tracked);

                if (!jobEntities.Any())
                {
                    return;
                }

                var getStatusRequest = from je in jobEntities
                                       group je by je.ConnectionId into getStatusGroup
                                       select new KeyValuePair<Guid?, IEnumerable<string>>
                                       (
                                           getStatusGroup.Key,
                                           getStatusGroup.Select(je => je.LocalJobId)
                                       );

                var jobStatusEntities = await _clusterService.GetJobStatusesAsync(getStatusRequest, context.CancellationToken);
                foreach (var jobStatusEntity in jobStatusEntities)
                {
                    var jobEntity = jobEntities.FirstOrDefault(x => x.ConnectionId == jobStatusEntity?.ConnectionId &&
                        x.LocalJobId == jobStatusEntity?.LocalJobId);
                    if (jobEntity != null)
                    {
                        if (jobStatusEntity.Succeeded)
                        {
                            jobEntity.State = jobStatusEntity.State;
                            jobEntity.Logs = jobStatusEntity.Logs;
                            switch (jobStatusEntity.State)
                            {
                                case JobState.Cancelled:
                                    jobEntity.CancelledDate = DateTime.UtcNow;
                                    break;
                                case JobState.Completed:
                                    jobEntity.CompletedDate = DateTime.UtcNow;
                                    break;
                                case JobState.Failed:
                                    jobEntity.FailedDate = DateTime.UtcNow;
                                    break;
                            }
                        }
                        else
                        {
                            jobEntity.TracingFailures = jobEntity.TracingFailures.HasValue ? jobEntity.TracingFailures++ : 1;

                            if (jobEntity.TracingFailures > 5) // TODO: Need a better strategy of handling the tracing failures
                            {
                                jobEntity.Traceability = JobTraceability.Untracked;
                            }
                        }

                        await _dao.UpdateByIdAsync(jobEntity.Id, jobEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}
