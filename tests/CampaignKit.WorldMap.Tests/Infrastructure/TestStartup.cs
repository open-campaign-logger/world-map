using CampaignKit.WorldMap.Entities;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
	public class TestStartup : Startup
	{

		public TestStartup(IHostingEnvironment env) : base (env)
		{

		}

		protected override void ConfigureAuth(IServiceCollection services)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = "Test Scheme"; // has to match scheme in TestAuthenticationExtensions
				options.DefaultChallengeScheme = "Test Scheme";
			}).AddTestAuth(o => { });
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

		}
	}
}
