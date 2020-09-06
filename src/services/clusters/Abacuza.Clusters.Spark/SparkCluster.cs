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
    [Cluster("B22A9E13-CE6E-4927-868D-DBD215B456E4", "spark", "Apache Spark", typeof(SparkClusterConnection))]
    public sealed class SparkCluster : Cluster
    {
        private bool disposed;
        private readonly HttpClient _httpClient = new HttpClient();

        public override async Task<ClusterState> GetStateAsync(IClusterConnection connection, CancellationToken cancellationToken = default)
        {
            try
            {
                var connectionInformation = connection.As<SparkClusterConnection>();
                var responseMessage = await _httpClient.GetAsync(connectionInformation.BaseUrl, cancellationToken);
                responseMessage.EnsureSuccessStatusCode();

                return ClusterState.Online;
            }
            catch
            {
                return ClusterState.Offline;
            }
        }

        public override async Task<Job> SubmitJobAsync(IClusterConnection connection, IDictionary<string, object> properties, CancellationToken cancellationToken = default)
        {
            var connectionInformation = connection.As<SparkClusterConnection>();
            dynamic payload = new ExpandoObject();
            foreach(var property in properties)
            {
                ((IDictionary<string, object>)payload)[property.Key] = property.Value;
            }

            if (connectionInformation.Properties?.Count() > 0)
            {
                ((IDictionary<string, object>)payload)["conf"] = new Dictionary<string, object>(connectionInformation.Properties);
            }

            var payloadJson = JsonConvert.SerializeObject(payload);
            var postBatchesUrl = new Uri(new Uri(connectionInformation.BaseUrl), "batches");
            var responseMessage = await _httpClient.PostAsync(postBatchesUrl,
                new StringContent(payloadJson, Encoding.UTF8, "application/json"),
                cancellationToken);

            if (responseMessage.StatusCode == HttpStatusCode.Created)
            {
                var responseJson = await responseMessage.Content.ReadAsStringAsync();
                var responseObj = JObject.Parse(responseJson);
                var sparkBatchId = responseObj["id"].Value<int>();
                var sparkBatchName = responseObj["name"]?.Value<string>();
              
                return new Job
                {
                    ConnectionId = connectionInformation.Id,
                    LocalJobId = sparkBatchId.ToString(),
                    Created = DateTime.UtcNow,
                    Name = sparkBatchName
                };
            }
            else
            {
                var errorMessage = await responseMessage.Content.ReadAsStringAsync();
                throw new JobException($"Failed to create the job. Details: {errorMessage}");
            }
        }

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
