
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Clusters.ApiService.Models
{
    public class ExecuteJobRequest
    {
        public ExecuteJobRequest()
        {
            Properties = new Dictionary<string, object>();
        }

        [Required]
        public string ClusterType { get; set; }

        public Dictionary<string, object> Properties { get; set; }
    }
}
