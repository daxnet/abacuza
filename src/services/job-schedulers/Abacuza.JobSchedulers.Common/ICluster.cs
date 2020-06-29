using Abacuza.Common;
using Abacuza.JobSchedulers.Common.Models;
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

        Task<PagedResult<JobResponse>> GetJobsAsync(IClusterConnection connection, int pageNumber = 0, int pageSize = 10, CancellationToken cancellationToken = default);
    }
}
