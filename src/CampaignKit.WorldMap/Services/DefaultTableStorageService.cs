// <copyright file="DefaultTableStorageService.cs" company="Jochen Linnemann - IT-Service">
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

    using Serilog;

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
        public DefaultTableStorageService(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.loggerService = new LoggerConfiguration().ReadFrom.Configuration(this.configuration).CreateLogger();
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
                var cloudTable = cloudTableClient.GetTableReference("worldmapmaps");

                // Create an insert operation
                var insertOperation = TableOperation.Insert(map);

                // Execute the operation
                await cloudTable.ExecuteAsync(insertOperation);

                return map.MapId;
            }
            catch (StorageException ex)
            {
                this.loggerService.Error("Unable to create map record: {0}", ex.Message);
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
                var cloudTable = cloudTableClient.GetTableReference("worldmaptiles");

                // Create an insert operation
                var operation = TableOperation.Insert(tile);

                // Execute the operation
                await cloudTable.ExecuteAsync(operation);

                return tile.TileId;
            }
            catch (StorageException ex)
            {
                this.loggerService.Error("Unable to create tile record: {0}", ex.Message);
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
                var cloudTable = cloudTableClient.GetTableReference("worldmaptiles");

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
                cloudTable = cloudTableClient.GetTableReference("worldmapmaps");

                // Delete the map record.
                var mapOperation = TableOperation.Delete(map);
                await cloudTable.ExecuteAsync(mapOperation);

                return true;
            }
            catch (StorageException ex)
            {
                this.loggerService.Error("Unable to delete map and tile records: {0}", ex.Message);
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
                var cloudTable = cloudTableClient.GetTableReference("worldmaptiles");

                // Create a delete operation
                var tileOperation = TableOperation.Delete(tile);
                await cloudTable.ExecuteAsync(tileOperation);

                return true;
            }
            catch (StorageException ex)
            {
                this.loggerService.Error("Unable to delete tile record: {0}", ex.Message);
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
            return Task.Run(() => this.GetMapRecord(mapId));
        }

        /// <summary>
        /// Gets all map records for the specified user asynchronously.
        /// </summary>
        /// <param name="userId">The user's id.  Null if anonymous user.</param>
        /// <param name="includePublic">whether to include public maps in the results.</param>
        /// <returns>
        /// List of maps associated with the user, empty list if no maps found.
        /// </returns>
        public async Task<List<Map>> GetMapRecordsForUserAsync(string userId, bool includePublic)
        {
            return await Task.Run(() => this.GetMapRecordsForUser(userId, includePublic));
        }

        /// <summary>
        /// Gets a tile record asynchronously.
        /// </summary>
        /// <param name="tileId">The tile's unique id.</param>
        /// <returns>
        /// The tile if found, null otherwise.
        /// </returns>
        public async Task<Tile> GetTileRecordAsync(string tileId)
        {
            return await Task.Run(() => this.GetTileRecord(tileId));
        }

        /// <summary>
        /// Gets a list of unprocessed tiles records asynchronously.
        /// </summary>
        /// <returns>
        /// List of unprocessed tiles, empty list of no tiles require processing.
        /// </returns>
        public async Task<List<Tile>> GetUnprocessedTileRecordsAsync()
        {
            return await Task.Run(() => this.GetUnprocessedTileRecords());
        }

        /// <summary>
        /// Updates a map record asynchronously.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>
        /// True if the operation succeeds, false otherwise.
        /// </returns>
        public async Task<bool> UpdateMapRecordAsync(Map map)
        {
            try
            {
                // Initialize connection to Azure table storage
                var cloudStorageConnectionString = this.configuration.GetConnectionString("AzureTableStorage");
                var cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
                var cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());

                // Connect to the worldmapmaps table.
                var cloudTable = cloudTableClient.GetTableReference("worldmapmaps");

                // Create an insert operation
                var insertOperation = TableOperation.Merge(map);

                // Execute the operation
                var operationResult = await cloudTable.ExecuteAsync(insertOperation);

                return true;
            }
            catch (StorageException ex)
            {
                this.loggerService.Error("Unable to update map record: {0}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Updates a map tile record asynchronously.
        /// </summary>
        /// <param name="tile">The map tile.</param>
        /// <returns>
        /// True if the operation succeeds, false otherwise.
        /// </returns>
        public async Task<bool> UpdateTileRecordAsync(Tile tile)
        {
            try
            {
                // Initialize connection to Azure table storage
                var cloudStorageConnectionString = this.configuration.GetConnectionString("AzureTableStorage");
                var cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
                var cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());

                // Connect to the worldmapmaps table.
                var cloudTable = cloudTableClient.GetTableReference("worldmaptiles");

                // Create an insert operation
                var insertOperation = TableOperation.Merge(tile);

                // Execute the operation
                var operationResult = await cloudTable.ExecuteAsync(insertOperation);

                return true;
            }
            catch (StorageException ex)
            {
                this.loggerService.Error("Unable to update tile record: {0}", ex.Message);
                return false;
            }
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
            var table = tableClient.GetTableReference("worldmapmaps");

            // Create the table if required.
            if (await table.CreateIfNotExistsAsync())
            {
                this.loggerService.Debug("Created Table named: {0}", "worldmapmaps");
            }

            // Verify worldmaptiles table exists
            table = tableClient.GetTableReference("worldmaptiles");

            // Create the table if required.
            if (await table.CreateIfNotExistsAsync())
            {
                this.loggerService.Debug("Created Table named: {0}", "worldmaptiles");
            }

            return true;
        }

        /// <summary>
        /// Gets a map record and any associated tile records.
        /// </summary>
        /// <param name="mapId">The map's unique id.</param>
        /// <returns>
        /// The map and its tiles if found, null otherwise.
        /// </returns>
        private Map GetMapRecord(string mapId)
        {
            try
            {
                // Initialize connection to Azure table storage
                var cloudStorageConnectionString = this.configuration.GetConnectionString("AzureTableStorage");
                var cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
                var cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());

                // Connect to the worlmaptiles table.
                var cloudTable = cloudTableClient.GetTableReference("worldmapmaps");

                // Query the map record
                var mapQuery = from m in cloudTable.CreateQuery<Map>()
                               where m.MapId == mapId
                               select m;
                var mapList = mapQuery.ToList();
                if (mapList.Count == 0)
                {
                    return null;
                }

                var map = mapList.First();

                // Query the tile records
                cloudTable = cloudTableClient.GetTableReference("worldmaptiles");
                var tileQuery = from t in cloudTable.CreateQuery<Tile>()
                                where t.PartitionKey == map.RowKey
                                select t;
                map.Tiles = tileQuery.ToList();

                return map;
            }
            catch (StorageException ex)
            {
                this.loggerService.Error("Unable to retrieve map and tile records : {0}", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets all map records for the specified user.
        /// </summary>
        /// <param name="userId">The user's id.  Null if anonymous user.</param>
        /// <param name="includePublic">whether to include public maps in the results.</param>
        /// <returns>
        /// List of maps associated with the user, empty list if no maps found.
        /// </returns>
        private List<Map> GetMapRecordsForUser(string userId, bool includePublic)
        {
            try
            {
                // Initialize connection to Azure table storage
                var cloudStorageConnectionString = this.configuration.GetConnectionString("AzureTableStorage");
                var cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
                var cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());

                // Connect to the worlmaptiles table.
                var cloudTable = cloudTableClient.GetTableReference("worldmapmaps");

                // Query the map record
                var mapQuery = from m in cloudTable.CreateQuery<Map>()
                               select m;
                if (userId == null)
                {
                    mapQuery = mapQuery.Where(m => m.IsPublic == true);
                }
                else if (includePublic)
                {
                    mapQuery = mapQuery.Where(m => m.IsPublic == true || m.UserId == userId);
                }
                else
                {
                    mapQuery = mapQuery.Where(m => m.UserId == userId);
                }

                return mapQuery.ToList();
            }
            catch (StorageException ex)
            {
                this.loggerService.Error("Unable to retrieve maps for user: {0}", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets a tile record asynchronously.
        /// </summary>
        /// <param name="tileId">The tile's unique id.</param>
        /// <returns>
        /// The tile if found, null otherwise.
        /// </returns>
        private Tile GetTileRecord(string tileId)
        {
            try
            {
                // Initialize connection to Azure table storage
                var cloudStorageConnectionString = this.configuration.GetConnectionString("AzureTableStorage");
                var cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
                var cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());

                // Connect to the worlmaptiles table.
                var cloudTable = cloudTableClient.GetTableReference("worldmaptiles");

                // Query the tile record
                var tileQuery = from t in cloudTable.CreateQuery<Tile>()
                               where t.TileId == tileId
                               select t;
                var tileList = tileQuery.ToList();
                if (tileList.Count == 0)
                {
                    return null;
                }

                return tileList.First();
            }
            catch (StorageException ex)
            {
                this.loggerService.Error("Unable to retrieve tile records: {0}", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets a list of unprocessed tiles records asynchronously.
        /// </summary>
        /// <returns>
        /// List of unprocessed tiles, empty list of no tiles require processing.
        /// </returns>
        private List<Tile> GetUnprocessedTileRecords()
        {
            try
            {
                // Initialize connection to Azure table storage
                var cloudStorageConnectionString = this.configuration.GetConnectionString("AzureTableStorage");
                var cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
                var cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());

                // Connect to the worlmaptiles table.
                var cloudTable = cloudTableClient.GetTableReference("worldmaptiles");

                // Query the tile record
                var tileQuery = from t in cloudTable.CreateQuery<Tile>()
                                where t.IsRendered == false
                                select t;
                var tileList = tileQuery
                    .ToList()
                    .OrderBy(x => x.MapId)
                    .ThenBy(x => x.ZoomLevel)
                    .ToList();

                return tileList;
            }
            catch (StorageException ex)
            {
                this.loggerService.Error("Unable to retrieve tile records: {0}", ex.Message);
                return null;
            }
        }
    }
}
