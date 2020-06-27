using System.Threading;
using System.Threading.Tasks;
using System;
using System.Reflection;
using Abacuza.Common;

namespace Abacuza.JobSchedulers.Common.Models
{
    public abstract class Cluster<TConnection>
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

        public abstract Task<PagedResult<JobResponse>> GetJobsAsync(TConnection connection, int pageNumber = 0, int pageSize = 10, CancellationToken cancellationToken = default);

        private AbacuzaClusterAttribute GetAbacuzaClusterAttribute() =>
            this.GetType().IsDefined(typeof(AbacuzaClusterAttribute), true) ?
            this.GetType().GetCustomAttribute<AbacuzaClusterAttribute>() : null;
        
    }
}