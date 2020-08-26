using Abacuza.Clusters.Common;
using Abacuza.Common;
using Abacuza.Common.DataAccess;
using MongoDB.Driver.Core.Clusters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Clusters.ApiService.Models
{
    /// <summary>
    /// Represents the data storage model for cluster connections.
    /// </summary>
    [StorageModel("ClusterConnections")]
    public class ClusterConnectionEntity : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <c>ClusterConnectionEntity</c> class.
        /// </summary>
        public ClusterConnectionEntity()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <c>ClusterConnectionEntity</c> class.
        /// </summary>
        /// <param name="clusterConnection">The cluster connection object used to initialize the current instance.</param>
        public ClusterConnectionEntity(IClusterConnection clusterConnection)
        {
            if (clusterConnection == null)
            {
                throw new ArgumentNullException(nameof(clusterConnection));
            }

            Name = clusterConnection.Name;
            Description = clusterConnection.Description;
            ClusterType = clusterConnection.ClusterType;
            Settings = clusterConnection.SerializeConfiguration();
        }

        /// <summary>
        /// Gets or sets the type of the cluster that the current cluster connection entity is used for.
        /// </summary>
        [JsonProperty("clusterType")]
        [Required]
        public string ClusterType { get; set; }

        /// <summary>
        /// Gets or sets the description of the cluster connection entity.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the id of the cluster connection entity.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the cluster connection entity.
        /// </summary>
        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a JSON serialized string of the cluster connection.
        /// </summary>
        [JsonProperty("settings")]
        public string Settings { get; set; }

        public TClusterConnection Create<TClusterConnection>()
            where TClusterConnection : IClusterConnection, new()
        {
            var result = new TClusterConnection
            {
                ClusterType = ClusterType,
                Description = Description,
                Name = Name
            };

            result.DeserializeConfiguration(Settings);
            return result;
        }

        public IClusterConnection Create(Type clusterConnectionType)
        {
            var result = (IClusterConnection)Activator.CreateInstance(clusterConnectionType);
            result.ClusterType = ClusterType;
            result.Description = Description;
            result.Name = Name;
            result.DeserializeConfiguration(Settings);

            return result;
        }

        public override string ToString() => Name;
    }
}
