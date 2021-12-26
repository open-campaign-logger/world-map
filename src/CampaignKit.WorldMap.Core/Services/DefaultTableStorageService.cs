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

namespace CampaignKit.WorldMap.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CampaignKit.WorldMap.Core.Entities;

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
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTableStorageService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerService">The logger service.</param>
        public DefaultTableStorageService(IConfiguration configuration, ILogger<DefaultTableStorageService> loggerService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
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
                var cloudStorageConnectionString = _configuration.GetConnectionString("AzureTableStorageMaps");
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
                _loggerService.LogError("Unable to create map record: {0}", ex.Message);
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
                // Connect to the worlmapmaps table.
                var cloudStorageConnectionString = _configuration.GetConnectionString("AzureTableStorageMaps");
                var cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
                var cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
                var cloudTable = cloudTableClient.GetTableReference("worldmapmaps");

                // Delete the map record.
                var mapOperation = TableOperation.Delete(map);
                await cloudTable.ExecuteAsync(mapOperation);

                return true;
            }
            catch (StorageException ex)
            {
                _loggerService.LogError("Unable to delete map and tile records: {0}", ex.Message);
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
            return Task.Run(() => GetMapRecord(mapId));
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
            return await Task.Run(() => GetMapRecordsForUser(userId, includePublic));
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
                var cloudStorageConnectionString = _configuration.GetConnectionString("AzureTableStorageMaps");
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
                _loggerService.LogError("Unable to update map record: {0}", ex.Message);
                return false;
            }
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
                var cloudStorageConnectionString = _configuration.GetConnectionString("AzureTableStorageMaps");
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

                return map;
            }
            catch (StorageException ex)
            {
                _loggerService.LogError("Unable to retrieve map and tile records : {0}", ex.Message);
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
                var cloudStorageConnectionString = _configuration.GetConnectionString("AzureTableStorageMaps");
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
                _loggerService.LogError("Unable to retrieve maps for user: {0}", ex.Message);
                return null;
            }
        }

    }
}
