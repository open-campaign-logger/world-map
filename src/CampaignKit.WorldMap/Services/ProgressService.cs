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
using System.Linq;

using CampaignKit.WorldMap.Data;

using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.Services
{
    /// <summary>
    ///     Interface IProgressService
    /// </summary>
    public interface IProgressService
    {
        #region Methods

        /// <summary>
        ///     Gets the map creation progress.
        ///     0.0 = 0% .. 1.0 = 100%
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <returns>System.Double.</returns>
        double GetMapProgress(string mapId);

        #endregion
    }

    /// <inheritdoc />
    /// <summary>
    ///     Class DefaultProgressService.
    /// </summary>
    /// <seealso cref="T:CampaignKit.WorldMap.Services.IProgressService" />
    public class DefaultProgressService : IProgressService
    {
        #region Fields

        private readonly WorldMapDBContext _context;
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultProgressService" /> class.
        /// </summary>
        public DefaultProgressService(WorldMapDBContext context,
            ILogger<TileCreationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        #endregion

        #region Implementations

        #region IProgressService Members

        #region Public Methods

        /// <summary>
        ///     Gets the map creation progress.
        ///     0.0 = 0% .. 1.0 = 100%
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <returns>System.Double.</returns>
        public double GetMapProgress(string mapId)
        {
            // Create a default return value
            var progress = (double) 0;

            // Find tiles related to this map
            var tiles = (from t in _context.Tiles select t)
                .Where(t => t.MapId == Convert.ToInt32(mapId))
                .ToList();
            var total = tiles.Count();
            var completed = tiles.Where(t => t.CompletionTimestamp > DateTime.MinValue).Count();

            // Are there tiles defined for this map?
            if (total > 0)
                progress = completed / (double) total;
            else
                progress = 1;

            // Return the progress value
            return progress;
        }

        #endregion Public Methods

        #endregion

        #endregion
    }
}