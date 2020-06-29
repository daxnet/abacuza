using System;

namespace Abacuza.JobSchedulers.Common
{
    /// <summary>
    /// Represents that the decorated classes are Abacuza clusters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class AbacuzaClusterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <c>AbacuzaClusterAttribute</c> class.
        /// </summary>
        /// <param name="id">The id of the cluster.</param>
        /// <param name="name">The name of the cluster.</param>
        /// <param name="connectionType">The type of the connection.</param>
        public AbacuzaClusterAttribute(string id, string name, Type connectionType)
        {
            this.Id = Guid.Parse(id);
            this.Name = name;
            this.ConnectionType = connectionType;
        }

        /// <summary>
        /// Gets the id of the cluster.
        /// </summary>
        /// <value></value>
        public Guid Id { get; }

        /// <summary>
        /// Gets the name of the cluster.
        /// </summary>
        /// <value></value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the description of the cluster.
        /// </summary>
        /// <value></value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the connection.
        /// </summary>
        /// <value>
        /// The type of the connection.
        /// </value>
        public Type ConnectionType { get; set; }
    }
}