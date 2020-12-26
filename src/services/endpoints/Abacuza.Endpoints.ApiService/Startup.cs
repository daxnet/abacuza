using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abacuza.Endpoints.ApiService.Models;
using McMaster.NETCore.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Abacuza.Endpoints.ApiService
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

            var endpoints = DiscoverEndpoints();
            services.AddSingleton(endpoints);

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = true
                    }
                };
            });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Abacuza Endpoints API");
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

        private static EndpointCollection DiscoverEndpoints()
        {
            var pluginsDirectory = Path.Combine(AppContext.BaseDirectory, "plugins");
            var loaders = new List<PluginLoader>();
            var endpoints = new EndpointCollection();
            foreach (var file in Directory.EnumerateFiles(pluginsDirectory, "*.dll", SearchOption.AllDirectories))
            {
                if (File.Exists(file))
                {
                    var loader = PluginLoader.CreateFromAssemblyFile(file,
                        sharedTypes: new[] { typeof(IEndpoint) },
                        configure: (pc) =>
                        {
                            pc.SharedAssemblies.Add(typeof(JsonConvert).Assembly.GetName());
                        });
                    loaders.Add(loader);
                }
            }

            foreach (var loader in loaders)
            {
                var types = loader.LoadDefaultAssembly().GetTypes();

                foreach (var endpointType in types
                    .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    var endpoint = (IEndpoint)Activator.CreateInstance(endpointType);
                    endpoints.Add(endpoint);
                }
            }

            return endpoints;
        }
    }
}
