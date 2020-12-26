using Abacuza.JobRunners.Spark.SDK;
using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobRunners.Spark
{
    internal sealed class SampleRunner : SparkRunnerBase
    {
        public SampleRunner(string[] args) : base(args)
        {
        }

        protected override DataFrame RunInternal(SparkSession sparkSession, DataFrame dataFrame)
        {
            // dataFrame.Show();
            return dataFrame;
        }
    }
}
