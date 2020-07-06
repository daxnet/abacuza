using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Models
{
    public sealed class SubmitJobRequest
    {
        [JsonProperty("connection")]
        public string ClusterConnectionName { get; set; }

        [JsonProperty("parameters")]
        public Dictionary<string, object> JobParameters { get; set; }
    }
}
