using System;
using CampaignKit.WorldMap.Entities;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
	public class TestStartupAuth : TestStartupNoAuth
	{

		public TestStartupAuth(IHostingEnvironment env) : base (env)
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
