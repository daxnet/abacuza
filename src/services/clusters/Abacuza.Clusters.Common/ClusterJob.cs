// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020-2021 by daxnet. All rights reserved.
// Apache License Version 2.0
// ==============================================================

using System;
using System.Collections.Generic;

namespace Abacuza.Clusters.Common
{
    /// <summary>
    /// Represents a data processing and transforming job that is running
    /// on a certain data processing cluster.
    /// </summary>
    public class ClusterJob
    {
        #region Public Constructors

        public ClusterJob() => LocalJobId = string.Empty;

        public ClusterJob(Guid connectionId, string localJobId)
        {
            ConnectionId = connectionId;
            LocalJobId = localJobId;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the id of the cluster connection.
        /// </summary>
        /// <value>
        /// The cluster identifier.
        /// </value>
        public Guid ConnectionId { get; set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id => $"{ConnectionId}.{LocalJobId}";

        /// <summary>
        /// Gets or sets the id of the job that is assigned by the current cluster
        /// when the job was created.
        /// </summary>
        /// <value>
        /// The local job identifier.
        /// </value>
        public string LocalJobId { get; set; }

        public List<string> Logs { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the name of the current job.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string? Name { get; set; }

        public ClusterJobState State { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override bool Equals(object obj)
        {
            return obj is ClusterJob job &&
                   ConnectionId.Equals(job.ConnectionId) &&
                   LocalJobId == job.LocalJobId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConnectionId, LocalJobId);
        }

        public override string ToString() => Id;

        #endregion Public Methods
    }
}