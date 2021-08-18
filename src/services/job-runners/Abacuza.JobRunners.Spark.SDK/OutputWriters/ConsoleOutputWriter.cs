using Abacuza.Endpoints.Output;
using Abacuza.JobRunners.Spark.SDK.OutputWriters;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK.OutputWriters
{
    /// <summary>
    /// Represents the output writer that writes the output to the console.
    /// </summary>
    internal sealed class ConsoleOutputWriter : OutputWriter<ConsoleOutputEndpoint>
    {
        protected override void WriteToInternal(DataFrame dataFrame, ConsoleOutputEndpoint outputEndpoint, ProjectContext projectContext)
        {
            dataFrame.Show();
        }
    }
}
