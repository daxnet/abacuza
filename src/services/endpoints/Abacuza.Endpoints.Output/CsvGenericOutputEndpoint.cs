using Abacuza.Common.UIComponents;
using System;

namespace Abacuza.Endpoints.Output
{
    /// <summary>
    /// Represents the output endpoint for CSV/TSV files.
    /// </summary>
    /// <remarks>For a complete list of the options, please refer to https://spark.apache.org/docs/latest/api/java/org/apache/spark/sql/DataFrameWriter.html.
    /// </remarks>
    [Endpoint("endpoints.output.csv.generic", "CSV/TSV Files", EndpointType.Output)]
    public class CsvGenericOutputEndpoint : CsvProjectOutputEndpoint, IOutputEndpoint
    {
        [TextBox("txtCsvOutputPath", 
            "Path", 
            Required = true, 
            Ordinal = 500, 
            Tooltip = "The output path.")]
        public string? Path { get; set; }
    }
}
