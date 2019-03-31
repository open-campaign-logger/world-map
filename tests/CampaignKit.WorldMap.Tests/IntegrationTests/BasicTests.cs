// Copyright 2017-2019 Jochen Linnemann, Cory Gill
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Threading.Tasks;

using CampaignKit.WorldMap.Tests.Infrastructure;

using Xunit;

namespace CampaignKit.WorldMap.Tests.IntegrationTests
{
    public class BasicTests : IClassFixture<TestFixture<Startup, TestStartupNoAuth>>
    {
        public BasicTests(TestFixture<Startup, TestStartupNoAuth> testFixture)
        {
            _testFixture = testFixture;
        }

        private readonly TestFixture<Startup, TestStartupNoAuth> _testFixture;

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

        [Fact]
        public void TestDatabase()
        {
            // Is the database accessible?
            Assert.True(_testFixture.DatabaseService.Database.CanConnect());
        }

        [Fact]
        public async Task TestWebServer()
        {
            // Is the application home page available
            var response = await _testFixture.Client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
        }
    }
}