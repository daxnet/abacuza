using Abacuza.Common;
using Abacuza.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Projects.ApiService.Models
{
    [StorageModel("projects")]
    public class ProjectEntity : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid? JobRunnerId { get; set; }

        public string InputEndpointName { get; set; }

        public string InputEndpointSettings { get; set; }

        public override string ToString() => Name;
    }
}
