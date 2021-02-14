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

namespace CampaignKit.WorldMap.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using CampaignKit.WorldMap.Entities;
    using CampaignKit.WorldMap.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;

    /// <summary>
    ///     Default implementation of the EntityFramework repository for Map data elements.
    /// </summary>
    /// <seealso cref="IMapRepository" />
    public class DefaultMapRepository : IMapRepository
    {
        private const int TilePixelSize = 250;

        /// <summary>
        ///     The database context.
        /// </summary>
        private readonly WorldMapDBContext dbContext;

        /// <summary>
        ///     The file path service.
        /// </summary>
        private readonly IFilePathService filePathService;

        /// <summary>
        ///     The application logging service.
        /// </summary>
        private readonly ILogger loggerService;

        /// <summary>
        ///     The user manager service.
        /// </summary>
        private readonly IUserManagerService userManagerService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMapRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="filePathService">The file path service.</param>
        /// <param name="loggerService">The logger service.</param>
        /// <param name="userManagerService">The user manager service.</param>
        public DefaultMapRepository(
            WorldMapDBContext dbContext,
            IFilePathService filePathService,
            ILogger<DefaultMapRepository> loggerService,
            IUserManagerService userManagerService)
        {
            this.dbContext = dbContext;
            this.filePathService = filePathService;
            this.loggerService = loggerService;
            this.userManagerService = userManagerService;
        }

        /// <summary>
        ///     Deletes the specified map and all child entities.
        ///     Ensures that the authenticated user is owner of the map
        ///     before any database operation is performed.
        /// </summary>
        /// <param name="id">The map identifier.</param>
        /// <param name="user">The authenticated user.</param>
        /// <returns>
        ///     <c>true</c> if successful, <c>false</c> otherwise.
        /// </returns>
        public async Task<bool> Delete(int id, ClaimsPrincipal user)
        {
            // Ensure user is authenticated
            var userid = this.userManagerService.GetUserId(user);
            if (userid == null)
            {
                this.loggerService.LogError("Database operation prohibited for non-authenticated user");
                return false;
            }

            // Determine if this map exists
            var map = await this.dbContext.Maps.FindAsync(id);
            if (map == null)
            {
                this.loggerService.LogError($"Map with id:{id} not found");
                return false;
            }

            // Determine if the user has rights to delete the map
            if (!map.UserId.Equals(userid))
            {
                this.loggerService.LogError($"User {userid} does not have rights to delete map with id:{id}.");
                return false;
            }

            // Remove the map from the context.
            this.dbContext.Maps.Remove(map);
            await this.dbContext.SaveChangesAsync();

            // Delete map directory and files
            if (Directory.Exists(map.WorldFolderPath))
            {
                Directory.Delete(map.WorldFolderPath, true);
            }

            // Return result
            return true;
        }

        /// <summary>
        ///     Find a map based on its identifier.
        ///     If the map is private then the user must be owner of map or the
        ///     correct secret must be provided.
        /// </summary>
        /// <param name="id">The map identifier.</param>
        /// <param name="user">The authenticated user.</param>
        /// <param name="shareKey">Map secret to be used by friends of map author.</param>
        /// <returns>
        ///     <c>Map</c> if successful, <c>null</c> otherwise.
        /// </returns>
        public async Task<Map> Find(int id, ClaimsPrincipal user, string shareKey)
        {
            // Retrieve the map entry and any associated markers.
            var map = await this.dbContext.Maps
                .FirstOrDefaultAsync(m => m.MapId == id);

            // Ensure map has been found
            if (map == null)
            {
                this.loggerService.LogError($"Map with id:{id} not found");
                return null;
            }

            // If the map is not public ensure user has rights to it
            if (!map.IsPublic)
            {
                var userid = this.userManagerService.GetUserId(user);
                if (!map.UserId.Equals(userid) && !map.ShareKey.Equals(shareKey))
                {
                    this.loggerService.LogError($"User not authorized to access map with id:{id}.");
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
            var userid = this.userManagerService.GetUserId(user);

            // Return public and owned maps to authenticated users
            if (userid != null)
            {
                if (includePublic)
                {
                    return await this.dbContext.Maps
                        .Where(m => m.IsPublic || m.UserId.Equals(userid))
                        .ToListAsync();
                }

                return await this.dbContext.Maps
                    .Where(m => m.UserId.Equals(userid))
                    .ToListAsync();
            }

            return await this.dbContext.Maps
                .Where(m => m.IsPublic)
                .ToListAsync();
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
        public async Task<int> Create(Map map, Stream stream, ClaimsPrincipal user)
        {
            // **********************
            //   Precondition Tests
            // **********************
            // Image data not provided?
            if (stream == null)
            {
                return 0;
            }

            // User must be authenticated
            var userid = this.userManagerService.GetUserId(user);
            if (userid == null)
            {
                this.loggerService.LogError("Database operation prohibited for non-authenticated user");
                return 0;
            }

            // ************************************
            //  Create DB entity (Generate Map ID)
            // ************************************
            map.CreationTimestamp = DateTime.UtcNow;
            map.UpdateTimestamp = map.CreationTimestamp;
            map.UserId = userid;
            this.dbContext.Add(map);
            await this.dbContext.SaveChangesAsync();

            // **********************
            //   Create Map Folder
            // **********************
            var worldFolderPath = Path.Combine(this.filePathService.PhysicalWorldBasePath, $"{map.MapId}");
            if (Directory.Exists(worldFolderPath))
            {
                Directory.Delete(worldFolderPath, true);
            }

            Directory.CreateDirectory(worldFolderPath);

            // ****************************
            //   Save Original Image File
            // ****************************
            var originalFilePath = Path.Combine(worldFolderPath, $"original-file{map.FileExtension}");
            using (stream)
            using (var originalFileStream = new FileStream(originalFilePath, FileMode.CreateNew))
            {
                stream.CopyTo(originalFileStream);
            }

            // ************************************
            //      Create Master Image File
            // ************************************
            // Create master image file
            var masterFilePath = Path.Combine(worldFolderPath, "master-file.png");
            using (var originalFileStream = new FileStream(originalFilePath, FileMode.Open))
            using (var masterFileStream = new FileStream(masterFilePath, FileMode.CreateNew))
            {
                var masterImage = Image.Load(Configuration.Default, originalFileStream);
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

                masterImage.SaveAsPng(masterFileStream);

                map.MaxZoomLevel = adjustedMaxZoomLevel;
                map.AdjustedSize = adjustedLargestSize;

                map.ThumbnailPath = $"{this.filePathService.VirtualWorldBasePath}/{map.MapId}/0/zoom-level.png";
            }

            // ****************************************
            //        Create Tile Image Files
            // ****************************************

            // Calculate number of zoom levels and steps required
            map.Tiles = new List<Tile>();

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
                            CreationTimestamp = DateTime.UtcNow,
                            TileSize = TilePixelSize,
                            X = x,
                            Y = y,
                        };
                        map.Tiles.Add(tile);
                    }
                }
            }

            // ************************************
            //   Update Map Entity
            // ************************************
            map.WorldFolderPath = worldFolderPath;
            this.dbContext.Update(map);
            await this.dbContext.SaveChangesAsync();

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
            var userid = this.userManagerService.GetUserId(user);
            if (userid == null)
            {
                this.loggerService.LogError("Database operation prohibited for non-authenticated user");
                return false;
            }

            // Determine if the user has rights to delete the map
            if (!map.UserId.Equals(userid))
            {
                this.loggerService.LogError($"User {userid} does not have rights to delete map with id:{map.MapId}.");
                return false;
            }

            this.dbContext.Update(map);
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        ///     Determines if user owns the map.
        /// </summary>
        /// <param name="id">The map identifier.</param>
        /// <param name="user">The authenticated user.</param>
        /// <returns>
        ///     <c>id</c> if successful, <c>false</c> otherwise.
        /// </returns>
        public async Task<bool> CanEdit(int id, ClaimsPrincipal user)
        {
            // Ensure user is authenticated
            var userid = this.userManagerService.GetUserId(user);
            if (userid == null)
            {
                this.loggerService.LogError("Database operation prohibited for non-authenticated user");
                return false;
            }

            // Retrieve the map entry and any associated markers.
            var map = await this.dbContext.Maps
                .FirstOrDefaultAsync(m => m.MapId == id);

            // Ensure map has been found
            if (map == null)
            {
                this.loggerService.LogError($"Map with id:{id} not found");
                return false;
            }

            // Determine if the user has rights to delete the map
            if (!map.UserId.Equals(userid))
            {
                this.loggerService.LogError($"User {userid} does not have rights to delete map with id:{id}.");
                return false;
            }

            return true;
        }

        /// <summary>Determines if user has rights to view the map.</summary>
        /// <param name="id">The map identifier.</param>
        /// <param name="user">The authenticated user.</param>
        /// <param name="shareKey">The map's secret key.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public async Task<bool> CanView(int id, ClaimsPrincipal user, string shareKey)
        {
            // Retrieve the map entry and any associated markers.
            var map = await this.dbContext.Maps
                .FirstOrDefaultAsync(m => m.MapId == id);

            // Ensure map has been found
            if (map == null)
            {
                this.loggerService.LogError($"Map with id:{id} not found");
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
            var userid = this.userManagerService.GetUserId(user);

            // Determine if the user has rights to delete the map
            if (!map.UserId.Equals(userid))
            {
                this.loggerService.LogError($"User {userid} does not have rights to delete map with id:{id}.");
                return false;
            }

            return true;
        }
    }
}