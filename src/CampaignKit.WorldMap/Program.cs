// <copyright file="Program.cs" company="Jochen Linnemann - IT-Service">
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

namespace CampaignKit.WorldMap
{
    using System;

    using CampaignKit.WorldMap.Data;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Serilog;

    /// <summary>
    /// Main application executable.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">Command line application arguments.</param>
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            InitializeRepository(host);
            host.Run();
        }

        /// <summary>
        /// Creates the host builder.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Program initialization utility.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, builder) =>
                {
                    builder.Sources.Clear();

                    var env = hostingContext.HostingEnvironment;

                    builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    builder.AddEnvironmentVariables();

                    if (args != null)
                    {
                        builder.AddCommandLine(args);
                    }

                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        builder.AddUserSecrets<Program>();
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseSerilog()
                    .UseStartup<Startup>();
                });

        /// <summary>
        /// Initializes the repository if required.
        /// </summary>
        /// <param name="host">The host.</param>
        private static void InitializeRepository(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var repositoryService = services.GetRequiredService<IMapRepository>();
                    repositoryService.InitRepository().Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred initializing the repository: {0}", ex.Message);
                }
            }
        }
    }
}