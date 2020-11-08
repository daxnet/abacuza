using System;
using System.Linq;
using Microsoft.Spark.Sql;

namespace Abacuza.JobRunners.Spark
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args?.Length > 0)
            {
                Console.WriteLine(" ******************************************** ");
                foreach (var arg in args)
                {
                    Console.WriteLine($"    {arg}");
                }
                Console.WriteLine(" ******************************************** ");
            }

            var spark = SparkSession.Builder().GetOrCreate();
            var df = spark.Read().Json("s3a://data/input/sample.json");
            df.Show();
        }
    }
}
