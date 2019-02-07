using System.Threading.Tasks;
using CampaignKit.WorldMap.Tests.Infrastructure;
using Xunit;


namespace CampaignKit.WorldMap.Tests.ControllerTests
{
	public class MapControllerTests : IClassFixture<TestFixture<Startup, TestStartupAuth>>
	{
		private readonly TestFixture<Startup, TestStartupAuth> _testFixture;

		public MapControllerTests(TestFixture<Startup, TestStartupAuth> testFixture)
		{
			this._testFixture = testFixture;
		}

		[Theory]
		[InlineData("/Map/Create")]
		[InlineData("/Map/Delete")]
		[InlineData("/Map/Edit")]
		[InlineData("/Map/Progress")]
		public async Task TestAuthorizedGets(string page)
		{
			var response = await _testFixture.Client.GetAsync(page);
			response.EnsureSuccessStatusCode();
			var token = await AntiForgeryHelper.EnsureAntiForgeryTokenAsync(_testFixture.Client, page);
			Assert.NotNull(token);
		}

	}
}
