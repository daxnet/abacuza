using Abacuza.Endpoints.Output;
using Abacuza.JobRunners.Spark.SDK.OutputWriters;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK.OutputWriters
{
    internal sealed class CsvProjectOutputEndpointWriter : OutputWriter<CsvProjectOutputEndpoint>
    {
        protected override void WriteToInternal(DataFrame dataFrame, CsvProjectOutputEndpoint outputEndpoint, ProjectContext projectContext)
        {
            var outputKey = $"s3a://data/projects/{projectContext.ProjectId}/output/revisions/{projectContext.RevisionId}";
            var options = ReadOptionsFromOutputEndpoint(outputEndpoint);
            dataFrame.Write().Options(options).Csv(outputKey);
        }

        internal static Dictionary<string, string> ReadOptionsFromOutputEndpoint(CsvProjectOutputEndpoint endpoint)
        {
            var options = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(endpoint.DateFormat)) options.Add("dateFormat", endpoint.DateFormat);
            if (!string.IsNullOrEmpty(endpoint.Encoding)) options.Add("encoding", endpoint.Encoding);
            if (!string.IsNullOrEmpty(endpoint.Escape)) options.Add("escape", endpoint.Escape);
            if (endpoint.EscapeQuotes) options.Add("escapeQuotes", "true");
            if (endpoint.Header) options.Add("header", "true");
            if (endpoint.IgnoreLeadingWhiteSpace) options.Add("ignoreLeadingWhiteSpace", "true");
            if (endpoint.IgnoreTrailingWhiteSpace) options.Add("ignoreTrailingWhiteSpace", "true");
            if (!string.IsNullOrEmpty(endpoint.LineSep)) options.Add("lineSep", endpoint.LineSep);
            if (!string.IsNullOrEmpty(endpoint.NullValue)) options.Add("nullValue", endpoint.NullValue);
            if (!string.IsNullOrEmpty(endpoint.Quote)) options.Add("quote", endpoint.Quote);
            if (endpoint.QuoteAll) options.Add("quoteAll", "true");
            if (!string.IsNullOrEmpty(endpoint.TimestampFormat)) options.Add("timestampFormat", endpoint.TimestampFormat);
            var delimiter = endpoint.Separator switch
            {
                "Tab" => "\t",
                "Comma" => ",",
                "Pipe" => "|",
                "Space" => " ",
                _ => ","
            };
            options.Add("sep", delimiter);
            return options;
        }
    }
}
