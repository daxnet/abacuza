using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Models
{
    public class JobStatusEntity
    {
        public Guid ConnectionId { get; set; }

        public string LocalJobId { get; set; }

        public bool Succeeded { get; set; }

        public JobState State { get; set; }

        public List<string> Logs { get; set; }
    }
}
