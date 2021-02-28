// <copyright file="DefaultProgressService.cs" company="Jochen Linnemann - IT-Service">
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
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <inheritdoc />
    /// <summary>
    ///     Class DefaultProgressService.
    /// </summary>
    /// <seealso cref="T:CampaignKit.WorldMap.Services.IProgressService" />
    public class DefaultProgressService : IProgressService
    {
        /// <summary>
        /// The table storage service.
        /// </summary>
        private readonly ITableStorageService tableStorageService;

        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultProgressService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="tableStorageService">The table storage service.</param>
        /// <param name="loggerService">The logger service.</param>
        public DefaultProgressService(IConfiguration configuration, ITableStorageService tableStorageService, ILogger<DefaultProgressService> loggerService)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.tableStorageService = tableStorageService ?? throw new ArgumentNullException(nameof(tableStorageService));
            this.loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
        }

        /// <summary>
        ///     Gets the map creation progress.
        ///     0.0 = 0% .. 1.0 = 100%.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <returns>System.Double.</returns>
        public async Task<double> GetMapProgress(string mapId)
        {
            // Create a default return value
            var progress = 0D;

            // Find tiles related to this map
            var map = await this.tableStorageService.GetMapRecordAsync(mapId);
            var total = map.Tiles.Count();
            var completed = map.Tiles.Where(t => t.IsRendered == true).Count();

            // Are there tiles defined for this map?
            if (total > 0)
            {
                progress = completed / (double)total;
            }
            else
            {
                progress = 1;
            }

            // Return the progress value
            return progress;
        }
    }
}