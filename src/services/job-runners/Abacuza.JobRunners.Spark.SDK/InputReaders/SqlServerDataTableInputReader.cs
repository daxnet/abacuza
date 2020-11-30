using Abacuza.Endpoints.Input;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK.InputReaders
{
    /// <summary>
    /// Represents the input reader that reads data from Microsoft SQL Server.
    /// </summary>
    /// <remarks>
    /// For more information, please refer to: https://docs.microsoft.com/en-us/dotnet/spark/how-to-guides/connect-to-sql-server
    /// </remarks>
    public sealed class SqlServerDataTableInputReader : InputReader<SqlServerDataTableInputEndpoint>
    {
        protected override DataFrame ReadFromInternal(SparkSession sparkSession, SqlServerDataTableInputEndpoint inputEndpoint)
        {
            return sparkSession.Read()
                .Format("jdbc")
                .Option("driver", "com.microsoft.sqlserver.jdbc.SQLServerDriver")
                .Option("url", inputEndpoint.ConnectionUrl)
                .Option("dbtable", inputEndpoint.DataTable)
                .Option("user", inputEndpoint.UserName)
                .Option("password", inputEndpoint.Password)
                .Load();
        }
    }
}
