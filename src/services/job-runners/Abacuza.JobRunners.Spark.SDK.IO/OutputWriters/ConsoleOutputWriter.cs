using Abacuza.Endpoints.Output;
using Abacuza.JobRunners.Spark.SDK.OutputWriters;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK.IO.OutputWriters
{
    public sealed class ConsoleOutputWriter : OutputWriter<ConsoleOutputEndpoint>
    {
        protected override void WriteToInternal(DataFrame dataFrame, ConsoleOutputEndpoint outputEndpoint, ProjectContext projectContext)
        {
            dataFrame.Show();
        }
    }
}
