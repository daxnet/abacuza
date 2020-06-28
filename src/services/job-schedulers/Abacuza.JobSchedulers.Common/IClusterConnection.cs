using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.JobSchedulers.Common
{
    public interface IClusterConnection
    {
        string Name { get; set; }
    }
}
