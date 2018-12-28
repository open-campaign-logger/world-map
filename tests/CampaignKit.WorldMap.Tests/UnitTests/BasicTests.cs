using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CampaignKit.WorldMap.Tests.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CampaignKit.WorldMap.Tests.UnitTests
{
	public class BasicTests : IClassFixture<CustomWebApplicationFactory<Startup>>
	{

		private readonly CustomWebApplicationFactory<Startup> webApplicationFactory;
		private HttpClient httpClient;
		private TestFixture<Startup> fixture;

		public BasicTests(CustomWebApplicationFactory<Startup> webApplicationFactory)
		{
			this.webApplicationFactory = webApplicationFactory;
			// this.httpClient = this.webApplicationFactory.CreateDefaultClient(new Uri("https://localhost"));
			fixture = new TestFixture<Startup>();
		}

		[Fact]
		public async Task TestDataExists()
		{
			var results = await webApplicationFactory.MapDataService.FindAll();
			Assert.True(results.ToList().Count > 0);
		}



	}
}
