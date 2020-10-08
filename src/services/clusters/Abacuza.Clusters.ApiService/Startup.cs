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

using Abacuza.Clusters.ApiService.Models;
using Abacuza.Clusters.Common;
using Abacuza.Common.DataAccess;
using Abacuza.DataAccess.DistributedCached;
using Abacuza.DataAccess.Mongo;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Abacuza.Clusters.ApiService
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

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["redis:connectionString"];
            });

            var clusterImplementations = DiscoverClusterImplementations();
            services.AddSingleton(clusterImplementations);

            var mongoConnectionString = Configuration["mongo:connectionString"];
            var mongoDatabase = Configuration["mongo:database"];
            var wrapperDao = new MongoDataAccessObject(new MongoUrl(mongoConnectionString), mongoDatabase);

            services.AddTransient<IDataAccessObject>(sp => new DistributedCachedDataAccessObject(sp.GetService<IDistributedCache>(), wrapperDao));

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Abacuza Cluster API");
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

            lifetime.ApplicationStopping.Register(() =>
            {
                var clusterImplementations = app.ApplicationServices.GetRequiredService<ClusterCollection>();
                foreach (var impl in clusterImplementations)
                {
                    impl.Dispose();
                }
            });
        }

        private ClusterCollection DiscoverClusterImplementations()
        {
            var pluginsDirectory = Path.Combine(AppContext.BaseDirectory, "plugins");
            var loaders = new List<PluginLoader>();
            var clusterImplementations = new ClusterCollection();
            foreach (var file in Directory.EnumerateFiles(pluginsDirectory, "*.dll", SearchOption.AllDirectories))
            {
                if (File.Exists(file))
                {
                    var loader = PluginLoader.CreateFromAssemblyFile(file,
                        sharedTypes: new[] { typeof(ICluster) },
                        configure: (pc) =>
                        {
                            pc.SharedAssemblies.Add(typeof(JsonConvert).Assembly.GetName());
                        });
                    loaders.Add(loader);
                }
            }

            foreach (var loader in loaders)
            {
                foreach (var clusterType in loader
                    .LoadDefaultAssembly()
                    .GetTypes()
                    .Where(t => typeof(ICluster).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    // This assumes the implementation of IPlugin has a parameterless constructor
                    var cluster = (ICluster)Activator.CreateInstance(clusterType);
                    clusterImplementations.Add(cluster);
                }
            }

            return clusterImplementations;
        }
    }
}