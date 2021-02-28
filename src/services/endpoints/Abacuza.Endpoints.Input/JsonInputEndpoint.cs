using Abacuza.Common.Models;
using Abacuza.Common.UIComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Endpoints.Input
{
    [Endpoint("endpoints.input.json",
        "JSON Files",
        EndpointType.Input,
        Description = "The endpoint that reads data from JSON files.")]
    public sealed class JsonInputEndpoint : Endpoint, IInputEndpoint
    {
        [FilePicker("fpJsonFiles", "JSON Files", AllowedExtensions = ".json", AllowMultipleSelection = true, Tooltip = "Choose JSON files.")]
        public List<S3File>? Files { get; set; }
    }
}
