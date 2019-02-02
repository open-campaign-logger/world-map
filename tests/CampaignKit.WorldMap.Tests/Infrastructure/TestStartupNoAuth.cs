using System;
using CampaignKit.WorldMap.Entities;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
	public class TestStartupNoAuth : Startup
	{

		public TestStartupNoAuth(IHostingEnvironment env) : base (env)
		{
		}

		protected override void ConfigureAuth(IServiceCollection services)
		{
		}

		protected override void ConfigureDB(IServiceCollection services)
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
				var databaseService = scopedServices.GetRequiredService<WorldMapDBContext>();
				databaseService.Database.EnsureCreated();

			}

		}
	}
}
