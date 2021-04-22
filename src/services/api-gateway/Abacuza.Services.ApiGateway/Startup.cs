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

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.IO;
using System.Reflection;

namespace Abacuza.Services.ApiGateway
{
    public class Startup
    {

        #region Public Constructors

        public Startup(IConfiguration configuration) => Configuration = configuration;

        #endregion Public Constructors

        #region Public Properties

        public IConfiguration Configuration { get; }

        #endregion Public Properties

        #region Public Methods

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");

            app.UseSwaggerForOcelotUI();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOcelot().Wait();
            
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var idsAuthority = Configuration["ids:authority"] ?? "https://localhost:9051";
            var authenticationProviderKey = "authKey";
            Action<JwtBearerOptions> jwtBearerOptions = o =>
            {
                o.Authority = idsAuthority;
                o.RequireHttpsMetadata = true;
            };

            services.AddOcelot();
            services.AddAuthentication().AddJwtBearer(authenticationProviderKey, jwtBearerOptions);

            services.AddCors(options => options.AddPolicy("AllowAll", p =>
                p.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()));

            services.AddSwaggerForOcelot(Configuration);

            services.AddControllers();
        }

        #endregion Public Methods

    }
}