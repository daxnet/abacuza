// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020 by daxnet. All rights reserved.
// Licensed under LGPL-v3
// ==============================================================

using Abacuza.Common;
using Abacuza.JobSchedulers.Clusters.Spark.Models;
using Abacuza.JobSchedulers.Common;
using Abacuza.JobSchedulers.Common.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Abacuza.JobSchedulers.Clusters.Spark
{
    /// <summary>
    /// Represents the Apache Spark cluster.
    /// </summary>
    /// <seealso cref="Abacuza.JobSchedulers.Common.Cluster{Abacuza.JobSchedulers.Clusters.Spark.SparkClusterConnection}" />
    [AbacuzaCluster("fbc7b771-1053-44b1-bb1b-5c162b5fd91b",
        "spark",
        typeof(SparkClusterConnection),
        Description = "Apache Spark Cluster")]
    public sealed class SparkCluster : Cluster<SparkClusterConnection>
    {
        #region Protected Methods

        protected override async Task<PagedResult<JobResponse>> GetJobsAsync(SparkClusterConnection connection, int pageNumber = 0, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(connection.Uri);
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"batches?from={pageNumber * pageSize}&size={pageSize}");
                var responseMessage = await httpClient.SendAsync(requestMessage, cancellationToken);
                var batchResponse = JsonConvert.DeserializeObject<GetBatchesResponse>(await responseMessage.Content.ReadAsStringAsync());
                var jobs = batchResponse.Batches.Select(b => new JobResponse
                {
                    Id = b.Id.ToString(),
                    Logs = b.Logs.ToArray(),
                    State = b.State
                });

                return new PagedResult<JobResponse>(jobs, pageNumber, pageSize, batchResponse.Total, ((batchResponse.Total - 1) / pageSize) + 1);
            }
        }

        #endregion Protected Methods
    }
}