using Abacuza.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abacuza.Projects.ApiService.Services
{
    public sealed class JobsApiService
    {
        private const string JobsServiceUrlConfigurationKey = "services:jobsService:url";
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly Uri _jobsApiBaseUri;

        public JobsApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _jobsApiBaseUri = new Uri(_configuration[JobsServiceUrlConfigurationKey]);
        }

        public async Task<string> SubmitJobAsync(string clusterType, IEnumerable<KeyValuePair<string, object>> properties, CancellationToken cancellationToken = default)
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
    }
}
