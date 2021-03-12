using Abacuza.Common.Models;
using Abacuza.Common.UIComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Endpoints.Input
{
    [Endpoint("endpoints.input.text",
        "Text Files", 
        EndpointType.Input,
        Description = "The endpoint that reads data from text files.")]
    public sealed class TextInputEndpoint : Endpoint, IInputEndpoint
    {
        [FilePicker("fpTextFiles", "Text Files", AllowedExtensions = ".txt", AllowMultipleSelection = true, Tooltip = "Choose text files.")]
        public List<S3File>? Files { get; set; }
    }
}
