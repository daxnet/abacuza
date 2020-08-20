using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Clusters.Common
{
    /// <summary>
    /// Represents that the implemented classes are cluster connections that provide the connection
    /// information to a specific type of cluster.
    /// </summary>
    public interface IClusterConnection
    {
        /// <summary>
        /// Gets or sets the name of the connection.
        /// </summary>
        /// <value>
        /// The name of the connection.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the connection.
        /// </summary>
        /// <value>
        /// The description of the connection.
        /// </value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the cluster for which the current connection is used.
        /// </summary>
        /// <value>
        /// The type of the cluster.
        /// </value>
        string ClusterType { get; set; }
    }
}
