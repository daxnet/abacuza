using Abacuza.Clusters.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Clusters.ApiService.Models
{
    public sealed class GetJobStatusesResponseItem
    {
        /// <summary>
        /// Gets or sets the connection ID.
        /// </summary>
        public Guid ConnectionId { get; set; }

        /// <summary>
        /// Gets or sets the local ID of the job.
        /// </summary>
        public string LocalJobId { get; set; }

        /// <summary>
        /// Gets or sets the job state.
        /// </summary>
        public ClusterJobState State { get; set; }

        /// <summary>
        /// Gets or sets the logs emitted by the job.
        /// </summary>
        public List<string> Logs { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the <see cref="bool"/> value which indicates
        /// whether the retrieving of the status was succeeded. Note that
        /// this doesn't reflect the job status itself.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Gets or sets the error message whether indicates the error
        /// when retrieving the job status.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
