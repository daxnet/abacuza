﻿
using System.ComponentModel;
using System;
using Abacuza.JobSchedulers.Common.Models;

namespace Abacuza.JobSchedulers.Clusters.Spark
{
    [Serializable]
    public class SparkClusterConnection : ClusterConnection
    {
        [DisplayName("Livy Base URL")]
        [Description("The base URL of Livy")]
        public string Uri { get; set; }
    }
}
