using System.Collections.Generic;
using System;
using Abacuza.Common;
using Abacuza.Common.DataAccess;
using Newtonsoft.Json;

namespace Abacuza.JobSchedulers.Models
{
    [StorageModel("jobs")]
    public sealed class JobEntity : IEntity
    {
        public JobEntity()
        {

        }

        public Guid Id { get; set; }

        public Guid? ConnectionId { get; set; }

        /// <summary>
        /// Gets or sets the job id that is specific to the cluster on which
        /// the job was run.
        /// </summary>
        public string LocalJobId { get; set; }

        public string Name { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime? FailedDate { get; set; }

        public DateTime? CancelledDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the job submission, this is usually
        /// the name of the job executor that submits the job.
        /// </summary>
        public string SubmissionName { get; set; }

        public JobState State { get; set; }

        public JobTraceability? Traceability { get; set; }


        public int? TracingFailures { get; set; }

        public List<string> Logs { get; set; } = new List<string>();
    }
}