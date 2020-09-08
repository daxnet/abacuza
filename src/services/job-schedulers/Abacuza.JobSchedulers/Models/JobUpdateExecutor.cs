using Abacuza.Common.DataAccess;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Models
{
    [DisallowConcurrentExecution]
    public class JobUpdateExecutor : IJob
    {
        private readonly IDataAccessObject _dao;
        private int cnt = 0;

        public JobUpdateExecutor(IDataAccessObject dao)
        {
            _dao = dao;
        }

        public Task Execute(IJobExecutionContext context)
        {
            cnt++;
            return Task.CompletedTask;
        }
    }
}
