using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CampaignKit.WorldMap.Entities;
using CampaignKit.WorldMap.Services;
using CampaignKit.WorldMap.Tests.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
		public async Task TestHTTPResponse()
		{
			// Arrange & Act
			var response = await _client.GetAsync("/");
			response.EnsureSuccessStatusCode();
			var stringResponse = await response.Content.ReadAsStringAsync();
		}

		[Fact]
		public void TestDataExists()
		{
			// Build the service provider.
			var hostServices = _factory.Server.Host.Services;

			using (var scope = hostServices.CreateScope())
			{
				// Get the service provider
				var scopedServices = scope.ServiceProvider;

				// Retrieve services from the scoped context
				var service = scopedServices.GetService<MappingContext>();
				Assert.True(service.Maps.Count() > 0);
			}
		}


		[Fact]
		public void TestDatabaseExists()
		{
			// Build the service provider.
			var hostServices = _factory.Server.Host.Services;

			using (var scope = hostServices.CreateScope())
			{
				// Get the service provider
				var scopedServices = scope.ServiceProvider;

				// Retrieve services from the scoped context
				var db = scopedServices.GetService<MappingContext>();
				Assert.True(db.Database.CanConnect());
			}

		}


	}
}
