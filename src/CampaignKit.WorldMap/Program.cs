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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CampaignKit.WorldMap.Entities;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json.Linq;

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
			var host = CreateWebHostBuilder(args).Build();

			// Seed the database if required

			using (var scope = host.Services.CreateScope())
			{
				// Get the service provider
				var services = scope.ServiceProvider;

				// Determine if database has been created
				// WorldMap.db
				var filePathService = services.GetService<IFilePathService>();
				var sampleDB = Path.Combine(filePathService.AppDataPath, "Sample", "Sample.db");
				var appDB = Path.Combine(filePathService.AppDataPath, "WorldMap.db");
				if (!File.Exists(appDB))
				{
					File.Copy(sampleDB, appDB);
				}
				
				// Get the data base provide and ensure that it is created and ready.
				var dbContext = services.GetRequiredService<WorldMapDBContext>();
				dbContext.Database.EnsureCreated();
				dbContext.Database.GetDbConnection();

			}

			// Run the web host
			host.Run();
		}

		/// <summary>
		/// Creates the core web host.
		/// 
		/// see: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host?view=aspnetcore-2.2
		/// see: https://github.com/aspnet/Docs/blob/master/aspnetcore/test/integration-tests/samples/2.x/IntegrationTestsSample/src/RazorPagesProject/Program.cs
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();

		#endregion Public Methods

	}


}