using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Clusters.Common
{
    /// <summary>
    /// Provides the extension methods for the clusters related instances.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts the specified cluster connection into a generic cluster connection type.
        /// </summary>
        /// <typeparam name="T">The type of the cluster connection.</typeparam>
        /// <param name="clusterConnection">The cluster connection.</param>
        /// <returns>The generic typed cluster connection instance.</returns>
        public static T? As<T>(this IClusterConnection clusterConnection)
            where T : class, IClusterConnection => clusterConnection is T result ? result : null;
    }
}
