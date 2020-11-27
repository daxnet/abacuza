using Abacuza.Common.Models;
using Abacuza.Common.UIComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Endpoints.Input
{
    /// <summary>
    /// Represents the input endpoint for CSV/TSV files.
    /// </summary>
    /// <remarks>For possible options please refer to https://spark.apache.org/docs/latest/api/java/org/apache/spark/sql/DataFrameReader.html
    /// </remarks>
    [Endpoint("endpoints.input.csv", "CSV/TSV Files", EndpointType.Input)]
    public sealed class CsvInputEndpoint : Endpoint, IInputEndpoint
    {
        /// <summary>
        /// Gets or sets a list of files, in particular, the files with CSV or TSV extensions.
        /// </summary>
        [FilePicker("fpFiles", 
            "Files", 
            AllowedExtensions = ".csv,.tsv", 
            AllowMultipleSelection = true, 
            Ordinal = 200)]
        public List<S3File> Files { get; set; }

        /// <summary>
        /// Gets or sets the type of the separator of the row values.
        /// </summary>
        [DropDownBox("dropdownSeparator", 
            "Separator", 
            "Comma,Tab,Pipe,Space", 
            Tooltip = "Choose the separator character.", 
            Ordinal = 190,
            DefaultValue = "Comma")]
        public string SeparatorCharacter { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value which indicates whether the CSV/TSV contains header record.
        /// </summary>
        [Checkbox("chkHasHeaderRecord", 
            "Has Header Record", 
            Ordinal = 180, 
            Tooltip = "Whether the first line of the file is the file header.",
            DefaultValue = "true")]
        public bool HasHeaderRecord { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value which indicates whether the schema should be inferred from the raw data.
        /// </summary>
        [Checkbox("chkInferSchema", 
            "Infer Schema", 
            Ordinal = 170, 
            Tooltip = "Whether the schema should be inferred.",
            DefaultValue = "true")]
        public bool InferSchema { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value which indicates whether a record in the file could span multiple lines.
        /// </summary>
        [Checkbox("chkMultiline",
            "Multi-line support",
            Ordinal = 160,
            Tooltip = "Whether a record can span multiple lines.",
            DefaultValue = "false")]
        public bool Multiline { get; set; }
    }
}
