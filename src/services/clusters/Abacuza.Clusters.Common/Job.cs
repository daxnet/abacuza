﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Clusters.Common
{
    /// <summary>
    /// Represents a data processing and transforming job that is running
    /// on a certain data processing cluster.
    /// </summary>
    public class Job
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the id of the cluster connection.
        /// </summary>
        /// <value>
        /// The cluster identifier.
        /// </value>
        public Guid ConnectionId { get; set; }

        public string Id => $"{ConnectionId}.{LocalJobId}";

        /// <summary>
        /// Gets or sets the id of the job that is assigned by the current cluster
        /// when the job was created.
        /// </summary>
        /// <value>
        /// The local job identifier.
        /// </value>
        public string LocalJobId { get; set; }

        /// <summary>
        /// Gets or sets the name of the current job.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Created { get; set; }


        #endregion Public Properties

        #region Public Methods

        public override bool Equals(object obj)
        {
            return obj is Job job &&
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