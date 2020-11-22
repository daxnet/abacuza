using Abacuza.Common.Models;
using Abacuza.Common.UIComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Endpoints.Input
{
    [Endpoint("endpoints.input.csv", "CSV/TSV Files", EndpointType.Input)]
    public sealed class CsvInputEndpoint : Endpoint, IInputEndpoint
    {
        [FilePicker("fpFiles", "Files", AllowedExtensions = ".csv,.tsv", AllowMultipleSelection = true, Ordinal = 5)]
        public List<S3File> Files { get; set; }

        [DropDownBox("dropdownSeparator", "Separator", "Comma,Tab,Pipe,Space", Ordinal = 15)]
        public string SeparatorCharacter { get; set; }

        [Checkbox("chkHasHeaderRecord", "Has Header Record", Ordinal = 10, Tooltip = "Indicates whether the first line of the file is the file header.")]
        public bool HasHeaderRecord { get; set; }

        [Checkbox("chkInferSchema", "Infer Schema", Ordinal = 9, Tooltip = "Indicates whether the schema should be inferred.")]
        public bool InferSchema { get; set; }
    }
}
