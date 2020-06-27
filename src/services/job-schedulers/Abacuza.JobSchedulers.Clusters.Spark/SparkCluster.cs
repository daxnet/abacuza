using System.Linq;
using System.Net.Http;
using System.Threading;
using System;
using System.Threading.Tasks;
using Abacuza.JobSchedulers.Common.Models;
using Abacuza.Common;
using Newtonsoft.Json;
using Abacuza.JobSchedulers.Clusters.Spark.Models;

namespace Abacuza.JobSchedulers.Clusters.Spark
{
    [AbacuzaCluster("fbc7b771-1053-44b1-bb1b-5c162b5fd91b", "spark")]
    public sealed class SparkCluster : Cluster<SparkClusterConnection>
    {
        public override async Task<PagedResult<JobResponse>> GetJobsAsync(SparkClusterConnection connection, int pageNumber = 0, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(connection.Uri);
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"batches?from={pageNumber * pageSize}&size={pageSize}");
                var responseMessage = await httpClient.SendAsync(requestMessage, cancellationToken);
                var batchResponse = JsonConvert.DeserializeObject<GetBatchesResponse>(await responseMessage.Content.ReadAsStringAsync());
                var jobs = batchResponse.Batches.Select(b => new JobResponse
                {
                    Id = b.Id.ToString(),
                    Logs = b.Logs.ToArray(),
                    State = b.State
                });

                return new PagedResult<JobResponse>(jobs, pageNumber, pageSize, batchResponse.Total, ((batchResponse.Total - 1) / pageSize) + 1);
            }
        }
    }
}
