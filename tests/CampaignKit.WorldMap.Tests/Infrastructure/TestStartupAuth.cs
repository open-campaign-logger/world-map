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

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
    public class TestStartupAuth : TestStartupNoAuth
    {
        #region Constructors

        public TestStartupAuth(IHostingEnvironment env) : base(env)
        {
        }

        #endregion

        #region Methods

        protected override void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test Scheme"; // has to match scheme in TestAuthenticationExtensions
                options.DefaultChallengeScheme = "Test Scheme";
            }).AddTestAuth(o => { });
        }

        #endregion
    }
}