// <copyright file="DefaultFilePathService.cs" company="Jochen Linnemann - IT-Service">
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
    using System.IO;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <inheritdoc />
    /// <summary>
    ///     Class DefaultAppDataPathService.
    /// </summary>
    /// <seealso cref="T:CampaignKit.WorldMap.Services.IAppDataPathService" />
    public class DefaultFilePathService : IFilePathService
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
        ///     Initializes a new instance of the <see cref="DefaultFilePathService" /> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="loggerService">The application logger service.</param>
        /// <param name="env">The env.</param>
        public DefaultFilePathService(IConfiguration configuration, ILogger<DefaultFilePathService> loggerService, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this.loggerService = loggerService;
            this.AppDataPath = Path.Combine(env.ContentRootPath, "App_Data");
            this.SeedDataPath = Path.Combine(this.AppDataPath, "Sample");
            this.PhysicalWorldBasePath = Path.Combine(env.WebRootPath, "world");
            this.VirtualWorldBasePath = "~/world";
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the application data path.
        /// </summary>
        /// <value>The application data path.</value>
        public string AppDataPath { get; }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the world base path.
        /// </summary>
        /// <value>The world base path.</value>
        public string PhysicalWorldBasePath { get; }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the virtual world base path.
        /// </summary>
        /// <value>The virtual world base path.</value>
        public string VirtualWorldBasePath { get; }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the seed data path.
        /// </summary>
        /// <value>The seed data path.</value>
        public string SeedDataPath { get; }
    }
}