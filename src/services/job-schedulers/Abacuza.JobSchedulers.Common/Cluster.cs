using System.Threading;
using System.Threading.Tasks;
using System;
using System.Reflection;
using Abacuza.Common;
using Abacuza.JobSchedulers.Common.Models;

namespace Abacuza.JobSchedulers.Common
{
    public abstract class Cluster<TConnection> : ICluster
        where TConnection : ClusterConnection
    {
        /// <summary>
        /// Gets or sets the state of the cluster.
        /// </summary>
        /// <value></value>
        public ClusterState State { get; set; }


        public string Name => this.GetAbacuzaClusterAttribute()?.Name;
        
        public Guid Id => this.GetAbacuzaClusterAttribute()?.Id ?? Guid.Empty;

        public string Description => this.GetAbacuzaClusterAttribute()?.Description;

        protected abstract Task<PagedResult<JobResponse>> GetJobsAsync(TConnection connection, int pageNumber = 0, int pageSize = 10, CancellationToken cancellationToken = default);

        public Task<PagedResult<JobResponse>> GetJobsAsync(IClusterConnection connection, int pageNumber = 0, int pageSize = 10, CancellationToken cancellationToken = default)
            => connection is TConnection conn ? GetJobsAsync(conn, pageNumber, pageSize, cancellationToken) : throw new NotSupportedException();

        private AbacuzaClusterAttribute GetAbacuzaClusterAttribute() =>
            this.GetType().IsDefined(typeof(AbacuzaClusterAttribute), true) ?
            this.GetType().GetCustomAttribute<AbacuzaClusterAttribute>() : null;

        public override string ToString() => Name;

    }
}