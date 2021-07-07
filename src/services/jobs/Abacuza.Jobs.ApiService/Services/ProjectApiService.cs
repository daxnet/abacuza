// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020-2021 by daxnet. All rights reserved.
// Apache License Version 2.0
// ==============================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Abacuza.Jobs.ApiService.Services
{
    public sealed class ProjectApiService
    {
        #region Private Fields

        private const string ProjectServiceUrlConfigurationKey = "services:projectService:url";
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProjectApiService> _logger;
        private readonly Uri _projectApiBaseUri;

        #endregion Private Fields

        #region Public Constructors

        public ProjectApiService(HttpClient httpClient, IConfiguration configuration, ILogger<ProjectApiService> logger)
        {
            (_httpClient, _configuration, _logger) = (httpClient, configuration, logger);
            _projectApiBaseUri = new Uri(_configuration[ProjectServiceUrlConfigurationKey]);
        }

        #endregion Public Constructors

        public async Task<IEnumerable<Guid>?> CheckJobRunnerUsageAsync(Guid id)
        {
            var url = new Uri(_projectApiBaseUri, $"api/projects/job-runner-usage/{id}");
            using var responseMessage = await _httpClient.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();
            var responseJson = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Guid[]>(responseJson);
        }
    }
}