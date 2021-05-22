using Abacuza.WebApp.Areas.Administration.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Abacuza.WebApp.Models
{
    public sealed class ApiService
    {

        #region Private Fields

        private readonly IHttpClientFactory _httpClientFactory;

        #endregion Private Fields

        #region Public Constructors

        public ApiService(IHttpClientFactory httpClientFactory)
            => (_httpClientFactory) = (httpClientFactory);

        #endregion Public Constructors

        #region Public Methods

        public async Task<Guid> CreateClusterConnectionAsync(CreateOrEditClusterConnectionModel model)
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync("cluster-service/cluster-connections", new
            {
                model.ClusterType,
                model.Name,
                model.Description,
                model.Settings
            });

            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.Created)
            {
                return new Guid(responseContent.Trim('\"'));
            }

            throw new ApiException(response.StatusCode, responseContent);
        }

        public async Task<IEnumerable<ClusterConnection>> GetClusterConnectionsAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync("cluster-service/cluster-connections");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<ClusterConnection>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<ClusterType>> GetRegisteredClusterTypesAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync("cluster-service/clusters");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<ClusterType>>(await response.Content.ReadAsStringAsync());
        }
        
        public async Task<IEnumerable<Endpoint>> GetRegisteredEndpointsAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync("endpoint-service/endpoints");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<Endpoint>>(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteClusterConnectionAsync(string id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"cluster-service/cluster-connections/{id}");
            response.EnsureSuccessStatusCode();
        }

        #endregion Public Methods

        #region Private Methods

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("user_client");

        #endregion Private Methods

    }
}
