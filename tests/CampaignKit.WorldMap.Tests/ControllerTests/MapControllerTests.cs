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

namespace CampaignKit.WorldMap.Tests.ControllerTests
{
    public class MapControllerTests : IClassFixture<TestFixture<Startup, TestStartupAuth>>
    {
        #region Fields

        private readonly TestFixture<Startup, TestStartupAuth> _testFixture;

        #endregion

        #region Constructors

        public MapControllerTests(TestFixture<Startup, TestStartupAuth> testFixture)
        {
            _testFixture = testFixture;
        }

        #endregion

        #region Methods

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

        #endregion
    }
}