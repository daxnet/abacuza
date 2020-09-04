using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Services
{
    public class ClusterApiService
    {
        private readonly HttpClient _httpClient;

        public ClusterApiService(HttpClient httpClient)
            => _httpClient = httpClient;


    }
}
