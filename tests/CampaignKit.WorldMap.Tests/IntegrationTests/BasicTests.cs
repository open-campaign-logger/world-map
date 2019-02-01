using System.Threading.Tasks;

using Xunit;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
	public class BasicTests : IClassFixture<TestFixture<Startup, TestStartupNoAuth>>
	{

		private readonly TestFixture<Startup, TestStartupNoAuth> _testFixture;

		public BasicTests(TestFixture<Startup, TestStartupNoAuth> testFixture)
		{
			this._testFixture = testFixture;
		}

		[Fact]
		public async Task TestWebServer()
		{
			// Is the application home page available
			var response = await _testFixture.Client.GetAsync("/");
			response.EnsureSuccessStatusCode();
			var stringResponse = await response.Content.ReadAsStringAsync();
		}
		
		[Fact]
		public void TestDatabase()
		{
			// Is the database accessible?
			Assert.True(_testFixture.DatabaseService.Database.CanConnect());
		}

		[Theory]
		[InlineData("/", "<h3>Welcome, Adventurer!</h3>")]
		[InlineData("/Home", "<h3>Welcome, Adventurer!</h3>")]
		[InlineData("/Home/Index", "<h3>Welcome, Adventurer!</h3>")]
		[InlineData("/Home/Legalities", "<h2>Legalities</h2>")]
		[InlineData("/Map/Sample", "<a href=\"/Map/Sample\">Sample</a>")]
		[InlineData("/Map", "<a href=\"/\">World Map</a> - Map Index")]
		public async Task TestPublicPages(string page, string titleTestString)
		{
			var response = await _testFixture.Client.GetAsync(page);
			response.EnsureSuccessStatusCode();
			var stringResponse = await response.Content.ReadAsStringAsync();
			Assert.Contains(titleTestString, stringResponse);
		}

	}
}
