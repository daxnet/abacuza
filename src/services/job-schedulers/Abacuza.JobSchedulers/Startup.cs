using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abacuza.Common;
using Abacuza.Common.DataAccess;
using Abacuza.DataAccess.Mongo;
using Abacuza.JobSchedulers.Common;
using McMaster.NETCore.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var pluginsDirectory = Path.Combine(AppContext.BaseDirectory, "plugins");
            var loaders = new List<PluginLoader>();
            var clusters = new List<ICluster>();
            foreach (var file in Directory.EnumerateFiles(pluginsDirectory, "*.dll", SearchOption.AllDirectories))
            {
                if (File.Exists(file))
                {
                    var loader = PluginLoader.CreateFromAssemblyFile(file, sharedTypes: new[] { typeof(ICluster) });
                    loaders.Add(loader);
                }
            }

            // Create an instance of plugin types
            foreach (var loader in loaders)
            {
                foreach (var clusterType in loader
                    .LoadDefaultAssembly()
                    .GetTypes()
                    .Where(t => typeof(ICluster).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    // This assumes the implementation of IPlugin has a parameterless constructor
                    var cluster = (ICluster)Activator.CreateInstance(clusterType);
                    clusters.Add(cluster);
                }
            }

            services.AddSingleton<IEnumerable<ICluster>>(clusters);

            var mongoHost = Configuration["mongo:host"];
            var mongoPort = int.Parse(Configuration["mongo:port"]);
            var mongoDatabase = Configuration["mongo:database"];
            services.AddTransient<IDataAccessObject>(sp => new MongoDataAccessObject(mongoDatabase, mongoHost, mongoPort));
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
