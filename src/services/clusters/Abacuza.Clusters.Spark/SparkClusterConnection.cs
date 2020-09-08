using Abacuza.Clusters.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public Guid Id { get; set; }

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
            var configurationJObject = JObject.Parse(serializedConfiguration);
            this.BaseUrl = configurationJObject["baseUrl"].Value<string>();
            var props = configurationJObject["properties"].ToObject<Dictionary<string, object>>();
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
