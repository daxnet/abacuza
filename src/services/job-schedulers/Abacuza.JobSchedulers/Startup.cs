using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abacuza.Common;
using Abacuza.Common.DataAccess;
using Abacuza.DataAccess.Mongo;
using Abacuza.JobSchedulers.Models;
using Abacuza.JobSchedulers.Services;
using McMaster.NETCore.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz.Impl;
using Quartz.Spi;

namespace Abacuza.JobSchedulers
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
            });

            services.AddSwaggerGen(c =>
            {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var pluginsDirectory = Path.Combine(AppContext.BaseDirectory, "plugins");
            var loaders = new List<PluginLoader>();
            
            var mongoHost = Configuration["mongo:host"];
            var mongoPort = int.Parse(Configuration["mongo:port"]);
            var mongoDatabase = Configuration["mongo:database"];
            services.AddTransient<IDataAccessObject>(sp => new MongoDataAccessObject(mongoDatabase, mongoHost, mongoPort));

            services.AddHttpClient<ClusterApiService>(config =>
            {
                config.BaseAddress = new Uri(Configuration["CLUSTER_SERVICE_URL"]);
                config.Timeout = TimeSpan.FromMinutes(5);
            });

            // Initializes the job scheduler
            var quartzSchedulerSettings = new NameValueCollection
            {
                { "quartz.jobStore.type", " Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
                { "quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz" },
                { "quartz.jobStore.dataSource", "myDS" },
                { "quartz.dataSource.myDS.connectionString", "Server=localhost\\sqlexpress; Database=AbacuzaQuartzDB; Integrated Security=SSPI;" },
                { "quartz.dataSource.myDS.provider", "SqlServer" },
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Abacuza Job Scheduler API");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
