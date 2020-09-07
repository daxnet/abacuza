using Abacuza.Common.DataAccess;
using Abacuza.JobSchedulers.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Abacuza.JobSchedulers.Models
{
    public class JobSubmitExecutor : IJob
    {
        private readonly ILogger<JobSubmitExecutor> _logger;
        private readonly ClusterApiService _clusterService;
        private readonly IDataAccessObject _dao;
        

        public JobSubmitExecutor(ILogger<JobSubmitExecutor> logger,
            ClusterApiService clusterService,
            IDataAccessObject dao)
        {
            _logger = logger;
            _clusterService = clusterService;
            _dao = dao;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (!context.MergedJobDataMap.ContainsKey("clusterType"))
            {
                throw new ArgumentException();
            }

            if (!context.MergedJobDataMap.ContainsKey("properties"))
            {
                throw new ArgumentException();
            }

            var clusterType = context.MergedJobDataMap["clusterType"].ToString();
            var properties = context.MergedJobDataMap["properties"] as IDictionary<string, object>;
            _logger.LogDebug("Execute the job with the cluster type of '{0}'", clusterType);
            
            var jobEntity = await _clusterService.SubmitJobAsync(clusterType, properties, context.CancellationToken);
            await _dao.AddAsync(jobEntity);

        }
    }
}
