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
        /// The auto-generated mapid for the new map record, null if the operation fails.
        /// </returns>
        public async Task<string> CreateMapRecordAsync(Map map)
        {
            // Setup the partition key
            map.PartitionKey = map.UserId;

            // Create a mapId if one not provided.
            if (string.IsNullOrEmpty(map.MapId))
            {
                map.MapId = Guid.NewGuid().ToString();
            }

            // Setup the row key
            map.RowKey = map.MapId;

            try
            {
                // Initialize connection to Azure table storage
                var cloudStorageConnectionString = this.configuration.GetConnectionString("AzureTableStorage");
                var cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
                var cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());

                // Connect to the worldmapmaps table.
                var cloudTableName = "worldmapmaps";
                var cloudTable = cloudTableClient.GetTableReference(cloudTableName);

                // Create an insert operation
                var insertOperation = TableOperation.Insert(map);

                // Execute the operation
                var operationResult = await cloudTable.ExecuteAsync(insertOperation);
                var insertedEntity = operationResult.Result as Map;

                return insertedEntity.MapId;
            }
            catch (StorageException ex)
            {
                this.loggerService.LogError("Unable to create map entity: {0}", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Creates a map tile record asynchronously.
        /// </summary>
        /// <param name="tile">The map tile.</param>
        /// <returns>
        /// The auto-generated tileid for the new map tile record, null if the operation fails.
        /// </returns>
        public async Task<string> CreateTileRecordAsync(Tile tile)
        {
            // Setup the partition key
            tile.PartitionKey = tile.MapId;

            // Setup the row key
            tile.RowKey = Guid.NewGuid().ToString();

            try
            {
                // Initialize connection to Azure table storage
                var cloudStorageConnectionString = this.configuration.GetConnectionString("AzureTableStorage");
                var cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
                var cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());

                // Connect to the worlmaptiles table.
                var cloudTableName = "worldmaptiles";
                var cloudTable = cloudTableClient.GetTableReference(cloudTableName);

                // Create an insert operation
                var operation = TableOperation.Insert(tile);

                // Execute the operation
                var operationResult = await cloudTable.ExecuteAsync(operation);
                var insertedEntity = operationResult.Result as Tile;

                return insertedEntity.TileId;
            }
            catch (StorageException ex)
            {
                this.loggerService.LogError("Unable to create tile entity: {0}", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Deletes a map record and any associated tile records asynchronously.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>
        /// True if the operation succeeds, false otherwise.
        /// </returns>
        public async Task<bool> DeleteMapRecordAsync(Map map)
        {
            try
            {
                // Initialize connection to Azure table storage
                var cloudStorageConnectionString = this.configuration.GetConnectionString("AzureTableStorage");
                var cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
                var cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());

                // Connect to the worlmaptiles table.
                var cloudTableName = "worldmaptiles";
                var cloudTable = cloudTableClient.GetTableReference(cloudTableName);

                // Query the tile records associated with the map.
                var query = from tile in cloudTable.CreateQuery<Tile>()
                            where tile.PartitionKey == map.MapId
                            select tile;

                // Delete the tile records.
                foreach (var tile in query)
                {
                    // Create a delete operation
                    var tileOperation = TableOperation.Delete(tile);
                    var tileOperationResult = await cloudTable.ExecuteAsync(tileOperation);
                }

                // Connect to the worlmapmaps table.
                cloudTableName = "worldmapmaps";
                cloudTable = cloudTableClient.GetTableReference(cloudTableName);

                // Delete the map record.
                var mapOperation = TableOperation.Delete(map);
                var mapOperationResult = await cloudTable.ExecuteAsync(mapOperation);

                return true;
            }
            catch (StorageException ex)
            {
                this.loggerService.LogError("Unable to create tile entity: {0}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Deletes a tile record asynchronously.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <returns>
        /// True if the operation succeeds, false otherwise.
        /// </returns>
        public async Task<bool> DeleteTileRecordAsync(Tile tile)
        {
            try
            {
                // Initialize connection to Azure table storage
                var cloudStorageConnectionString = this.configuration.GetConnectionString("AzureTableStorage");
                var cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
                var cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());

                // Connect to the worlmaptiles table.
                var cloudTableName = "worldmaptiles";
                var cloudTable = cloudTableClient.GetTableReference(cloudTableName);

                // Create a delete operation
                var tileOperation = TableOperation.Delete(tile);
                var tileOperationResult = await cloudTable.ExecuteAsync(tileOperation);

                return true;
            }
            catch (StorageException ex)
            {
                this.loggerService.LogError("Unable to create tile entity: {0}", ex.Message);
                return false;
            }
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
            return null;
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
            return null;
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
            return null;
        }

        /// <summary>
        /// Gets a list of unprocessed tiles records asynchronously.
        /// </summary>
        /// <returns>
        /// List of unprocessed tiles, empty list of no tiles require processing.
        /// </returns>
        public Task<List<Tile>> GetUnprocessedTileRecordsAsync()
        {
            return null;
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
            return false;
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
            return false;
        }

        /// <summary>
        /// Initializes Azure tables if required.
        /// </summary>
        /// <returns>
        /// True if succeeds, false otherwise.
        /// </returns>
        public async Task<bool> InitTablesAsync()
        {
            // Initialize connection to Azure table storage
            var storageConnectionString = this.configuration.GetConnectionString("AzureTableStorage");
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            // Verify worldmapmaps table exists
            var tableName = "worldmapmaps";
            var table = tableClient.GetTableReference(tableName);

            // Create the table if required.
            if (await table.CreateIfNotExistsAsync())
            {
                this.loggerService.LogDebug("Created Table named: {0}", tableName);
            }

            // Verify worldmaptiles table exists
            tableName = "worldmaptiles";
            table = tableClient.GetTableReference(tableName);

            // Create the table if required.
            if (await table.CreateIfNotExistsAsync())
            {
                this.loggerService.LogDebug("Created Table named: {0}", tableName);
            }

            return true;
        }
    }
}
