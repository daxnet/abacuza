using Abacuza.Common.UIComponents;
using System;

namespace Abacuza.Endpoints.Output
{
    [Endpoint("endpoints.output.csv", "CSV/TSV Files", EndpointType.Output)]
    public class CsvOutputEndpoint : Endpoint, IOutputEndpoint
    {
        [DropDownBox("dropdownSeperator",
            "Delimiter",
            "Comma,Tab,Pipe,Space",
            DefaultValue = "Comma",
            Tooltip = "Choose the delimiter used for separating the values.")]
        public string SeparatorCharacter { get; set; }

        [Checkbox("chkHasHeaderRecord",
            "Has header record",
            Ordinal = 190,
            Tooltip = "Whether the first line of the file is the file header.",
            DefaultValue = "true")]
        public bool HasHeaderRecord { get; set; }

        [TextBox("txtCsvOutputPath", 
            "Path", 
            Required = true, 
            Ordinal = 180, 
            Tooltip = "The output path.")]
        public string Path { get; set; }
    }
}
