using Abacuza.Common.UIComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Endpoints.Input
{
    [Endpoint("endpoints.input.sqlserver", "Microsoft SQL Server Data Table", EndpointType.Input)]
    public sealed class SqlServerInputEndpoint : Endpoint
    {
        [TextBox("Connection URL", Required = true)]
        public string ConnectionUrl { get; set; }

        [TextBox("Data table", Required = true)]
        public string DataTable { get; set; }

        [TextBox("User name", Required = true)]
        public string UserName { get; set; }

        [TextBox("Password", Required = true)]
        public string Password { get; set; }
    }
}
