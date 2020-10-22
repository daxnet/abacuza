
using Abacuza.Common.UIComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Endpoints
{
    public abstract class Endpoint
    {
        [TextArea("Additional options")]
        public Dictionary<string, string> AdditionalOptions { get; set; }
            = new Dictionary<string, string>();
    }
}
