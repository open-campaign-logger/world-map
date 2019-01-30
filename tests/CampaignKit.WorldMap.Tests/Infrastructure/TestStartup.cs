using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
	}
}
