using System;

namespace Abacuza.JobSchedulers.Common.Models
{
    public abstract class ClusterConnection
    {
        /// <summary>
        /// Gets or sets the name of the connection.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
    }
}
