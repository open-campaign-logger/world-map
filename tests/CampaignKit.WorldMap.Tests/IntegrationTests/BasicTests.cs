using System.Threading.Tasks;

using Xunit;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
	public class BasicTests
	{

		[Fact]
		public async Task TestWebServer()
		{
			var testFixture = new TestFixture<Startup, TestStartupNoAuth>();
			var client = testFixture.Client;

			// Arrange & Act
			var response = await client.GetAsync("/");
			response.EnsureSuccessStatusCode();
			var stringResponse = await response.Content.ReadAsStringAsync();
		}
		
		[Fact]
		public void TestDatabase()
		{
			// Build the service provider.
			var testFixture = new TestFixture<Startup, TestStartupNoAuth>();

			Assert.True(testFixture.DatabaseService.Database.CanConnect());
		}
			   
	}
}
