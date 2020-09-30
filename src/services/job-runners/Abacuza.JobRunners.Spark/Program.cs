using System;
using Microsoft.Spark.Sql;

namespace Abacuza.JobRunners.Spark
{
    class Program
    {
        static void Main(string[] args)
        {
            var spark = SparkSession.Builder().GetOrCreate();
            var df = spark.Read().Json("s3a://data/input/sample.json");
            df.Show();
        }
    }
}
