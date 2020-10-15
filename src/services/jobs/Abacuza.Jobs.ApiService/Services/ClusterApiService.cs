﻿using Abacuza.JobSchedulers.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Services
{
    /// <summary>
    /// Represents the service client that communicates with the cluster service and
    /// performs clusters, jobs and cluster connections tasks.
    /// </summary>
    public sealed class ClusterApiService
    {
        private const string ClusterServiceUrlConfigurationKey = "services:clusterService:url";
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly Uri _clusterApiBaseUri;

        /// <summary>
        /// Initializes a new instance of the <c>ClusterApiService</c> class.
        /// </summary>
        /// <param name="httpClient">The http client instance for communicating with the cluster service.</param>
        /// <param name="configuration">The instance that holds the configuration information.</param>
        public ClusterApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _clusterApiBaseUri = new Uri(_configuration[ClusterServiceUrlConfigurationKey]);
        }


        public async Task<JobEntity> SubmitJobAsync(string clusterType, IEnumerable<KeyValuePair<string, object>> properties, CancellationToken cancellationToken = default)
        {
            var submitJobUrl = new Uri(_clusterApiBaseUri, "api/jobs/submit");
            var payloadJson = JsonConvert.SerializeObject(new
            {
                clusterType,
                properties
            });

            var responseMessage = await _httpClient.PostAsync(submitJobUrl, new StringContent(payloadJson, Encoding.UTF8, "application/json"));
            responseMessage.EnsureSuccessStatusCode();
            var jsonObj = JObject.Parse(await responseMessage.Content.ReadAsStringAsync());
            var jobEntity = new JobEntity
            {
                ConnectionId = Guid.Parse(jsonObj["connectionId"]?.Value<string>()),
                LocalJobId = jsonObj["localJobId"]?.Value<string>(),
                Name = jsonObj["name"]?.Value<string>(),
                State = JobState.Created,
                CreatedDate = DateTime.UtcNow,
                Traceability = JobTraceability.Tracked,
                TracingFailures = 0
            };

            return jobEntity;
        }

        public async Task<IEnumerable<JobStatusEntity>> GetJobStatusesAsync(IEnumerable<KeyValuePair<Guid?, IEnumerable<string>>> jobIdentifiers, CancellationToken cancellationToken = default)
        {
            var url = new Uri(_clusterApiBaseUri, "api/jobs/statuses");
            var payloadJson = JsonConvert.SerializeObject(jobIdentifiers.Select(kvp =>
                new 
                {
                    connectionId = kvp.Key,
                    localJobIdentifiers = kvp.Value
                }));

            var responseMessage = await _httpClient.PostAsync(url, 
                new StringContent(payloadJson, Encoding.UTF8, "application/json"),
                cancellationToken);

            responseMessage.EnsureSuccessStatusCode();
            var responseJson = await responseMessage.Content.ReadAsStringAsync();
            var response = JArray.Parse(responseJson);
            var jobStatusEntities = new List<JobStatusEntity>();
            foreach (var item in response)
            {
                jobStatusEntities.Add(new JobStatusEntity
                {
                    ConnectionId = Guid.Parse(item["connectionId"].Value<string>()),
                    LocalJobId = item["localJobId"].Value<string>(),
                    State = (JobState)item["state"].Value<int>(),
                    Logs = item["logs"].ToObject<List<string>>(),
                    Succeeded = item["succeeded"].Value<bool>()
                });
            }

            return jobStatusEntities;
        }
    }
}