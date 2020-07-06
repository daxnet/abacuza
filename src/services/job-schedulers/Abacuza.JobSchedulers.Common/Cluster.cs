using System.Threading;
using System.Threading.Tasks;
using System;
using System.Reflection;
using Abacuza.Common;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

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

        public Type ConnectionType => this.GetAbacuzaClusterAttribute()?.ConnectionType;

        public abstract bool ValidateJobParameters(IEnumerable<KeyValuePair<string, object>> jobParameters);

        private AbacuzaClusterAttribute GetAbacuzaClusterAttribute() =>
            this.GetType().IsDefined(typeof(AbacuzaClusterAttribute), true) ?
            this.GetType().GetCustomAttribute<AbacuzaClusterAttribute>() : null;

        public override string ToString() => Name;

        public IClusterConnection CreateConnection(string name, string jsonSettings)
        {
            var connection = (ClusterConnection)JsonConvert.DeserializeObject(jsonSettings, ConnectionType);
            connection.Name = name;
            connection.Type = this.Name;
            return connection;
        }
    }
}