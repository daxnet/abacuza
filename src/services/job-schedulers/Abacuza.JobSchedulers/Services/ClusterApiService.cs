using Abacuza.JobSchedulers.Models;
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
    public class ClusterApiService
    {
        private const string ClusterServiceUrlConfigurationKey = "CLUSTER_SERVICE_URL";
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly Uri _clusterApiBaseUri;

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
                Created = DateTime.UtcNow,
                State = JobState.Created
            };

            return jobEntity;
        }

        public async Task GetJobStatusesAsync(string payloadJson)
        {
            var url = new Uri(_clusterApiBaseUri, "api/jobs/statuses");
            var responseMessage = await _httpClient.PostAsync(url, new StringContent(payloadJson, Encoding.UTF8, "application/json"));
        }
    }
}
