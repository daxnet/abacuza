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
// Licensed under LGPL-v3
// ==============================================================

using Abacuza.Common;
using Abacuza.Common.Models;
using Abacuza.Projects.ApiService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abacuza.Projects.ApiService.Services
{
    public sealed class JobsApiService
    {

        #region Private Fields

        private const string JobsServiceUrlConfigurationKey = "services:jobsService:url";
        private readonly HttpClient _httpClient;
        private readonly Uri _jobsApiBaseUri;
        private readonly ILogger<JobsApiService> _logger;

        #endregion Private Fields

        #region Public Constructors

        public JobsApiService(HttpClient httpClient, IConfiguration configuration, ILogger<JobsApiService> logger) => (_httpClient, _jobsApiBaseUri, _logger) =
                (httpClient, new Uri(configuration[JobsServiceUrlConfigurationKey]), logger);

        #endregion Public Constructors

        #region Internal Methods

        internal async Task<IEnumerable<string>> GetJobLogsBySubmissionName(string submissionName, CancellationToken cancellationToken = default)
        {
            var url = new Uri(_jobsApiBaseUri, $"api/jobs/submissions/{submissionName}");
            using var responseMessage = await _httpClient.GetAsync(url, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            try
            {
                var result = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                var responseObject = JObject.Parse(result);
                var logs = responseObject["logs"].ToObject<List<string>>();
                return logs;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            
        }

        internal async Task<JobRunner> GetJobRunnerByIdAsync(Guid jobRunnerId, CancellationToken cancellationToken = default)
        {
            var getJobRunnerByIdUrl = new Uri(_jobsApiBaseUri, $"api/job-runners/{jobRunnerId}");
            using var responseMessage = await _httpClient.GetAsync(getJobRunnerByIdUrl, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            var jsonObj = JObject.Parse(await responseMessage.Content.ReadAsStringAsync(cancellationToken));
            var binaries = jsonObj["binaryFiles"]?.ToObject<List<S3File>>();
            var jobRunner = new JobRunner
            (
                Guid.Parse(jsonObj["id"]?.Value<string>()),
                jsonObj["name"]?.Value<string>(),
                jsonObj["description"]?.Value<string>(),
                jsonObj["clusterType"]?.Value<string>(),
                jsonObj["payloadTemplate"]?.Value<string>(),
                binaries
            );

            return jobRunner;
        }

        internal async Task<IEnumerable<Job>> GetJobsBySubmissionNames(IEnumerable<string> submissionNames, CancellationToken cancellationToken = default)
        {
            var url = new Uri(_jobsApiBaseUri, "api/jobs/submissions");
            var payload = JsonConvert.SerializeObject(submissionNames);
            using var responseMessage = await _httpClient.PostAsync(url,
                new StringContent(payload, Encoding.UTF8, "application/json"),
                cancellationToken);

            responseMessage.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Job[]>(await responseMessage.Content.ReadAsStringAsync(cancellationToken));
        }

        internal async Task<string> SubmitJobAsync(string clusterType, IEnumerable<KeyValuePair<string, object>> properties, CancellationToken cancellationToken = default)
        {
            var submitUrl = new Uri(_jobsApiBaseUri, "api/jobs/submit");
            var payloadJson = JsonConvert.SerializeObject(new
            {
                type = clusterType,
                properties
            });

            using var responseMessage = await _httpClient.PostAsync(submitUrl,
                new StringContent(payloadJson, Encoding.UTF8, "application/json"),
                cancellationToken);

            if (responseMessage.StatusCode == HttpStatusCode.Created)
            {
                return await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            }

            throw new ServiceInvocationException(responseMessage.StatusCode);
        }

        #endregion Internal Methods

    }
}
