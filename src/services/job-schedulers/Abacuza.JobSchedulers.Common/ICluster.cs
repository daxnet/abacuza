using Abacuza.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Common
{
    public interface ICluster
    {
        ClusterState State { get; set; }

        Guid Id { get; }

        string Name { get; }

        string Description { get; }

        Type ConnectionType { get; }

        IClusterConnection CreateConnection(string name, string jsonSettings);

        bool ValidateJobParameters(IEnumerable<KeyValuePair<string, object>> jobParameters);

        Task<Job> SubmitJobAsync(IClusterConnection connection, IEnumerable<KeyValuePair<string, object>> jobParameters);
    }
}
