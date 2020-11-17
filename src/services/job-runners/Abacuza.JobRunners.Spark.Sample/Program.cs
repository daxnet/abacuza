using System;
using System.Linq;
using Microsoft.Spark.Sql;

namespace Abacuza.JobRunners.Spark
{
    class Program
    {
        static void Main(string[] args)
        {
            new SampleRunner(args).Run();
        }
    }
}
