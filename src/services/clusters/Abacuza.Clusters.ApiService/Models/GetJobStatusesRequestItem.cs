using System;
using System.Collections.Generic;
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
        public Guid ConnectionId { get; set; }

        /// <summary>
        /// The identifiers of the jobs on the cluster where
        /// they were running.
        /// </summary>
        public string[] LocalJobIdentifiers { get; set; }
    }
}
