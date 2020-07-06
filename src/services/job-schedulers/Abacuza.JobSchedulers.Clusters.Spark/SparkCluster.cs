using System.Data;
using System.Text;
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
using Abacuza.Common.Utilities;
using Abacuza.JobSchedulers.Clusters.Spark.Models;
using Abacuza.JobSchedulers.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public override bool ValidateJobParameters(IEnumerable<KeyValuePair<string, object>> jobParameters)
        {
            return true;
        }

        protected override async Task<Job> SubmitJobInternalAsync(SparkClusterConnection connection, IEnumerable<KeyValuePair<string, object>> jobParameters)
        {
            var payload = jobParameters.ToExpando();
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(connection.Uri);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "batches")
            {
                Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8)
            };

            var responseMessage = await httpClient.SendAsync(requestMessage);
            responseMessage.EnsureSuccessStatusCode();
            var batch = JsonConvert.DeserializeObject<Batch>(await responseMessage.Content.ReadAsStringAsync());
            return new Job
            {
                JobId = batch.Id.ToString(),
                Logs = batch.Logs.ToList(),
                State = batch.State
            };
        }

        #endregion Protected Methods
    }
}