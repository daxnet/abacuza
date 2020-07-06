using System.Collections.Generic;
using System;
using Abacuza.Common;

namespace Abacuza.JobSchedulers.Models
{
    public sealed class JobStorageModel : IEntity
    {
        public JobStorageModel()
        {

        }

        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the job id that is specific to the cluster on which
        /// the job was run.
        /// </summary>
        public string ClusterJobId { get; set; }

        /// <summary>
        /// Gets or sets the id of the cluster on which the job was executed.
        /// </summary>
        /// <value></value>
        public Guid ClusterId { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Queued { get; set; }

        public DateTime? Started { get; set; }

        public DateTime? Completed { get; set; }

        public DateTime? Cancelled { get; set; }

        public DateTime? Failed { get; set; }

        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        public JobState State { get; set; }
    }
}