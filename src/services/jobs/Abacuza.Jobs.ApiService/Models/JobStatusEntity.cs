using System;
using System.Collections.Generic;

namespace Abacuza.Jobs.ApiService.Models
{
    public class JobStatusEntity
    {
        public Guid ConnectionId { get; set; }

        public string LocalJobId { get; set; } = string.Empty;

        public bool Succeeded { get; set; }

        public JobState State { get; set; }

        public List<string> Logs { get; set; } = new List<string>();
    }
}
