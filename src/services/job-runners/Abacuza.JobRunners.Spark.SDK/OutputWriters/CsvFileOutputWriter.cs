using Abacuza.Endpoints;
using Abacuza.Endpoints.Output;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark.SDK.OutputWriters
{
    public class CsvFileOutputWriter : OutputWriter<CsvOutputEndpoint>
    {
        protected override void WriteToInternal(DataFrame dataFrame, CsvOutputEndpoint outputEndpoint)
        {
            var delimiter = outputEndpoint.SeparatorCharacter switch
            {
                "Tab" => "\t",
                "Comma" => ",",
                "Pipe" => "|",
                "Space" => " ",
                _ => ","
            };

            var options = new Dictionary<string, string>()
            {
                { "sep", delimiter },
                { "header", outputEndpoint.HasHeaderRecord.ToString().ToLower() }
            };

            dataFrame.Write().Options(options).Csv(outputEndpoint.Path);
        }
    }
}
