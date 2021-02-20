﻿// <copyright file="DefaultProgressService.cs" company="Jochen Linnemann - IT-Service">
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
    using CampaignKit.WorldMap.Data;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <inheritdoc />
    /// <summary>
    ///     Class DefaultProgressService.
    /// </summary>
    /// <seealso cref="T:CampaignKit.WorldMap.Services.IProgressService" />
    public class DefaultProgressService : IProgressService
    {
        private readonly WorldMapDBContext dbContext;

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
        /// <param name="loggerService">The application logger service.</param>
        /// <param name="dbContext">The WorldMapDBContext.</param>
        public DefaultProgressService(
            IConfiguration configuration,
            ILogger<DefaultProgressService> loggerService,
            WorldMapDBContext dbContext)
        {
            this.configuration = configuration;
            this.loggerService = loggerService;
            this.dbContext = dbContext;
        }

        /// <summary>
        ///     Gets the map creation progress.
        ///     0.0 = 0% .. 1.0 = 100%.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <returns>System.Double.</returns>
        public double GetMapProgress(string mapId)
        {
            // Create a default return value
            var progress = 0D;

            // Find tiles related to this map
            var tiles = (from t in this.dbContext.Tiles select t)
                .Where(t => t.MapId == Convert.ToInt32(mapId))
                .ToList();
            var total = tiles.Count();
            var completed = tiles.Where(t => t.CompletionTimestamp > DateTime.MinValue).Count();

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