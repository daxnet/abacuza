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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using Abacuza.Common.DataAccess;
using Abacuza.Common.Utilities;
using Abacuza.DataAccess.DistributedCached;
using Abacuza.DataAccess.Mongo;
using Abacuza.Jobs.ApiService.Models;
using Abacuza.Jobs.ApiService.Services;
using McMaster.NETCore.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using Polly;
using Quartz.Impl;
using Quartz.Spi;

namespace Abacuza.Jobs.ApiService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["redis:connectionString"];
            });

            services.AddSwaggerGen(c =>
            {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            var pluginsDirectory = Path.Combine(AppContext.BaseDirectory, "plugins");
            var loaders = new List<PluginLoader>();

            var mongoConnectionString = Configuration["mongo:connectionString"];
            var mongoDatabase = Configuration["mongo:database"];
            var wrapperDao = new MongoDataAccessObject(new MongoUrl(mongoConnectionString), mongoDatabase);

            services.AddTransient<IDataAccessObject>(sp => new DistributedCachedDataAccessObject(sp.GetService<IDistributedCache>()!, wrapperDao));

            services.AddHttpClient<ClusterApiService>(config =>
            {
                config.BaseAddress = new Uri(Configuration["services:clusterService:url"]);
                config.Timeout = Utils.ParseTimeSpanExpression(Configuration["services:clusterService:timeout"], TimeSpan.FromMinutes(2));
            }).AddTransientHttpErrorPolicy(builder => builder.RetryAsync(Convert.ToInt32(Configuration["services:clusterService:retries"])));

            services.AddHttpClient<CommonApiService>(config =>
            {
                config.BaseAddress = new Uri(Configuration["services:commonService:url"]);
                config.Timeout = Utils.ParseTimeSpanExpression(Configuration["services:commonService:timeout"], TimeSpan.FromSeconds(2));
            }).AddTransientHttpErrorPolicy(builder => builder.RetryAsync(Convert.ToInt32(Configuration["services:commonService:retries"])));

            services.AddHttpClient<ProjectApiService>(config =>
            {
                config.BaseAddress = new Uri(Configuration["services:projectService:url"]);
                config.Timeout = Utils.ParseTimeSpanExpression(Configuration["services:projectService:timeout"], TimeSpan.FromSeconds(2));
            }).AddTransientHttpErrorPolicy(builder => builder.RetryAsync(Convert.ToInt32(Configuration["services:projectService:retries"])));

            // Initializes the job scheduler
            var quartzSchedulerSettings = new NameValueCollection
            {
                { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
                { "quartz.jobStore.driverDelegateType", Configuration["quartz:driverDelegateType"] },
                { "quartz.jobStore.dataSource", "myDS" },
                { "quartz.dataSource.myDS.connectionString", Configuration["quartz:dataSource:connectionString"] },
                { "quartz.dataSource.myDS.provider", Configuration["quartz:dataSource:provider"] },
                { "quartz.jobStore.useProperties", "false" },
                { "quartz.serializer.type", "json" },
                { "quartz.scheduler.instanceId", "AUTO" },
                { "quartz.jobStore.clustered", "true" }
            };

            var schedulerFactory = new StdSchedulerFactory(quartzSchedulerSettings);
            var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton(scheduler);
            services.AddSingleton<JobSubmitExecutor>();
            services.AddSingleton<JobUpdateExecutor>();
            services.AddHostedService<JobSchedulingHostedService>();
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Abacuza Jobs API");
            });

            app.UseRouting();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}