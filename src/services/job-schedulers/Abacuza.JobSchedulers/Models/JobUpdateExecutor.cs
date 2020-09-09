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
    public class JobUpdateExecutor : IJob
    {
        private readonly IDataAccessObject _dao;
        private readonly ILogger<JobUpdateExecutor> _logger;
        private readonly ClusterApiService _clusterService;

        public JobUpdateExecutor(IDataAccessObject dao, ILogger<JobUpdateExecutor> logger, ClusterApiService clusterService)
        {
            _dao = dao;
            _logger = logger;
            _clusterService = clusterService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobEntities = await _dao.FindBySpecificationAsync<JobEntity>(je => je.State < JobState.Completed);
            var getStatusRequest = from je in jobEntities
                                   group je by je.ConnectionId into getStatusGroup
                                   select new
                                   {
                                       connectionId = getStatusGroup.Key,
                                       localJobIdentifiers = getStatusGroup.Select(je => je.LocalJobId)
                                   };

            var json = JsonConvert.SerializeObject(getStatusRequest);
            await _clusterService.GetJobStatusesAsync(json);
        }
    }
}
