using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Clusters.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ClusterAttribute : Attribute
    {
        public ClusterAttribute(string id, string type, string name)
        {
            this.Id = Guid.Parse(id);
            this.Type = type;
            this.Name = name;
        }

        public Guid Id { get; }

        public string Type { get; }

        public string Name { get; }

        public string Description { get; set; }
    }
}
