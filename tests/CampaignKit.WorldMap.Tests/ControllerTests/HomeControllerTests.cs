using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using CampaignKit.WorldMap.Controllers;
using CampaignKit.WorldMap.Entities;
using CampaignKit.WorldMap.Tests.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Xunit;

namespace CampaignKit.WorldMap.Tests.ControllerTests
{
	public class HomeControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
	{

		private readonly CustomWebApplicationFactory<Startup> _factory;
		private HttpClient _client;

		public HomeControllerTests(CustomWebApplicationFactory<Startup> webApplicationFactory)
		{
			this._factory = webApplicationFactory;
			this._client = webApplicationFactory.CreateClient();
		}

		[Fact]
		public async Task TestHomePageListingAsync()
		{
			// Build the service provider.
			var hostServices = _factory.Server.Host.Services;

			using (var scope = hostServices.CreateScope())
			{
				// Get the service provider
				var scopedServices = scope.ServiceProvider;

				// Retrieve services from the scoped context
				var mapRepository = scopedServices.GetService<IMapRepository>();
				var loggerService = scopedServices.GetService<ILogger<HomeController>>();

				// Instantiate the controller
				var controller = new HomeController(mapRepository, loggerService);

				// call the controller method
				IActionResult result = await controller.Index();

				// Assert result is a view
				ViewResult viewResult = Assert.IsType<ViewResult>(result);

				// Assert result contains three maps 
				IEnumerable<Map> model = (IEnumerable<Map>) viewResult.Model;
				var modelList = model.ToList();
				Assert.Equal(3, modelList.Count);

				// Ensure maps are ordered correctly.
				Assert.Equal("Map4", modelList[0].Name);
				Assert.Equal("Map3", modelList[1].Name);
				Assert.Equal("Map2", modelList[2].Name); // Map 1 is excluded from results
			}
		}

		[Theory]
		[InlineData("/", "<h3>Welcome, Adventurer!</h3>")]
		[InlineData("/Home/Legalities", "<h2>Legalities</h2>")]
		public async Task TestPageGet(string page, string titleTestString)
		{
			var response = await _client.GetAsync(page);
			response.EnsureSuccessStatusCode();
			var stringResponse = await response.Content.ReadAsStringAsync();
			Assert.Contains(titleTestString, stringResponse);
		}
	}
}
