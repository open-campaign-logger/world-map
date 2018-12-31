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


using CampaignKit.WorldMap.Entities;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.Tests.IntegrationTests
{
	/// <summary>
	/// Reference tutorial: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-2.2
	/// </summary>
	/// <typeparam name="TStartup">The type of the startup.</typeparam>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory{CampaignKit.WorldMap.Startup}" />
	public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
	{
		#region Public Properties
		
		/// <summary>
		/// Gets or sets the database service.
		/// </summary>
		/// <value>
		/// The database service.
		/// </value>
		public WorldMapDBContext DatabaseService { get; set; }

		/// <summary>
		/// Gets or sets the logger service.
		/// </summary>
		/// <value>
		/// The logger service.
		/// </value>
		public ILogger LoggerService { get; set; }

		#endregion

		#region Constructors

		public CustomWebApplicationFactory()
		{

		}

		#endregion

		/// <summary>
		/// Gives a fixture an opportunity to configure the application before it gets built.
		/// </summary>
		/// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> for the application.</param>
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			
			builder.ConfigureServices(services =>
			{
				// Create a new service provider.
				var serviceProvider = new ServiceCollection()
					.AddEntityFrameworkInMemoryDatabase()
					.BuildServiceProvider();

				// Add a database context (MappingContext) using an in-memory 
				// database for testing.
				services.AddDbContext<WorldMapDBContext>(options =>
				{
					options.UseInMemoryDatabase("InMemoryDbForTesting");
					options.UseInternalServiceProvider(serviceProvider);
				});

				// Build the service provider.
				var sp = services.BuildServiceProvider();

				// Create a scope to obtain a reference to the database
				// and other services
				using (var scope = sp.CreateScope())
				{
					// Get a handle to the service provider
					var scopedServices = scope.ServiceProvider;

					// Get a handle to the database service
					DatabaseService = scopedServices.GetRequiredService<WorldMapDBContext>();
					DatabaseService.Database.EnsureCreated();

					// Get a handle to the logging service
					LoggerService = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

				}
			});
		}
	}
}
