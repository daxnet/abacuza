using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.DataAccess
{
    /// <summary>
    /// Represents that the decorated class is a storage model object.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class StorageModelAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageModelAttribute"/> class.
        /// </summary>
        public StorageModelAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageModelAttribute"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table that the decorated class will be mapped to.</param>
        public StorageModelAttribute(string tableName)
        {
            TableName = tableName;
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get; }
    }
}
