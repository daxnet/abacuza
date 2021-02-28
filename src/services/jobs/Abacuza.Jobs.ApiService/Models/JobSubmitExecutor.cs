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
// Apache License Version 2.0
// ==============================================================

using Abacuza.Common.DataAccess;
using Abacuza.JobSchedulers.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Models
{
    public sealed class JobSubmitExecutor : IJob
    {
        private readonly ClusterApiService _clusterService;
        private readonly IDataAccessObject _dao;
        private readonly ILogger<JobSubmitExecutor> _logger;

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
            var jobName = context?.JobDetail?.Key?.Name;
            
            if (string.IsNullOrEmpty(jobName))
            {
                _logger.LogError("No name defined on the JobDetail.");
                return;
            }

            if (!context?.MergedJobDataMap.ContainsKey("clusterType") ?? true)
            {
                _logger.LogError($"Failed to start the job, the clusterType is not specified in the job data. Execution ID: {jobName}");
                return;
            }

            try
            {
                var clusterType = context?.MergedJobDataMap["clusterType"].ToString()!;
                IDictionary<string, object> properties;
                if (context?.MergedJobDataMap.ContainsKey("properties") ?? false)
                {
                    properties = context.MergedJobDataMap["properties"] as IDictionary<string, object> ?? new Dictionary<string, object>();
                }
                else
                {
                    properties = new Dictionary<string, object>();
                }

                _logger.LogInformation($"Submitting job, Execution ID: {jobName}, Cluster Type: {clusterType}");

                var jobEntity = await _clusterService.SubmitJobAsync(clusterType, properties, context?.CancellationToken ?? default);

                if (jobEntity.State == JobState.Created)
                {
                    _logger.LogInformation($"Job {jobName} has been successfully submitted to the cluster whose type is {clusterType}");
                }
                else
                {
                    _logger.LogError($"Job {jobName} created failed, check log for more information.");
                    _logger.LogError(string.Join(Environment.NewLine, jobEntity.Logs.ToArray()));
                }

                jobEntity.SubmissionName = jobName;
                await _dao.AddAsync(jobEntity);

                _logger.LogInformation($"Job {jobName} has been saved successfully, Job ID: {jobEntity.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to submit the job to the cluster, Execution ID: {jobName}");
            }
        }
    }
}