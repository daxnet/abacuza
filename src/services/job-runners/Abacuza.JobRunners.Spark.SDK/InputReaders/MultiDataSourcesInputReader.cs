using Abacuza.Endpoints.Input;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK.InputReaders
{
    public sealed class MultiDataSourcesInputReader : InputReader<MultiDataSourcesInputEndpoint>
    {
        protected override DataFrame ReadFromInternal(SparkSession sparkSession, MultiDataSourcesInputEndpoint inputEndpoint, ProjectContext projectContext)
        {
            throw new NotImplementedException();
        }
    }
}
