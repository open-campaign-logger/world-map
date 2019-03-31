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

using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
    public class TestAuthHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        #region Constructors

        public TestAuthHandler(IOptionsMonitor<TestAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        #endregion

        #region Methods

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticationTicket = new AuthenticationTicket(
                new ClaimsPrincipal(Options.Identity),
                new AuthenticationProperties(),
                "Test Scheme");

            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }

        #endregion
    }

    public static class TestAuthenticationExtensions
    {
        #region Methods

        public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthHandler>("Test Scheme", "Test Auth", configureOptions);
        }

        #endregion
    }

    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        #region Static Fields

        public static string TEST_ID = "817be42b-3c0c-4389-841e-97e53ef51337";

        #endregion

        #region Properties

        public virtual ClaimsIdentity Identity { get; } = new ClaimsIdentity(new[]
        {
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", TEST_ID)
        }, "test");

        #endregion
    }
}