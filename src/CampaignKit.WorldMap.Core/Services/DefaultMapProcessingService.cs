// <copyright file="DefaultMapProcessingService.cs" company="Jochen Linnemann - IT-Service">
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
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using CampaignKit.WorldMap.Core.Entities;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;

    /// <summary>
    ///     A timed background service that queries the Tiles table and processes
    ///     tiles that haven't been created yet.
    ///     This article was used to model this timed background service.
    ///     https://thinkrethink.net/2018/02/21/asp-net-core-background-processing/.
    /// </summary>
    public class DefaultMapProcessingService : IMapProcessingService
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
        /// The service provider.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// The BLOB storage service.
        /// </summary>
        private readonly IBlobStorageService _blobStorageService;

        /// <summary>
        /// The table storage service.
        /// </summary>
        private readonly ITableStorageService _tableStorageService;

        /// <summary>
        ///     The executing task.
        /// </summary>
        private Task executingTask;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMapProcessingService" /> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="blobStorageService">The blob storage service.</param>
        /// <param name="tableStorageService">The table storage service.</param>
        /// <param name="loggerService">The logger service.</param>
        public DefaultMapProcessingService(IConfiguration configuration, IServiceProvider serviceProvider, IBlobStorageService blobStorageService, ITableStorageService tableStorageService, ILogger<DefaultMapProcessingService> loggerService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
            _tableStorageService = tableStorageService ?? throw new ArgumentNullException(nameof(tableStorageService));
        }

        /// <summary>
        /// Creates tiles for the map.
        /// </summary>
        /// <param name="mapId">The id of the map to create tiles for.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public async Task<bool> ProcessMap(string mapId)
        {
            // Query the tiles table to see if there are any unprocessed tiles.
            var tiles = await _tableStorageService.GetUnprocessedTileRecordsAsync();

            if (tiles.Count > 0)
            {
                _loggerService.LogDebug("{0} tiles waiting to be processed.", tiles.Count);

                // Process each map with unprocessed tiles
                var mapList = tiles.Select(o => o.MapId).Distinct();
                foreach (var map in mapList)
                {
                    _loggerService.LogDebug("Processing tiles for map: {0}.", map);

                    // Calculate Folder Paths for the Map
                    var folderName = $"map{map}";
                    var masterBlob = await _blobStorageService.ReadBlobAsync(folderName, "master-file.png");

                    // Process each map zoom level with unprocessed tiles
                    var zoomList = tiles.Where(t => t.MapId == map).Select(o => o.ZoomLevel).Distinct();
                    foreach (var zoomLevel in zoomList)
                    {
                        // Select Tiles for Processing
                        var tilesToProcess = tiles.Where(t => t.MapId == map).Where(t => t.ZoomLevel == zoomLevel);

                        // Calculate Tile Size
                        var tilePixelSize = tiles[0].TileSize;

                        // Calculate the number of tiles required for this zoom level
                        var numberOfTilesPerDimension = (int)Math.Pow(2, zoomLevel);

                        // Create zoom level base file (sync)
                        _loggerService.LogDebug("Creating zoom level base image for zoom level: {0}.", zoomLevel);
                        var zoomLevelBlob
                            = await CreateZoomLevelBaseFile(
                                numberOfTilesPerDimension,
                                masterBlob,
                                zoomLevel,
                                tilePixelSize,
                                folderName,
                                $"{zoomLevel}_zoom-level.png");

                        // Create collection for tile creation tasks
                        var tasks = new List<Task<bool>>();
                        tasks.Clear();

                        // Cycle through the tiles selected for the current map and zoom and process them
                        foreach (var tile in tilesToProcess)
                        {
                            var blobName = $"{zoomLevel}_{tile.X}_{tile.Y}.png";
                            _loggerService.LogDebug("Creating zoom level tile: {0}.", blobName);
                            tasks.Add(Task.Run(() => CreateZoomLevelTileFile(zoomLevelBlob, tile, folderName, blobName)));
                        }

                        // Wait for all tile creation tasks to complete
                        var results = await Task.WhenAll(tasks);
                    }
                }

                // Delete all processed tile records
                foreach (var tile in tiles)
                {
                    await _tableStorageService.DeleteTileRecordAsync(tile);
                }
            }

            return true;
        }

        /// <summary>
        /// Creates the zoom level base file.
        /// </summary>
        /// <param name="numberOfTilesPerDimension">The number of tiles per dimension.</param>
        /// <param name="masterBlob">The master image.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        /// <param name="tilePixelSize">Tile pixel size.</param>
        /// <param name="folderName">The blob container name.</param>
        /// <param name="blobName">The name of the blob.</param>
        /// <returns>byte[] of zoom level base file.</returns>
        private async Task<byte[]> CreateZoomLevelBaseFile(int numberOfTilesPerDimension, byte[] masterBlob, int zoomLevel, int tilePixelSize, string folderName, string blobName)
        {
            using (var masterBaseImage = Image.Load(masterBlob))
            {
                var size = numberOfTilesPerDimension * tilePixelSize;

                masterBaseImage.Mutate(context => context.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Pad,
                    Position = AnchorPositionMode.Center,
                    Size = new Size(size, size),
                }));

                using (var ms = new MemoryStream())
                {
                    await masterBaseImage.SaveAsPngAsync(ms);
                    var blob = ms.ToArray();
                    await _blobStorageService.CreateBlobAsync(folderName, blobName, blob);
                    return blob;
                }
            }
        }

        /// <summary>
        /// Creates the zoom level tile file.
        /// </summary>
        /// <param name="zoomLevelBlob">The zoome level image.</param>
        /// <param name="tile">The tile to be created.</param>
        /// <param name="folderName">The blob container name.</param>
        /// <param name="blobName">The name of the blob.</param>
        /// <returns>True when complete.</returns>
        private async Task<bool> CreateZoomLevelTileFile(byte[] zoomLevelBlob, Tile tile, string folderName, string blobName)
        {
            using (var zoomLevelImage = Image.Load(zoomLevelBlob))
            {
                zoomLevelImage.Mutate(context => context.Crop(
                new Rectangle(tile.X * tile.TileSize, tile.Y * tile.TileSize, tile.TileSize, tile.TileSize)));
                using (var ms = new MemoryStream())
                {
                    await zoomLevelImage.SaveAsPngAsync(ms);
                    await _blobStorageService.CreateBlobAsync(folderName, blobName, ms.ToArray());
                }

                tile.IsRendered = true;
                await _tableStorageService.UpdateTileRecordAsync(tile);
                return true;
            }
        }
    }
}