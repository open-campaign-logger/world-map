// Copyright 2017-2018 Jochen Linnemann
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

using System.IO;
using System;
using System.Linq;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CampaignKit.WorldMap.Services;
using Microsoft.Extensions.Logging;
using CampaignKit.WorldMap.Entities;

namespace CampaignKit.WorldMap
{
    /// <summary>
    ///     Class Program.
    /// </summary>
    public class Program
    {
        #region Public Methods

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
		{
			// Build the web host
			var host = BuildWebHost(args);

			// Seed the database if required

			using (var scope = host.Services.CreateScope())
			{
				// Get the service provider
				var services = scope.ServiceProvider;

				// Get the data base provide and ensure that it is created and ready.
				var dbContext = services.GetService<MappingContext>();
				dbContext.Database.EnsureCreated();
				dbContext.Database.GetDbConnection();

				// Get the map data service provider and test to see if it already contains data			
				var mapDataService = services.GetService<IMapDataService>();
				var maps = mapDataService.FindAll();
				maps.Wait();

				if (maps.Result.Count() == 0)
				{
					// Create an object for the sample map
					var sampleMap = new Map()
					{
						Name = "Sample",
						Secret = "lNtqjEVQ",
						Copyright = String.Empty,
						ContentType = "image/png",
						FileExtension = ".png",
						CreationTimestamp = DateTime.UtcNow,
						RepeatMapInX = false,
					};
					
					// Retrieve the sample map image
					var filePathService = services.GetService<IFilePathService>();
					var imageStream = File.Open(Path.Combine(filePathService.SeedDataPath, "sample.png"), FileMode.Open);

					// Use the data service to create the map
					var mapTask = mapDataService.Create(sampleMap, imageStream);
					mapTask.Wait();
				}


			}

			// Run the web host
			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();

		#endregion Public Methods

	}


}