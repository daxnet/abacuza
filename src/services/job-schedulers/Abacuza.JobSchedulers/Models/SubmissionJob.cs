using Abacuza.JobSchedulers.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Models
{
    [DisallowConcurrentExecution]
    public class SubmissionJob : IJob
    {
        private readonly ILogger<SubmissionJob> _logger;
        private readonly ClusterApiService _clusterService;

        public SubmissionJob(ILogger<SubmissionJob> logger,
            ClusterApiService clusterService)
        {
            _logger = logger;
            _clusterService = clusterService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
