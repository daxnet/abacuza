using Abacuza.Clusters.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abacuza.Clusters.Spark
{
    public sealed class SparkClusterConnection : IClusterConnection
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        public string Name { get; set; }

        public string Description { get; set; }

        public string ClusterType { get; set; }

        public override string ToString() => Name;

        public string SerializeConfiguration()
        {
            var serializingObject = new
            {
                baseUrl = BaseUrl,
                properties = _properties
            };

            return JsonConvert.SerializeObject(serializingObject);
        }

        public void DeserializeConfiguration(string serializedConfiguration)
        {
            dynamic deserializedObject = JsonConvert.DeserializeObject<dynamic>(serializedConfiguration);
            this.BaseUrl = (string)deserializedObject.baseUrl;
            var props = (Dictionary<string, object>)deserializedObject.properties;
            _properties.Clear();
            if (props != null)
            {
                foreach (var kvp in props)
                {
                    _properties.Add(kvp.Key, kvp.Value);
                }
            }
        }

        public string BaseUrl { get; set; }

        public IEnumerable<KeyValuePair<string, object>> Properties => _properties;
    }
}
