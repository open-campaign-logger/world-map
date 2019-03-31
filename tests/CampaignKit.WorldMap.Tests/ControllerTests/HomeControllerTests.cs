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
    public class HomeControllerTests : IClassFixture<TestFixture<Startup, TestStartupAuth>>
    {
        public HomeControllerTests(TestFixture<Startup, TestStartupAuth> testFixture)
        {
            _testFixture = testFixture;
        }

        private readonly TestFixture<Startup, TestStartupAuth> _testFixture;

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