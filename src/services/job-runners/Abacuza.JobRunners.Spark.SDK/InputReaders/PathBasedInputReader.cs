using Abacuza.Endpoints.Input;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK.InputReaders
{
    public class PathBasedInputReader : InputReader<PathBasedInputEndpoint>
    {
        protected override DataFrame ReadFromInternal(SparkSession sparkSession, PathBasedInputEndpoint inputEndpoint, ProjectContext projectContext)
        {
            IEnumerable<string> formats;
            if (!string.IsNullOrEmpty(inputEndpoint.Formats))
            {
                formats = inputEndpoint.Formats.Split(',');
            }
            else
            {
                formats = new[] { "csv", "json" };
            }

            var paths = inputEndpoint.Paths.Split(Environment.NewLine);
            var dataFrameReader = sparkSession.Read();
            foreach (var format in formats)
            {
                dataFrameReader = dataFrameReader.Format(format);
            }

            return dataFrameReader.Load(paths);
        }
    }
}
