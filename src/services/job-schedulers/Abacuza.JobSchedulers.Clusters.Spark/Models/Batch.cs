using System.Collections.Generic;
using Newtonsoft.Json;

namespace Abacuza.JobSchedulers.Clusters.Spark.Models
{
    internal sealed class Batch
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("appId")]
        public string ApplicationId { get; set; }

        [JsonProperty("appInfo")]
        public Dictionary<string, string> ApplicationInfo { get; set; }

        [JsonProperty("log")]
        public List<string> Logs { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}