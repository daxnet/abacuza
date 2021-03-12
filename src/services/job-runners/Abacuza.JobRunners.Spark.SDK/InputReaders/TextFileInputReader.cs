using Abacuza.Endpoints.Input;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK.InputReaders
{
    internal sealed class TextFileInputReader : InputReader<TextInputEndpoint>
    {
        protected override DataFrame ReadFromInternal(SparkSession sparkSession, TextInputEndpoint inputEndpoint, ProjectContext projectContext)
        {
            var textFiles = inputEndpoint
                .Files
                .Select(f => $"s3a://{f.Bucket}/{f.Key}/{f.File}")
                .ToArray();

            if (textFiles?.Length == 0)
            {
                throw new SparkRunnerException("No files could be read by the TextFileInputReader.");
            }

            return sparkSession.Read().Text(textFiles);
        }
    }
}
