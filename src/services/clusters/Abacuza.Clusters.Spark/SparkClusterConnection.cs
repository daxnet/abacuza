using Abacuza.Clusters.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Clusters.Spark
{
    public sealed class SparkClusterConnection : IClusterConnection
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ClusterType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
