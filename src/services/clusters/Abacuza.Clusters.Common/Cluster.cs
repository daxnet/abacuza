// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020 by daxnet. All rights reserved.
// Licensed under LGPL-v3
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

        public Type ConnectionType => this.ClusterAttribute?.ConnectionType;
        public string Description => this.ClusterAttribute?.Description;
        public Guid Id => this.ClusterAttribute?.Id ?? Guid.Empty;

        public string Name => this.ClusterAttribute?.Name;

        public string Type => this.ClusterAttribute?.Type;
        #endregion Public Properties

        #region Private Properties

        private ClusterAttribute ClusterAttribute => this.GetType().IsDefined(typeof(ClusterAttribute), true) ?
                this.GetType().GetCustomAttribute<ClusterAttribute>() : null;

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
            int hashCode = -678952093;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
            return hashCode;
        }

        #endregion Private Properties

        #region Public Methods

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public abstract Task<ClusterState> GetStateAsync(IClusterConnection connection, CancellationToken cancellationToken = default);

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => this.Name;

        protected virtual void Dispose(bool disposing) { }

        public abstract Task<ClusterJob> SubmitJobAsync(IClusterConnection connection, IEnumerable<KeyValuePair<string, object>> properties, CancellationToken cancellationToken = default);

        public abstract Task<ClusterJob> GetJobAsync(IClusterConnection connection, string localJobId, CancellationToken cancellation = default);



        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Cluster()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }
        #endregion Public Methods
    }
}