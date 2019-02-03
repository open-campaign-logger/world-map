using System.Threading.Tasks;
using CampaignKit.WorldMap.Tests.Infrastructure;
using Xunit;

namespace CampaignKit.WorldMap.Tests.ControllerTests
{
	public class HomeControllerTests : IClassFixture<TestFixture<Startup, TestStartupAuth>>
	{

		private readonly TestFixture<Startup, TestStartupAuth> _testFixture;

		public HomeControllerTests(TestFixture<Startup, TestStartupAuth> testFixture)
		{
			this._testFixture = testFixture;
		}

		[Fact]
		public async Task TestJWTCookie()
		{
			// Is the application home page available
			var response = await _testFixture.Client.GetAsync("/Home/JWTCookie");
			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();
			var stringResponse = await response.Content.ReadAsStringAsync();
		}
	}
}
