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

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.Core.Services
{
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
        private readonly ITableStorageService _tableStorageService;

        /// <summary>
        /// The BLOB storage service.
        /// </summary>
        private readonly IBlobStorageService _blobStorageService;

        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultProgressService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="blobStorageService">The blob storage service.</param>
        /// <param name="tableStorageService">The table storage service.</param>
        /// <param name="loggerService">The logger service.</param>
        public DefaultProgressService(IConfiguration configuration, IBlobStorageService blobStorageService, ITableStorageService tableStorageService, ILogger<DefaultProgressService> loggerService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _tableStorageService = tableStorageService ?? throw new ArgumentNullException(nameof(tableStorageService));
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
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
            var map = await _tableStorageService.GetMapRecordAsync(mapId);

            // If map found calculate what percentage of files have been found.
            if (map != null)
            {
                var total = 1; // master-image
                for (int zoomLevel = 0; zoomLevel <= map.MaxZoomLevel; zoomLevel++) {
                    total++; // zoom level image
                    total += (int)Math.Pow(2, 2*zoomLevel);
                }

                var createdMapFileCount = await _blobStorageService.ListFolderContentsAsync($"map{map.MapId}");

                // Are there tiles defined for this map?
                progress = createdMapFileCount.Count / (double) total;
            }

            // Return the progress value
            return progress;
        }
    }
}