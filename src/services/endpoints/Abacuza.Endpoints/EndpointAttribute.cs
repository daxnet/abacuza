using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Endpoints
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class EndpointAttribute : Attribute
    {
        public EndpointAttribute(string name, string displayName, EndpointType type)
        {
            Name = name;
            DisplayName = displayName;
            Type = type;
        }

        public string Name { get; }

        public string DisplayName { get; }

        public EndpointType Type { get; }

        public string Description { get; set; }
    }
}
