using System.Collections.Generic;
using System;

namespace Abacuza.JobSchedulers.Common.Models
{
    public sealed class Job
    {
        private JobState state;
        private readonly Dictionary<string, object> parameters = new Dictionary<string, object>();

        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the id of the cluster on which the job was executed.
        /// </summary>
        /// <value></value>
        public Guid ClusterId { get; set; }

        public DateTime? Created { get; private set; }

        public DateTime? Queued { get; private set; }

        public DateTime? Started { get; private set; }

        public DateTime? Completed { get; private set; }

        public DateTime? Cancelled { get; private set; }

        public DateTime? Failed { get; private set; }

        public IEnumerable<KeyValuePair<string, object>> Parameters => this.parameters;

        public void AddParameter(string name, object value) => this.parameters.Add(name, value);

        public JobState State
        {
            get => state;
            set
            {
                if (state != value)
                {
                    state = value;
                    switch (state)
                    {
                        case JobState.Created:
                            this.Created = DateTime.UtcNow;
                            break;
                        case JobState.Queued:
                            this.Queued = DateTime.UtcNow;
                            break;
                        case JobState.Started:
                            this.Started = DateTime.UtcNow;
                            break;
                        case JobState.Completed:
                            this.Completed = DateTime.UtcNow;
                            break;
                        case JobState.Cancelled:
                            this.Cancelled = DateTime.UtcNow;
                            break;
                        case JobState.Failed:
                            this.Failed = DateTime.UtcNow;
                            break;
                    }
                }

            }
        }
    }
}