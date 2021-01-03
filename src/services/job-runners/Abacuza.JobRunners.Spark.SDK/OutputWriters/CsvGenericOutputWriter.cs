using Abacuza.Endpoints;
using Abacuza.Endpoints.Output;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK.OutputWriters
{
    public class CsvGenericOutputWriter : OutputWriter<CsvGenericOutputEndpoint>
    {
        protected override void WriteToInternal(DataFrame dataFrame, CsvGenericOutputEndpoint outputEndpoint, ProjectContext projectContext)
        {
            var options = CsvProjectOutputEndpointWriter.ReadOptionsFromOutputEndpoint(outputEndpoint);
            dataFrame.Write().Options(options).Csv(outputEndpoint.Path);
        }
    }
}
