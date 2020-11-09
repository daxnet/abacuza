using Abacuza.Endpoints;
using Abacuza.JobRunners.Spark.SDK.InputReaders;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abacuza.JobRunners.Spark.SDK.InputReaders
{
    public abstract class InputReader<TEndpoint> : IInputReader
        where TEndpoint : IInputEndpoint
    {
        protected abstract DataFrame ReadFromInternal(SparkSession sparkSession, TEndpoint inputEndpoint);

        public DataFrame ReadFrom(SparkSession sparkSession, IInputEndpoint inputEndpoint)
        {
            if (sparkSession == null)
            {
                throw new ArgumentNullException(nameof(sparkSession));
            }

            if (inputEndpoint == null)
            {
                throw new ArgumentNullException(nameof(inputEndpoint));
            }

            if (inputEndpoint is TEndpoint endPoint)
            {
                return ReadFromInternal(sparkSession, endPoint);
            }

            throw new InputReaderException($"Input endpoint {inputEndpoint.GetType().FullName} can't be converted to type {typeof(TEndpoint).FullName}");
        }
    }
}
