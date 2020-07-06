using System.Collections.Generic;
namespace Abacuza.JobSchedulers.Common
{
    public class Job
    {
        public string JobId {get;set;}

        public string State {get;set;}

        public IEnumerable<string> Logs{get;set;}
    }
}