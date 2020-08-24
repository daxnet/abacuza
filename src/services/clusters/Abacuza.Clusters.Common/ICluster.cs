﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Abacuza.Clusters.Common
{
    /// <summary>
    /// Represents that the implemented classes are data transforming clusters.
    /// </summary>
    public interface ICluster
    {
        #region Public Properties

        /// <summary>
        /// Gets the description of the current cluster.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        string Description { get; }

        /// <summary>
        /// Gets the identifier of the current cluster.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid Id { get; }

        /// <summary>
        /// Gets the name of the current cluster.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the type of the cluster. The cluster type is a human readable
        /// text which represents the cluster type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        string Type { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets the state of the cluster.
        /// </summary>
        /// <param name="connection">The cluster connection instance which provides the connection information to the cluster.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The cluster state.</returns>
        Task<ClusterState> GetStateAsync(IClusterConnection connection, CancellationToken cancellationToken = default);

        #endregion Public Methods
    }
}