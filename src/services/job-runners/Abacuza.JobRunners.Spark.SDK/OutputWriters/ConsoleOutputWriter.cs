using Abacuza.Endpoints.Output;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK.OutputWriters
{
    public sealed class ConsoleOutputWriter : OutputWriter<ConsoleOutputEndpoint>
    {
        protected override void WriteToInternal(DataFrame dataFrame, ConsoleOutputEndpoint outputEndpoint)
        {
            dataFrame.Show();
        }
    }
}
