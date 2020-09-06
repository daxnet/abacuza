using System.Collections.Generic;
using System;
using Abacuza.Common;
using Abacuza.Common.DataAccess;

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

        public DateTime? Created { get; set; }


        // public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        public JobState State { get; set; }
    }
}