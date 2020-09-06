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

        public JobUpdateExecutor(IDataAccessObject dao)
        {
            _dao = dao;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}
