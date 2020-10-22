﻿using Abacuza.Common.Models;
using Abacuza.Common.UIComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Endpoints.Input
{
    [Endpoint("endpoints.input.csv", "CSV/TSV Files", EndpointType.Input)]
    public sealed class CsvInputEndpoint : Endpoint
    {
        [Files("Files", AllowedExtensions = ".csv,.tsv", AllowMultipleSelection = true)]
        public List<S3File> Files { get; set; }

        [Select("Separator", "comma,tab")]
        public string SeparatorCharacter { get; set; }

        [Checkbox("Has header")]
        public bool HasHeader { get; set; }
    }
}
