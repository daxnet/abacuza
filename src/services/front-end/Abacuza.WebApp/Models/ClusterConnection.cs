using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.WebApp.Models
{
    /// <summary>
    /// Represents the view model for cluster connections.
    /// </summary>
    public class ClusterConnection
    {
        public Guid? Id { get; set; }

        [Required]
        public string? ClusterType { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Settings { get; set; }
    }
}
