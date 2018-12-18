﻿// Copyright 2017-2018 Jochen Linnemann
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

using System;

using CampaignKit.WorldMap.Entities;
using CampaignKit.WorldMap.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace CampaignKit.WorldMap
{
	/// <summary>
	///     Class Startup.
	/// </summary>
	public class Startup
    {
        #region Private Fields

        private readonly IConfiguration _configuration;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether this instance is development configured.
        /// </summary>
        /// <value><c>true</c> if this instance is development configured; otherwise, <c>false</c>.</value>
        public bool IsDevelopmentConfigured =>
            string.Equals(_configuration["IsDevelopment"], bool.TrueString, StringComparison.OrdinalIgnoreCase);

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        ///     Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>

        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

			// Json conversion settings
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings { Formatting = Formatting.Indented };

			// Reads the IsDevelopment configuration variable to determine if this is a development environment or not
			if (env.IsDevelopment() || IsDevelopmentConfigured)
			{ 
                app.UseDeveloperExceptionPage();
			}

			// Enable all static file middleware (except directory browsing) for the current request path in the current directory.
			app.UseFileServer();

			// Adds MVC to the IApplicationBuilder request execution pipeline.
			app.UseMvcWithDefaultRoute();
        }

        /// <summary>
        ///     Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
			// This method gets called by the runtime. Use this method to add services to the container.
			// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940

			// Add the configuration to the context
			services.AddSingleton(_configuration);

			// Instantiate the database and add to the context
			services.AddDbContext<MappingContext>
 				(options => options.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));
			
			// Add the MVC service
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			// Add data services to context
			// Note: these have been changed from singleton to scoped services in order
			//       to work with the dbcontext which is scoped.
			services.AddScoped<IFilePathService, DefaultFilePathService>();
            services.AddScoped<IRandomDataService, DefaultRandomDataService>();
            services.AddScoped<IProgressService, DefaultProgressService>();
			services.AddScoped<IMapDataService, DefaultMapDataService>();

			// Add background services
			services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, TileCreationService>();
			

			// Ensure that the sample map has been setup

		}

        #endregion Public Methods
    }
}