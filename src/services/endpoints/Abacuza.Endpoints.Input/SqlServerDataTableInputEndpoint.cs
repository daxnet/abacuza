using Abacuza.Common.UIComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Endpoints.Input
{
    /// <summary>
    /// Represents the input endpoint that reads data from Microsoft SQL Server database.
    /// </summary>
    [Endpoint("endpoints.input.sqlserver", "Microsoft SQL Server Data Table", EndpointType.Input)]
    public sealed class SqlServerDataTableInputEndpoint : Endpoint, IInputEndpoint
    {
        [TextBox("txtConnectionURL", "Connection URL", Required = true, Ordinal = 100)]
        public string? ConnectionUrl { get; set; }

        [TextBox("txtDataTable", "Data table", Required = true, Ordinal = 90)]
        public string? DataTable { get; set; }

        [TextBox("txtUserName", "User name", Required = true, Ordinal = 80, Tooltip = "User name of the database.")]
        public string? UserName { get; set; }

        [TextBox("txtPassword", "Password", Required = true, Ordinal = 70, Tooltip = "Password for the user.")]
        public string? Password { get; set; }
    }
}
