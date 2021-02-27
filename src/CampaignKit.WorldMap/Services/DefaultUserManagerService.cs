// <copyright file="DefaultUserManagerService.cs" company="Jochen Linnemann - IT-Service">
// Copyright (c) 2017-2021 Jochen Linnemann, Cory Gill.
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
// </copyright>

namespace CampaignKit.WorldMap.Services
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.Extensions.Configuration;

    using Serilog;

    /// <inheritdoc />
    /// <summary>
    ///     Class DefaultUserManagerService.
    /// </summary>
    /// <seealso cref="T:CampaignKit.WorldMap.Services.IUserManagerService" />
    public class DefaultUserManagerService : IUserManagerService
    {
        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultUserManagerService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public DefaultUserManagerService(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.loggerService = new LoggerConfiguration().ReadFrom.Configuration(this.configuration).CreateLogger();
        }

        /// <summary>
        /// Gets the built in system user.
        /// </summary>
        /// <returns>
        /// A ClaimsPrincipal object representing the system user.
        /// </returns>
        public ClaimsPrincipal GetSystemUser()
        {
            return new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "SystemAccount"),
                    }));
        }

        /// <summary>
        ///     Derives the user's userId from the list of their claims.
        /// </summary>
        /// <param name="user">The authorized user.</param>
        /// <returns>UserId (String) if found otherwise Null.</returns>
        public string GetUserId(ClaimsPrincipal user)
        {
            if (user == null)
            {
                return null;
            }

            if (user.Claims.Count(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")) == 0)
            {
                return null;
            }

            return user.Claims.First(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value;
        }
    }
}