using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using CampaignKit.WorldMap.Controllers;
using CampaignKit.WorldMap.Entities;
using CampaignKit.WorldMap.Tests.IntegrationTests;
using CampaignKit.WorldMap.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Xunit;

namespace CampaignKit.WorldMap.Tests.ControllerTests
{
	public class MapControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
	{

		private readonly CustomWebApplicationFactory<Startup> _factory;
		private HttpClient _client;

		public MapControllerTests(CustomWebApplicationFactory<Startup> webApplicationFactory)
		{
			this._factory = webApplicationFactory;
			this._client = webApplicationFactory.CreateClient();
		}

		[Theory]
		[InlineData("/Map", "<h3>All Maps</h3>")]
		[InlineData("/Map/Create", "<h2>Create your own!</h2>")]
		[InlineData("/Map/Delete/2", "<h2>Not allowed</h2>")]
		[InlineData("/Map/Edit/2", "<h2>Not allowed</h2>")]
		[InlineData("/Map/Sample", "<title>Sample</title>")]
		public async Task TestPageGet(string page, string titleTestString)
		{
			var response = await _client.GetAsync(page);
			response.EnsureSuccessStatusCode();
			var stringResponse = await response.Content.ReadAsStringAsync();
			Assert.Contains(titleTestString, stringResponse);
		}

		[Fact]
		public async Task TestMapEdit()
		{
			// Build the service provider.
			var hostServices = _factory.Server.Host.Services;

			using (var scope = hostServices.CreateScope())
			{
				// Get the service provider
				var scopedServices = scope.ServiceProvider;

				// Retrieve services from the scoped context
				var mapRepository = scopedServices.GetService<IMapRepository>();
				var loggerService = scopedServices.GetService<ILogger<MapController>>();
				var randomDataService = scopedServices.GetService<IRandomDataService>();
				var progressService = scopedServices.GetService<IProgressService>();
				var filePathService = scopedServices.GetService<IFilePathService>();
				var dbContext = scopedServices.GetService<WorldMapDBContext>();

				// Instantiate the controller
				var controller = new MapController(randomDataService, mapRepository, progressService, filePathService, dbContext, loggerService);

				// call the controller method
				var viewModel = new MapEditViewModel()
				{
					Copyright = "Copy",
					Name = "Map2_Altered",
					RepeatMapInX = true
				};
				IActionResult result = await controller.Edit(3, "Map3", viewModel);

				// Assert that map has been deleted
				var editedMap = await mapRepository.Find(3);
				Assert.Equal(viewModel.Copyright, editedMap.Copyright);
				Assert.Equal(viewModel.Name, editedMap.Name);
				Assert.Equal(viewModel.RepeatMapInX, editedMap.RepeatMapInX);

			}

		}

		[Fact]
		public async Task TestMapDelete()
		{
			// Build the service provider.
			var hostServices = _factory.Server.Host.Services;

			using (var scope = hostServices.CreateScope())
			{
				// Get the service provider
				var scopedServices = scope.ServiceProvider;

				// Retrieve services from the scoped context
				var mapRepository = scopedServices.GetService<IMapRepository>();
				var loggerService = scopedServices.GetService<ILogger<MapController>>();
				var randomDataService = scopedServices.GetService<IRandomDataService>();
				var progressService = scopedServices.GetService<IProgressService>();
				var filePathService = scopedServices.GetService<IFilePathService>();
				var dbContext = scopedServices.GetService<WorldMapDBContext>();

				// Instantiate the controller
				var controller = new MapController(randomDataService, mapRepository, progressService, filePathService, dbContext, loggerService);

				// call the controller method
				var viewModel = new MapDeleteViewModel()
				{
					HiddenId = 2,
					HiddenSecret = "Map2",
					Name = "Map2"
				};
				IActionResult result = await controller.Delete(viewModel.HiddenId, viewModel.HiddenSecret, viewModel);
				
				// Assert that map has been deleted
				var deletedMap = await mapRepository.Find(viewModel.HiddenId);
				Assert.Null(deletedMap);

			}
		}
		
	}
}
