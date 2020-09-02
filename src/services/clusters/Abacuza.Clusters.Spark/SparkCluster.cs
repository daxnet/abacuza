using Abacuza.Clusters.Common;
using System;
using System.Net.Http;
using System.Security.Cryptography;
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
