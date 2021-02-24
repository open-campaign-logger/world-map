// <copyright file="TableStorageService.cs" company="Jochen Linnemann - IT-Service">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CampaignKit.WorldMap.Entities;

    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Default table storage service.
    /// </summary>
    /// <seealso cref="CampaignKit.WorldMap.Services.ITableStorageService" />
    public class DefaultTableStorageService : ITableStorageService
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
        /// Initializes a new instance of the <see cref="DefaultTableStorageService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerService">The logger service.</param>
        public DefaultTableStorageService(IConfiguration configuration, ILogger loggerService)
        {
            this.configuration = configuration;
            this.loggerService = loggerService;
        }

        /// <summary>
        /// Creates a map record asynchronously.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>
        /// The auto-generated mapid for the new map record.
        /// </returns>
        public async Task<string> CreateMapRecordAsync(Map map)
        {
            // Setup the partition key
            map.PartitionKey = map.UserId;

            // Setup the row key
            var rowKey = Guid.NewGuid().ToString();
            map.MapId = rowKey;
            map.RowKey = map.MapId;

            // Initialize connection to Azure table storage
            var storageConnectionString = this.configuration.GetConnectionString("AzureTableStorage");
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var tableName = "worldmapmaps";
            var table = tableClient.GetTableReference(tableName);

            // Create the table if required.
            if (await table.CreateIfNotExistsAsync())
            {
                this.loggerService.LogDebug("Created Table named: {0}", tableName);
            }

        }

        /// <summary>
        /// Creates a map tile record asynchronously.
        /// </summary>
        /// <param name="tile">The map tile.</param>
        /// <returns>
        /// The auto-generated tileid for the new map tile record.
        /// </returns>
        public Task<string> CreateTileRecordAsync(Tile tile)
        {
        }

        /// <summary>
        /// Deletes a map record and any associated tile records asynchronously.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>
        /// True if the operation succeeds, false otherwise.
        /// </returns>
        public Task<bool> DeleteMapRecordAsync(Map map)
        {
        }

        /// <summary>
        /// Deletes a tile record asynchronously.
        /// </summary>
        /// <param name="map"></param>
        /// <returns>
        /// True if the operation succeeds, false otherwise.
        /// </returns>
        public Task<bool> DeleteTileRecordAsync(Map map)
        {
        }

        /// <summary>
        /// Gets a map record and any associated tile records asynchronously.
        /// </summary>
        /// <param name="mapId">The map's unique id.</param>
        /// <returns>
        /// The map and its tiles if found, null otherwise.
        /// </returns>
        public Task<Map> GetMapRecordAsync(string mapId)
        {
        }

        /// <summary>
        /// Gets all map records for the specified user asynchronously.
        /// </summary>
        /// <param name="userId">The user's id.  Null if anonymous user.</param>
        /// <param name="includePublic">whether to include public maps in the results.</param>
        /// <returns>
        /// List of maps associated with the user, empty list if no maps found.
        /// </returns>
        public Task<List<Map>> GetMapRecordsForUserAsync(string userId, bool includePublic)
        {
        }

        /// <summary>
        /// Gets a tile record asynchronously.
        /// </summary>
        /// <param name="tileId">The tile's unique id.</param>
        /// <returns>
        /// The tile if found, null otherwise.
        /// </returns>
        public Task<Map> GetTileRecordAsync(string tileId)
        {
        }

        /// <summary>
        /// Gets a list of unprocessed tiles records asynchronously.
        /// </summary>
        /// <returns>
        /// List of unprocessed tiles, empty list of no tiles require processing.
        /// </returns>
        public Task<List<Tile>> GetUnprocessedTileRecordsAsync()
        {
        }

        /// <summary>
        /// Updates a map record asynchronously.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>
        /// True if the operation succeeds, false otherwise.
        /// </returns>
        public Task<bool> UpdateMapRecordAsync(Map map)
        {
        }

        /// <summary>
        /// Updates a map tile record asynchronously.
        /// </summary>
        /// <param name="tile">The map tile.</param>
        /// <returns>
        /// True if the operation succeeds, false otherwise.
        /// </returns>
        public Task<bool> UpdateTileRecordAsync(Tile tile)
        {
        }
    }
}
