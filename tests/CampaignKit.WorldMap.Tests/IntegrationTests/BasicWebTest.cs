using Microsoft.AspNetCore.Mvc.Testing;

using System.Net.Http;
using System.Threading.Tasks;
using CampaignKit.WorldMap.Tests.Infrastructure;

using Xunit;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
    public class BasicWebTest : IClassFixture<TestFixture>
    {
        protected readonly HttpClient _client;

        public BasicWebTest(TestFixture factory)
        {
            _client = factory.CreateClient();
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
            var response = await _client.GetAsync(page);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains(titleTestString, stringResponse);
        }

    }
}
