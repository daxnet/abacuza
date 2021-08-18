using System.Collections.Generic;

namespace Abacuza.Jobs.ApiService.Models
{
    /// <summary>
    /// Represents the request schema of the job submission.
    /// </summary>
    public sealed class SubmitJobRequest
    {
        /// <summary>
        /// Gets or sets the type of the cluster to which the job is submitted.
        /// </summary>
        /// <value>
        /// The cluster type.
        /// </value>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the job parameters.
        /// </summary>
        /// <value>
        /// The job parameters.
        /// </value>
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();


    }
}
