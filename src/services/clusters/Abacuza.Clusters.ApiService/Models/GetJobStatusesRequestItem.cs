using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Clusters.ApiService.Models
{
    /// <summary>
    /// Represents the item in a GetJobStatus request.
    /// </summary>
    public class GetJobStatusesRequestItem
    {
        /// <summary>
        /// The identifier of the cluster connection where
        /// the job was running.
        /// </summary>
        [Required]
        public Guid ConnectionId { get; set; }

        /// <summary>
        /// The identifiers of the jobs on the cluster where
        /// they were running.
        /// </summary>
        [Required]
        public string[] LocalJobIdentifiers { get; set; } = Array.Empty<string>();
    }
}
