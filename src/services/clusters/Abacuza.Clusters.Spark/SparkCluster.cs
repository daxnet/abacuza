using Abacuza.Clusters.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abacuza.Clusters.Spark
{
    [Cluster("B22A9E13-CE6E-4927-868D-DBD215B456E4", "spark", "Apache Spark", typeof(SparkClusterConnection), Description = "Apache Spark is a unified analytics engine for large-scale data processing.")]
    public sealed class SparkCluster : Cluster
    {
        private bool disposed;
        private readonly HttpClient _httpClient = new HttpClient();

        public override async Task<ClusterState> GetStateAsync(IClusterConnection connection, CancellationToken cancellationToken = default)
        {
            try
            {
                var connectionInformation = connection.As<SparkClusterConnection>();
                using var responseMessage = await _httpClient.GetAsync(connectionInformation.BaseUrl, cancellationToken);
                responseMessage.EnsureSuccessStatusCode();

                return ClusterState.Online;
            }
            catch
            {
                return ClusterState.Offline;
            }
        }

        public override async Task<ClusterJob> SubmitJobAsync(IClusterConnection connection, IEnumerable<KeyValuePair<string, object>> properties, CancellationToken cancellationToken = default)
        {
            var connectionInformation = connection.As<SparkClusterConnection>();

            dynamic payload = new ExpandoObject();
            foreach (var kvp in properties)
            {
                ((IDictionary<string, object>)payload)[kvp.Key] = kvp.Value;
            }

            if (connectionInformation.Properties?.Count() > 0)
            {
                var conf = new Dictionary<string, object>(connectionInformation.Properties);
                ((IDictionary<string, object>)payload)["conf"] = conf;
            }

            var payloadJson = JsonConvert.SerializeObject(payload);

            var postBatchesUrl = BuildEndpointUrl(connectionInformation.BaseUrl, "batches");
            using var responseMessage = await _httpClient.PostAsync(postBatchesUrl,
                new StringContent(payloadJson, Encoding.UTF8, "application/json"),
                cancellationToken);

            if (responseMessage.StatusCode == HttpStatusCode.Created)
            {
                var responseJson = await responseMessage.Content.ReadAsStringAsync();
                var responseObj = JObject.Parse(responseJson);
                var sparkBatchId = responseObj["id"].Value<int>();
                var sparkBatchName = responseObj["name"]?.Value<string>();
              
                return new ClusterJob
                {
                    ConnectionId = connectionInformation.Id,
                    LocalJobId = sparkBatchId.ToString(),
                    Name = sparkBatchName,
                    State = ClusterJobState.Created
                };
            }
            else
            {
                var errorMessage = await responseMessage.Content.ReadAsStringAsync();
                throw new ClusterJobException($"Failed to create the job. Details: {errorMessage}");
            }
        }

        public override async Task<ClusterJob> GetJobAsync(IClusterConnection connection, string localJobId, CancellationToken cancellationToken = default)
        {
            var connectionInformation = connection.As<SparkClusterConnection>();
            var retrieveBatchUrl = BuildEndpointUrl(connectionInformation.BaseUrl, $"batches/{localJobId}");
            HttpResponseMessage responseMessage;
            using (responseMessage = await _httpClient.GetAsync(retrieveBatchUrl, cancellationToken))
            {
                try
                {

                    responseMessage.EnsureSuccessStatusCode();
                    var responseJson = await responseMessage.Content.ReadAsStringAsync();
                    var responseObj = JObject.Parse(responseJson);
                    var jobName = responseObj["name"]?.Value<string>();
                    var jobState = ConvertToJobState(responseObj["state"]?.Value<string>());

                    var retrieveBatchLogUrl = BuildEndpointUrl(connectionInformation.BaseUrl, $"batches/{localJobId}/log");
                    responseMessage = await _httpClient.GetAsync(retrieveBatchLogUrl);
                    responseMessage.EnsureSuccessStatusCode();
                    var logResponseJson = await responseMessage.Content.ReadAsStringAsync();
                    var logResponseObj = JObject.Parse(logResponseJson);
                    var jobLogs = logResponseObj["log"]?.ToObject<string[]>();

                    return new ClusterJob(connection.Id, localJobId)
                    {
                        Name = jobName,
                        State = jobState,
                        Logs = jobLogs.ToList()
                    };

                }
                catch (HttpRequestException ex) when (responseMessage?.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ClusterJobException($"The job {localJobId} doesn't exist on cluster {connection.Id}.", ex);
                }
                catch (Exception ex)
                {
                    throw new ClusterJobException(ex.Message, ex);
                }
            }
        }

        private static Uri BuildEndpointUrl(string baseUrl, string relativeUrl)
            => new Uri(new Uri(baseUrl), relativeUrl);

        private static ClusterJobState ConvertToJobState(string jobStateValue) => jobStateValue?.ToLower() switch
        {
            "not_started" => ClusterJobState.Created,
            "starting" => ClusterJobState.Initializing,
            "running" => ClusterJobState.Running,
            "busy" => ClusterJobState.Busy,
            "error" => ClusterJobState.Failed,
            "dead" => ClusterJobState.Failed,
            "killed" => ClusterJobState.Cancelled,
            "success" => ClusterJobState.Completed,
            _ => ClusterJobState.Unknown
        };

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                }

                disposed = true;
            }
        }
    }
}
