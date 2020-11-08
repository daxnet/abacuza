using Abacuza.Endpoints;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abacuza.JobRunners.Spark.SDK.InputReaders
{
    internal interface IInputReader
    {
        DataFrame ReadFrom(SparkSession sparkSession, IInputEndpoint inputEndpoint);
    }
}
