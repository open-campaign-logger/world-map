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
    using System.Threading.Tasks;

    using CampaignKit.WorldMap.Core.Entities;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;

    /// <summary>
    /// Default tile processing service.
    /// </summary>
    public class DefaultTileProcessingService : ITileProcessingService
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
        ///     Initializes a new instance of the <see cref="DefaultTileProcessingService" /> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="blobStorageService">The blob storage service.</param>
        /// <param name="tableStorageService">The table storage service.</param>
        /// <param name="loggerService">The logger service.</param>
        public DefaultTileProcessingService(IConfiguration configuration, IServiceProvider serviceProvider, IBlobStorageService blobStorageService, ITableStorageService tableStorageService, ILogger<DefaultTileProcessingService> loggerService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
            _tableStorageService = tableStorageService ?? throw new ArgumentNullException(nameof(tableStorageService));
        }

        /// <summary>
        /// Creates a tile for a map.
        /// </summary>
        /// <param name="tileId">The id of the tile to create.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public async Task<bool> ProcessTile(string tileId)
        {

            // Retrieve the unprocessed tile from the database
            _loggerService.LogDebug("Retrieving tile: {0}", tileId);
            var tile = await _tableStorageService.GetTileRecordAsync(tileId);
            if (tile == null)
            {
                _loggerService.LogError("Unable to retrieve tile: {0}", tileId);
                return false;
            }

            // Calculate Folder Paths for the Map
            var mapFolderName = $"map{tile.MapId}";
            var masterImageName = "master-file.png";
            _loggerService.LogDebug("Loading master file: {0}/{1}", mapFolderName, masterImageName);
            var masterImage = await _blobStorageService.ReadBlobAsync(mapFolderName, masterImageName);

            // Create zoom level base file (if required)
            var zoomLevelBaseImageName = $"{tile.ZoomLevel}_zoom-level.png";
            byte[] zoomLevelBaseImage;
            _loggerService.LogDebug("Determining if zoom level base image exists: {0}/{1}.", mapFolderName, zoomLevelBaseImageName);
            var zoomLevelBaseImageExists = await _blobStorageService.BlobExistsAsync(mapFolderName, zoomLevelBaseImageName);
            if (!zoomLevelBaseImageExists)
            {
                _loggerService.LogDebug("Creating zoom level base image: {0}/{1}.", mapFolderName, zoomLevelBaseImageName);
                var tilePixelSize = tile.TileSize;
                var numberOfTilesPerDimension = (int)Math.Pow(2, tile.ZoomLevel);
                zoomLevelBaseImage = await CreateZoomLevelBaseImage(
                        numberOfTilesPerDimension,
                        masterImage,
                        tile.ZoomLevel,
                        tilePixelSize,
                        mapFolderName,
                        zoomLevelBaseImageName);
            } else
            {
                zoomLevelBaseImage = await _blobStorageService.ReadBlobAsync(mapFolderName, zoomLevelBaseImageName);
            }

            // Create zoom level tile
            var tileImageName = $"{tile.ZoomLevel}_{tile.X}_{tile.Y}.png";
            _loggerService.LogDebug("Creating tile: {0}/{1}.", mapFolderName, tileImageName);
            await CreateTileImage(zoomLevelBaseImage, tile, mapFolderName, tileImageName);

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
        private async Task<byte[]> CreateZoomLevelBaseImage(int numberOfTilesPerDimension, byte[] masterBlob, int zoomLevel, int tilePixelSize, string folderName, string blobName)
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
        private async Task<bool> CreateTileImage(byte[] zoomLevelBlob, Tile tile, string folderName, string blobName)
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