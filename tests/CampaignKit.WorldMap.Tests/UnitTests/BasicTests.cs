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

		private readonly CustomWebApplicationFactory<Startup> _factory;
		private HttpClient _client;

		public BasicTests(CustomWebApplicationFactory<Startup> webApplicationFactory)
		{
			this._factory = webApplicationFactory;
			this._client = webApplicationFactory.CreateClient();
		}

		[Fact]
		public async Task TestDataExists()
		{
			// Arrange & Act
			var response = await _client.GetAsync("/");
			response.EnsureSuccessStatusCode();
			var stringResponse = await response.Content.ReadAsStringAsync();
		}



	}
}
