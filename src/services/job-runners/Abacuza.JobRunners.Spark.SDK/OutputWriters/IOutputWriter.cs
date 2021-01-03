using Abacuza.Endpoints;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK.OutputWriters
{
    internal interface IOutputWriter
    {
        void WriteTo(DataFrame dataFrame, IOutputEndpoint outputEndpoint, ProjectContext projectContext);
    }
}
