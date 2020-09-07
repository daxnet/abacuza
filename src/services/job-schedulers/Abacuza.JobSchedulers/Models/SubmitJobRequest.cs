using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Models
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
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the job parameters.
        /// </summary>
        /// <value>
        /// The job parameters.
        /// </value>
        public Dictionary<string, object> Properties { get; set; }


    }
}
