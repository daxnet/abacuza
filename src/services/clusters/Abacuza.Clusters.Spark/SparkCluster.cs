using Abacuza.Clusters.Common;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Abacuza.Clusters.Spark
{
    [Cluster("B22A9E13-CE6E-4927-868D-DBD215B456E4", "spark", "Apache Spark")]
    public sealed class SparkCluster : Cluster
    {
        public override async Task<ClusterState> GetStateAsync(IClusterConnection connection, CancellationToken cancellationToken = default)
        {
            var connectionInformation = connection.As<SparkClusterConnection>();
            using var httpClient = new HttpClient();
            await httpClient.GetAsync("");
            return ClusterState.Online;
        }
    }
}
