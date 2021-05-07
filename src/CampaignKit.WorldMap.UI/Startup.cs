// <copyright file="Startup.cs" company="Jochen Linnemann - IT-Service">
// Copyright (c) 2017-2021 Jochen Linnemann, Cory Gill.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using CampaignKit.WorldMap.Core.Data;
using CampaignKit.WorldMap.Core.Services;
using CampaignKit.WorldMap.UI.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

namespace CampaignKit.WorldMap.UI
{
    /// <summary>
    ///     Class Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            // Json conversion settings
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings { Formatting = Formatting.Indented };

            // Reads the IsDevelopment configuration variable to determine if this is a development environment or not
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable all static file middleware (except directory browsing) for the current request path in the current directory.
            app.UseFileServer();

            // Use custom JWT cookie middleware component
            app.UseMiddleware<JWTInHeaderMiddleware>();

            // Enable authentication
            app.UseAuthentication();

            // Adds MVC to the IApplicationBuilder request execution pipeline.
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(configure => configure.MapControllers());
        }

        /// <summary>
        ///     Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // This method gets called by the runtime. Use this method to add services to the container.
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940

            // Add the MVC service
            services.AddMvc();

            // Configuration Authentiation
            this.ConfigureAuth(services);

            // Add services to context
            services.AddSingleton<IRandomDataService, DefaultRandomDataService>();
            services.AddSingleton<IFilePathService, DefaultFilePathService>();
            services.AddSingleton<IProgressService, DefaultProgressService>();
            services.AddSingleton<IUserManagerService, DefaultUserManagerService>();
            services.AddSingleton<IMapRepository, DefaultMapRepository>();

            // Add storage services
            this.ConfigureStorage(services);
        }

        /// <summary>
        ///     Configures the authentication.
        ///     This virtual method is used to encapsulate authentication
        ///     configuration information in a way that can be easily overridden
        ///     in the unit testing project.
        /// </summary>
        /// <param name="services">The services.</param>
        protected virtual void ConfigureAuth(IServiceCollection services)
        {
            // Configure services to expect a Campaign-Identity access_token in the
            // Authorization header using the JWT Bearer scheme.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://campaign-identity.com";
                    options.Audience = "logger";
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                        // Create an asp.net identity object for the user
                        var userName = context.Principal.FindFirstValue("preferred_username") ??
                        context.Principal.FindFirstValue("name");
                        var identity = new GenericIdentity(userName);
                        context.Principal.AddIdentity(identity);

                        // Store the JWT bearer details in a client side cookie.
                        var tokenString = context.Request.Headers["Authorization"];

                        context.Response.Cookies.Append(
                        ".worldmap.ui",
                        tokenString,
                        new CookieOptions
                                {
                                    Path = "/",
                                });

                        return Task.CompletedTask;
                        },
                    };
                });
        }

        /// <summary>
        ///     Configures the storage provider.
        ///     This virtual method is used to encapsulate storage configuration
        ///     information in a way that can be easily overridden in the unit
        ///     testing project.
        /// </summary>
        /// <param name="services">The services.</param>
        protected virtual void ConfigureStorage(IServiceCollection services)
        {
            services.AddSingleton<IBlobStorageService, DefaultBlobStorageService>();
            services.AddSingleton<ITableStorageService, DefaultTableStorageService>();
            services.AddSingleton<IQueueStorageService, DefaultQueueStorageService>();
        }
    }
}