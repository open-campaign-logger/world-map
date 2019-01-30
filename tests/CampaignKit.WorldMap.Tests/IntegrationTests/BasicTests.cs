using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

using Xunit;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
	public class BasicTests
	{

		[Fact]
		public async Task TestHTTPResponse()
		{

			var testFixture = new TestFixture<Startup>();
			var client = testFixture.Client;

			// Arrange & Act
			var response = await client.GetAsync("/");
			response.EnsureSuccessStatusCode();
			var stringResponse = await response.Content.ReadAsStringAsync();
		}

		[Fact]
		public void TestDataExists()
		{
			//// Build the service provider.
			//var hostServices = _factory.Server.Host.Services;

			//using (var scope = hostServices.CreateScope())
			//{
			//	// Get the service provider
			//	var scopedServices = scope.ServiceProvider;

			//	// Retrieve services from the scoped context
			//	var service = scopedServices.GetService<WorldMapDBContext>();
			//	Assert.True(service.Maps.Count() > 0);
			//}
		}


		[Fact]
		public void TestDatabaseExists()
		{
			//// Build the service provider.
			//var hostServices = _factory.Server.Host.Services;

			//using (var scope = hostServices.CreateScope())
			//{
			//	// Get the service provider
			//	var scopedServices = scope.ServiceProvider;

			//	// Retrieve services from the scoped context
			//	var db = scopedServices.GetService<WorldMapDBContext>();
			//	Assert.True(db.Database.CanConnect());
			//}

		}
			   
	}
}
