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
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;

    /// <summary>
    /// Default tile processing service.
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
        /// The BLOB storage service.
        /// </summary>
        private readonly IBlobStorageService _blobStorageService;

        /// <summary>
        /// The table storage service.
        /// </summary>
        private readonly ITableStorageService _tableStorageService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMapProcessingService" /> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="blobStorageService">The blob storage service.</param>
        /// <param name="tableStorageService">The table storage service.</param>
        /// <param name="loggerService">The logger service.</param>
        public DefaultMapProcessingService(IConfiguration configuration, IBlobStorageService blobStorageService, ITableStorageService tableStorageService, ILogger<DefaultMapProcessingService> loggerService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
            _tableStorageService = tableStorageService ?? throw new ArgumentNullException(nameof(tableStorageService));
        }

        /// <summary>
        /// Process a master image.
        /// </summary>
        /// <param name="mapId">The id of the map to process.</param>
        /// <returns>
        /// True if successful, false otherwise.
        /// </returns>
        public async Task<bool> ProcessMasterImage(string mapId)
        {

            // Retrieve the map record from the database
            var map = await _tableStorageService.GetMapRecordAsync(mapId);
            if (map == null)
            {
                _loggerService.LogError("Unable to retrieve map: {0}", mapId);
                return false;
            }

            // Retrieve the master image and create zoom level images
            var mapFolderName = $"map{map.MapId}";
            var masterImageName = "master-file.png";
            using (var masterImage = Image.Load(await _blobStorageService.ReadBlobAsync(mapFolderName, masterImageName)))
            {
                for (int zoomLevel = 0; zoomLevel <= map.MaxZoomLevel; zoomLevel++)
                {
                    var zoomLevelBaseImageName = $"{zoomLevel}_zoom-level.png";
                    var tilePixelSize = this._configuration.GetValue<int>("TilePixelSize");
                    int numberOfTilesPerDimension = (int)Math.Pow(2, zoomLevel);
                    var size = numberOfTilesPerDimension * tilePixelSize;

                    // Mutate a deep clone of the original image
                    using var imageCopy = masterImage.Clone(context => context.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Pad,
                        Position = AnchorPositionMode.Center,
                        Size = new Size(size, size),
                    }));
                    using var ms = new MemoryStream();
                    await imageCopy.SaveAsPngAsync(ms);
                    var blob = ms.ToArray();
                    await _blobStorageService.CreateBlobAsync(mapFolderName, zoomLevelBaseImageName, blob);
                }
            }

            return true;
        }

        /// <summary>
        /// Process a zoom level image.
        /// </summary>
        /// <param name="tileId">The id of the tile to process.</param>
        /// <param name="zoomLevel">The zoom level to process.</param>
        /// <returns>
        /// True if successful, false otherwise.
        /// </returns>
        public async Task<bool> ProcessZoomLevelImage(string mapId, int zoomLevel)
        {
            // Retrieve the map record from the database.
            var map = await _tableStorageService.GetMapRecordAsync(mapId);
            if (map == null)
            {
                _loggerService.LogError("Unable to retrieve map: {0}", mapId);
                return false;
            }

            // Retrieve the zoom level base image.
            var mapFolderName = $"map{mapId}";
            var zoomLevelBaseImageName = $"{zoomLevel}_zoom-level.png";
            var tilePixelSize = this._configuration.GetValue<int>("TilePixelSize");
            using var zoomLevelBaseImage = Image.Load(await _blobStorageService.ReadBlobAsync(mapFolderName, zoomLevelBaseImageName));

            // Create zoom level tile files
            var numberOfTilesPerDimension = (int)Math.Pow(2, zoomLevel);
            for (var x = 0; x < numberOfTilesPerDimension; x++)
            {
                for (var y = 0; y < numberOfTilesPerDimension; y++)
                {
                    var tileImageName = $"{zoomLevel}_{x}_{y}.png";

                    // Mutate a deep clone of the original image
                    using var imageCopy = zoomLevelBaseImage.Clone(context => context.Crop(                    
                    new Rectangle(x * tilePixelSize, y * tilePixelSize, tilePixelSize, tilePixelSize)));
                    using var ms = new MemoryStream();
                    await imageCopy.SaveAsPngAsync(ms);
                    await _blobStorageService.CreateBlobAsync(mapFolderName, tileImageName, ms.ToArray());
                }
            }

            return true;
        }
    }
}