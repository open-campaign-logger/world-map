// <copyright file="DefaultRandomDataService.cs" company="Jochen Linnemann - IT-Service">
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
    using System.Text;
    using Microsoft.Extensions.Configuration;

    using Serilog;

    /// <inheritdoc />
    /// <summary>
    ///     Class DefaultRandomDataService.
    /// </summary>
    /// <seealso cref="T:CampaignKit.WorldMap.Services.IRandomDataService" />
    public class DefaultRandomDataService : IRandomDataService
    {
        /// <summary>
        /// The pseudo-random number generator.
        /// </summary>
        private readonly Random rand = new Random();

        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRandomDataService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public DefaultRandomDataService(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.loggerService = new LoggerConfiguration().ReadFrom.Configuration(this.configuration).CreateLogger();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the random text.
        /// </summary>
        /// <param name="numberOfCharacters">The number of characters.</param>
        /// <returns>System.String.</returns>
        public string GetRandomText(int numberOfCharacters)
        {
            // ReSharper disable once StringLiteralTypo
            const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var sb = new StringBuilder();
            for (var i = 0; i < numberOfCharacters; i++)
            {
                var next = this.rand.Next(characters.Length);
                sb.Append(characters[next]);
            }

            return sb.ToString();
        }
    }
}