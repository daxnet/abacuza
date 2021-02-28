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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Abacuza.Clusters.Common
{
    public abstract class Cluster : ICluster
    {

        #region Public Properties

        public Type ConnectionType => this.ClusterAttribute?.ConnectionType ?? throw new ArgumentException($"{nameof(ConnectionType)} must be defined on the ClusterAttribute decoration.");

        public string? Description => this.ClusterAttribute?.Description;

        public Guid Id => this.ClusterAttribute?.Id ?? throw new ArgumentException($"{nameof(Id)} must be defined on the ClusterAttribute decoration.");

        public string Name => this.ClusterAttribute?.Name ?? throw new ArgumentException($"{nameof(Name)} must be defined on the ClusterAttribute decoration.");

        public string Type => this.ClusterAttribute?.Type ?? throw new ArgumentException($"{nameof(Type)} must be defined on the ClusterAttribute decoration.");

        #endregion Public Properties

        #region Private Properties

        private ClusterAttribute? ClusterAttribute => 
            this.GetType().IsDefined(typeof(ClusterAttribute), true) ?
            this.GetType().GetCustomAttribute<ClusterAttribute>() : null;

        #endregion Private Properties

        #region Public Methods

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Cluster cluster &&
                   Id.Equals(cluster.Id) &&
                   Name == cluster.Name &&
                   Type == cluster.Type;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Type);
        }
        public abstract Task<ClusterJob> GetJobAsync(IClusterConnection connection, string localJobId, CancellationToken cancellation = default);

        public abstract Task<ClusterState> GetStateAsync(IClusterConnection connection, CancellationToken cancellationToken = default);

        public abstract Task<ClusterJob> SubmitJobAsync(IClusterConnection connection, IEnumerable<KeyValuePair<string, object>> properties, CancellationToken cancellationToken = default);

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => this.Name;

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing) { }

        #endregion Protected Methods
    }
}