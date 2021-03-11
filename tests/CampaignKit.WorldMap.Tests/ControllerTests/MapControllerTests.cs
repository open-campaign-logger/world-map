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

using System.Net.Http;
using System.Threading.Tasks;

using CampaignKit.WorldMap.Tests.Infrastructure;

using Xunit;

namespace CampaignKit.WorldMap.Tests.ControllerTests
{
    public class MapControllerTests : IClassFixture<TestFixture>
    {
        protected readonly HttpClient _client;

        public MapControllerTests(TestFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("/Map/Create")]
        [InlineData("/Map/Delete/1")]
        [InlineData("/Map/Edit/1")]
        public async Task TestAuthorizedGetsWithForms(string page)
        {
            var token = await AntiForgeryHelper.EnsureAntiForgeryTokenAsync(_client, page);
            Assert.NotNull(token);
        }

        [Theory]
        [InlineData("/Map/Progress/1")]
        public async Task TestAuthorizedGetsWithoutForms(string page)
        {
            var response = await _client.GetAsync(page);
            response.EnsureSuccessStatusCode();
        }
    }
}