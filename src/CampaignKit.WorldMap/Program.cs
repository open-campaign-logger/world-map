// Copyright 2017-2020 Jochen Linnemann, Cory Gill
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
using System.IO;
using CampaignKit.WorldMap.Data;
using CampaignKit.WorldMap.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CampaignKit.WorldMap
{
    /// <summary>
    ///     Class Program.
    /// </summary>
    public static class Program
    {
        #region Methods

        /// <summary>
        ///     Creates the core web host.
        ///     see: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host?view=aspnetcore-2.2
        ///     see:
        ///     https://github.com/aspnet/Docs/blob/master/aspnetcore/test/integration-tests/samples/2.x/IntegrationTestsSample/src/RazorPagesProject/Program.cs
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>

        // ReSharper disable once MemberCanBePrivate.Global
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(configure =>
                    configure
                        .UseSerilog()
                        .UseStartup<Startup>()
                );
        }

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                    true)
                .AddEnvironmentVariables()
                .Build();

            var logFile = "Logs/worldmap_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.File(logFile)
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");

                // Build the web host
                var host = CreateHostBuilder(args).Build();

                // Seed the database if required
                using (var scope = host.Services.CreateScope())
                {
                    // Get the service provider
                    var services = scope.ServiceProvider;

                    // Determine if database has been created
                    // WorldMap.db
                    var filePathService = services.GetService<IFilePathService>();
                    var sampleDb = Path.Combine(filePathService.AppDataPath, "Sample", "Sample.db");
                    var appDb = Path.Combine(filePathService.AppDataPath, "WorldMap.db");
                    if (!File.Exists(appDb)) File.Copy(sampleDb, appDb);

                    // Get the database provider and ensure that it is created and ready.
                    var dbContext = services.GetRequiredService<WorldMapDBContext>();
                    dbContext.Database.Migrate();
                }

                // Run the web host
                host.Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Web host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        #endregion
    }
}