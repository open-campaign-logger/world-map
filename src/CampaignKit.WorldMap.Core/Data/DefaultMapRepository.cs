﻿// <copyright file="DefaultMapRepository.cs" company="Jochen Linnemann - IT-Service">
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
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

using CampaignKit.WorldMap.Core.Entities;
using CampaignKit.WorldMap.Core.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace CampaignKit.WorldMap.Core.Data
{
    /// <summary>
    ///     Default implementation of the EntityFramework repository for Map data elements.
    /// </summary>
    /// <seealso cref="IMapRepository" />
    public class DefaultMapRepository : IMapRepository
    {
        private const int TilePixelSize = 250;

        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger _loggerService;

        /// <summary>
        ///     The user manager service.
        /// </summary>
        private readonly IUserManagerService _userManagerService;

        /// <summary>
        /// The BLOB storage service.
        /// </summary>
        private readonly IBlobStorageService _blobStorageService;

        /// <summary>
        /// The table storage service.
        /// </summary>
        private readonly ITableStorageService _tableStorageService;

        /// <summary>
        /// The queue storage service
        /// </summary>
        private readonly IQueueStorageService _queueStorageService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMapRepository" /> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="loggerService">The logger service.</param>
        /// <param name="tableStorageService">The table storage service.</param>
        /// <param name="userManagerService">The user manager service.</param>
        /// <param name="blobStorageService">The blob storage service.</param>
        /// <param name="queueStorageService">The queue storage service.</param>
        public DefaultMapRepository(
            IConfiguration configuration,
            ILogger<DefaultMapRepository> loggerService,
            ITableStorageService tableStorageService,
            IUserManagerService userManagerService,
            IBlobStorageService blobStorageService,
            IQueueStorageService queueStorageService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            _tableStorageService = tableStorageService ?? throw new ArgumentNullException(nameof(tableStorageService));
            _userManagerService = userManagerService ?? throw new ArgumentNullException(nameof(userManagerService));
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
            _queueStorageService = queueStorageService ?? throw new ArgumentNullException(nameof(queueStorageService));
        }

        /// <summary>
        /// Deletes the specified map and all child entities.
        ///     Ensures that the authenticated user is owner of the map
        ///     before any database operation is performed.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="user">The authenticated user.</param>
        /// <returns>
        ///     <c>true</c> if successful, <c>false</c> otherwise.
        /// </returns>
        public async Task<bool> Delete(string mapId, ClaimsPrincipal user)
        {
            // Ensure user is authenticated
            var userId = _userManagerService.GetUserId(user);
            if (userId == null)
            {
                _loggerService.LogError("Database operation prohibited for non-authenticated user");
                return false;
            }

            // Determine if this map exists
            var map = await _tableStorageService.GetMapRecordAsync(mapId);
            if (map == null)
            {
                _loggerService.LogError($"Map with id:{mapId} not found");
                return false;
            }

            // Remove the map from the context.
            await _tableStorageService.DeleteMapRecordAsync(map);

            // Delete map directory and files
            await _blobStorageService.DeleteFolderAsync($"map{map.MapId}");

            // Return result
            return true;
        }

        /// <summary>
        /// Find a map based on its identifier.
        /// If the map is private then the user must be owner of map or the
        /// correct secret must be provided.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="user">The authenticated user.</param>
        /// <param name="shareKey">Map secret to be used by friends of map author.</param>
        /// <returns>
        ///     <c>Map</c> if successful, <c>null</c> otherwise.
        /// </returns>
        public async Task<Map> Find(string mapId, ClaimsPrincipal user, string shareKey)
        {
            // Retrieve the map entry and any associated markers.
            var map = await _tableStorageService.GetMapRecordAsync(mapId);

            // Ensure map has been found
            if (map == null)
            {
                _loggerService.LogError($"Map with id:{mapId} not found");
                return null;
            }

            // If the map is not public ensure user has rights to it
            if (!map.IsPublic)
            {
                var userid = _userManagerService.GetUserId(user);
                if (!map.UserId.Equals(userid) && !map.ShareKey.Equals(shareKey))
                {
                    _loggerService.LogError($"User not authorized to access map with id:{mapId}.");
                    return null;
                }
            }

            return map;
        }

        /// <summary>
        ///     Finds all maps that the user is authorized to see.
        ///     Unauthenticated users will have access only to public maps.
        ///     Authenticated users will see their maps and, optionally, public maps.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <param name="includePublic">Specify whether public maps should also be returned.</param>
        /// <returns>IEnumerable&lt;Map&gt;.</returns>
        public async Task<IEnumerable<Map>> FindAll(ClaimsPrincipal user, bool includePublic)
        {
            var userId = _userManagerService.GetUserId(user);
            return await _tableStorageService.GetMapRecordsForUserAsync(userId, includePublic);
        }

        /// <summary>
        ///     Creates the specified map.  Requires an authenticated user.
        /// </summary>
        /// <param name="map">The map entity to create.</param>
        /// <param name="stream">Map image data stream.</param>
        /// <param name="user">The authenticated user.</param>
        /// <returns>
        ///     <c>id</c> if successful, <c>0</c> otherwise.
        /// </returns>
        public async Task<string> Create(Map map, Stream stream, ClaimsPrincipal user)
        {
            // **********************
            //   Precondition Tests
            // **********************
            // Image data not provided?
            if (stream == null)
            {
                return null;
            }

            // User must be authenticated
            var userid = _userManagerService.GetUserId(user);
            if (userid == null)
            {
                _loggerService.LogError("Database operation prohibited for non-authenticated user");
                return null;
            }

            // ************************************
            //  Create DB entity (Generate Map ID)
            // ************************************
            map.UserId = userid;
            await _tableStorageService.CreateMapRecordAsync(map);

            // **********************
            //   Create Map Folder
            // **********************
            var folderName = $"map{map.MapId}";

            // ****************************
            //   Save Original Image File
            // ****************************
            byte[] originalImageBlob, masterImageBlob;
            using (var ms = new MemoryStream())
            {
                // Save original file.
                stream.CopyTo(ms);
                originalImageBlob = ms.ToArray();
                await _blobStorageService.CreateBlobAsync(folderName, $"original-file{map.FileExtension}", originalImageBlob);
            }

            // ****************************
            //  Save PNG Master Image File
            // ****************************
            var masterImage = Image.Load(originalImageBlob);
            var width = masterImage.Width;
            var height = masterImage.Height;

            var largestSize = Math.Max(width, height);
            var maxZoomLevel = Math.Log((double)largestSize / TilePixelSize, 2);

            var adjustedMaxZoomLevel = (int)Math.Max(0, Math.Floor(maxZoomLevel));
            var adjustedLargestSize = (int)Math.Round(Math.Pow(2, adjustedMaxZoomLevel) * TilePixelSize);

            if (width != height || largestSize != adjustedLargestSize)
            {
                masterImage = masterImage.Clone(context => context.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Pad,
                    Position = AnchorPositionMode.Center,
                    Size = new Size(width = adjustedLargestSize, height = adjustedLargestSize),
                }));
            }

            using (var ms = new MemoryStream())
            {
                masterImage.Save(ms, new PngEncoder());
                masterImageBlob = ms.ToArray();
                await _blobStorageService.CreateBlobAsync(folderName, "master-file.png", masterImageBlob);
            }

            // ****************************
            //      Update Map Entity
            // ****************************
            // Save png master file.
            map.MaxZoomLevel = adjustedMaxZoomLevel;
            map.AdjustedSize = adjustedLargestSize;
            map.WorldFolderPath = $"{_configuration.GetValue<string>("AzureBlobBaseURL")}/map{map.MapId}";
            map.ThumbnailPath = $"{map.WorldFolderPath}/0_zoom-level.png";
            await _tableStorageService.UpdateMapRecordAsync(map);

            // ****************************************
            //        Create Tile Entities
            // ****************************************

            // Iterate through zoom levels to create required tiles
            for (var zoomLevel = 0; zoomLevel <= map.MaxZoomLevel; zoomLevel++)
            {
                // Calculate the number of tiles required for this zoom level
                var numberOfTilesPerDimension = (int)Math.Pow(2, zoomLevel);

                for (var x = 0; x < numberOfTilesPerDimension; x++)
                {
                    for (var y = 0; y < numberOfTilesPerDimension; y++)
                    {
                        var tile = new Tile
                        {
                            MapId = map.MapId,
                            ZoomLevel = zoomLevel,
                            IsRendered = false,
                            TileSize = TilePixelSize,
                            X = x,
                            Y = y,
                        };
                        await _tableStorageService.CreateTileRecordAsync(tile);
                    }
                }
            }

            await _queueStorageService.QueueMapForProcessing(map);
            return map.MapId;
        }

        /// <summary>
        ///     Saves changes to the specified map.  Only map owner may save changes.
        /// </summary>
        /// <param name="map">The map entity to save.</param>
        /// <param name="user">The authenticated user.</param>
        /// <returns>
        ///     <c>id</c> if successful, <c>false</c> otherwise.
        /// </returns>
        public async Task<bool> Save(Map map, ClaimsPrincipal user)
        {
            // Ensure user is authenticated
            var userid = _userManagerService.GetUserId(user);
            if (userid == null)
            {
                _loggerService.LogError("Database operation prohibited for non-authenticated user");
                return false;
            }

            // Determine if the user has rights to delete the map
            if (!map.UserId.Equals(userid))
            {
                _loggerService.LogError($"User {userid} does not have rights to delete map with id:{map.MapId}.");
                return false;
            }

            await _tableStorageService.UpdateMapRecordAsync(map);
            return true;
        }

        /// <summary>
        ///     Determines if user owns the map.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="user">The authenticated user.</param>
        /// <returns>
        ///     <c>id</c> if successful, <c>false</c> otherwise.
        /// </returns>
        public async Task<bool> CanEdit(string mapId, ClaimsPrincipal user)
        {
            // Ensure user is authenticated
            var userid = _userManagerService.GetUserId(user);
            if (userid == null)
            {
                _loggerService.LogError("Database operation prohibited for non-authenticated user");
                return false;
            }

            // Retrieve the map entry and any associated markers.
            var map = await _tableStorageService.GetMapRecordAsync(mapId);

            // Ensure map has been found
            if (map == null)
            {
                _loggerService.LogError($"Map with id:{mapId} not found");
                return false;
            }

            // Determine if the user has rights to delete the map
            if (!map.UserId.Equals(userid))
            {
                _loggerService.LogError($"User {userid} does not have rights to delete map with id:{mapId}.");
                return false;
            }

            return true;
        }

        /// <summary>Determines if user has rights to view the map.</summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="user">The authenticated user.</param>
        /// <param name="shareKey">The map's secret key.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public async Task<bool> CanView(string mapId, ClaimsPrincipal user, string shareKey)
        {
            // Retrieve the map entry and any associated markers.
            var map = await _tableStorageService.GetMapRecordAsync(mapId);

            // Ensure map has been found
            if (map == null)
            {
                _loggerService.LogError($"Map with id:{mapId} not found");
                return false;
            }

            // Determine if this is a public map
            if (map.IsPublic)
            {
                return true;
            }

            // Determine if provided secret matches
            var isSecretProvided = !string.IsNullOrEmpty(shareKey);
            if (isSecretProvided && map.ShareKey.Equals(shareKey))
            {
                return true;
            }

            // Determine if user is authenticated
            var userid = _userManagerService.GetUserId(user);

            // Determine if the user has rights to delete the map
            if (!map.UserId.Equals(userid))
            {
                _loggerService.LogError($"User {userid} does not have rights to delete map with id:{mapId}.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Initializes the repository if required.
        /// </summary>
        /// <param name="mapImagePath">Path to sample map image file.</param>
        /// <param name="mapDataPath">Path to sample map json file.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public async Task<bool> InitRepository(string mapImagePath, string mapDataPath)
        {
            //// Ensure sample map has been created
            var sampleMap = await _tableStorageService.GetMapRecordAsync("sample");

            if (sampleMap == null)
            {
                var sampleImage = File.ReadAllBytes(mapImagePath);
                var sampleJSON = File.ReadAllText(mapDataPath);
                var map = new Map()
                {
                    Copyright = string.Empty,
                    MarkerData = sampleJSON,
                    IsPublic = true,
                    Name = "Sample",
                    RepeatMapInX = false,
                    ShareKey = "lNtqjEVQ",
                    MapId = "sample",
                };
                using (var ms = new MemoryStream(sampleImage))
                {
                    await Create(map, ms, _userManagerService.GetSystemUser());
                }
            }

            return true;
        }
    }
}