using System;

namespace Abacuza.JobSchedulers.Common
{
    public abstract class ClusterConnection : IClusterConnection
    {
        /// <summary>
        /// Gets or sets the name of the connection.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
    }
}
