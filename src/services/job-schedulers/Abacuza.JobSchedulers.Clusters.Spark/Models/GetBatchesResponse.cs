using System.Collections.Generic;
using Newtonsoft.Json;

namespace Abacuza.JobSchedulers.Clusters.Spark.Models
{
    internal sealed class GetBatchesResponse
    {
        [JsonProperty("from")]
        public int Index { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("sessions")]
        public List<Batch> Batches { get; set; }
    }
}