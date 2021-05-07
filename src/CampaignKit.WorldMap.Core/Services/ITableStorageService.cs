// <copyright file="ITableStorageService.cs" company="Jochen Linnemann - IT-Service">
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

namespace CampaignKit.WorldMap.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CampaignKit.WorldMap.Core.Entities;

    /// <summary>
    /// TableStorageService interface.
    /// </summary>
    public interface ITableStorageService
    {
        /// <summary>
        /// Creates a map record asynchronously.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>The auto-generated mapid for the new map record.</returns>
        public Task<string> CreateMapRecordAsync(Map map);

        /// <summary>
        /// Creates a map tile record asynchronously.
        /// </summary>
        /// <param name="tile">The map tile.</param>
        /// <returns>The auto-generated tileid for the new map tile record.</returns>
        public Task<string> CreateTileRecordAsync(Tile tile);

        /// <summary>
        /// Updates a map record asynchronously.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>True if the operation succeeds, false otherwise.</returns>
        public Task<bool> UpdateMapRecordAsync(Map map);

        /// <summary>
        /// Updates a map tile record asynchronously.
        /// </summary>
        /// <param name="tile">The map tile.</param>
        /// <returns>True if the operation succeeds, false otherwise.</returns>
        public Task<bool> UpdateTileRecordAsync(Tile tile);

        /// <summary>
        /// Deletes a map record and any associated tile records asynchronously.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>True if the operation succeeds, false otherwise.</returns>
        public Task<bool> DeleteMapRecordAsync(Map map);

        /// <summary>
        /// Deletes a tile record asynchronously.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <returns>True if the operation succeeds, false otherwise.</returns>
        public Task<bool> DeleteTileRecordAsync(Tile tile);

        /// <summary>
        /// Gets a map record and any associated tile records asynchronously.
        /// </summary>
        /// <param name="mapId">The map's unique id.</param>
        /// <returns>The map and its tiles if found, null otherwise.</returns>
        public Task<Map> GetMapRecordAsync(string mapId);

        /// <summary>
        /// Gets a tile record asynchronously.
        /// </summary>
        /// <param name="tileId">The tile's unique id.</param>
        /// <returns>The tile if found, null otherwise.</returns>
        public Task<Tile> GetTileRecordAsync(string tileId);

        /// <summary>
        /// Gets all map records for the specified user asynchronously.
        /// </summary>
        /// <param name="userId">The user's id.  Null if anonymous user.</param>
        /// <param name="includePublic">whether to include public maps in the results.</param>
        /// <returns>List of maps associated with the user, empty list if no maps found.</returns>
        public Task<List<Map>> GetMapRecordsForUserAsync(string userId, bool includePublic);

        /// <summary>
        /// Gets a list of unprocessed tiles records asynchronously.
        /// </summary>
        /// <returns>List of unprocessed tiles, empty list of no tiles require processing.</returns>
        public Task<List<Tile>> GetUnprocessedTileRecordsAsync();
    }
}
