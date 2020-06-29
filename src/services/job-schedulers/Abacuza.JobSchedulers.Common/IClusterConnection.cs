using Abacuza.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobSchedulers.Common
{
    /// <summary>
    /// Represents that the implemented classes are cluster connections.
    /// </summary>
    /// <seealso cref="Abacuza.Common.IEntity" />
    public interface IClusterConnection : IEntity
    {
        /// <summary>
        /// Gets or sets the name of the connection.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the cluster to which the connection belongs.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        string Type { get; set; }
    }
}
