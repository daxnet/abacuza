using Abacuza.Common.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Abacuza.Jobs.ApiService.Services
{
    public sealed class CommonApiService
    {
        #region Private Fields

        private const string CommonServiceUrlConfigurationKey = "services:commonService:url";
        private readonly Uri _commonApiBaseUri;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        #endregion Private Fields

        #region Public Constructors

        public CommonApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _commonApiBaseUri = new Uri(_configuration[CommonServiceUrlConfigurationKey]);
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<(bool, HttpStatusCode, string)> DeleteS3FileAsync(S3File file, CancellationToken cancellationToken = default)
        {
            var normalizedBucket = HttpUtility.UrlEncode(file.Bucket);
            var normalizedKey = HttpUtility.UrlEncode(file.Key);
            var normalizedFile = HttpUtility.UrlEncode(file.File);
            var uri = new Uri(_commonApiBaseUri, $"api/files/{normalizedBucket}/{normalizedKey}/{normalizedFile}");
            using var responseMessage = await _httpClient.DeleteAsync(uri, cancellationToken);
            if (responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return (true, HttpStatusCode.NoContent, string.Empty);
            }

            return (false, responseMessage.StatusCode, await responseMessage.Content.ReadAsStringAsync());
        }

        #endregion Public Methods
    }
}
