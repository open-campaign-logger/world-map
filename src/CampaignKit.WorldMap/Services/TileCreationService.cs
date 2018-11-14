// Copyright 2017-2018 Jochen Linnemann
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using CampaignKit.WorldMap.Entities;

using Microsoft.AspNetCore.Mvc.Routing;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using SixLabors.ImageSharp.Formats;

namespace CampaignKit.WorldMap.Services
{
    /// <summary>
    ///     Interface ITileCreationService
    /// </summary>
    public interface ITileCreationService
    {
        #region Public Methods

        /// <summary>
        ///     Creates the tiles asynchronous.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> CreateTilesAsync(Guid mapId, Stream stream);

        /// <summary>
        ///     Removes the tiles.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        void RemoveTiles(Guid mapId);

        #endregion Public Methods
    }

    /// <inheritdoc />
    /// <summary>
    ///     Class DefaultTileCreationService.
    /// </summary>
    public class DefaultTileCreationService : ITileCreationService
    {
        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultTileCreationService" /> class.
        /// </summary>
        /// <param name="worldBasePathService">The world path service.</param>
        /// <param name="mapDataService">The map data service.</param>
        /// <param name="progressService">The progress service.</param>
        /// <param name="urlHelperFactory">The URL helper factory.</param>
        public DefaultTileCreationService(
            IWorldBasePathService worldBasePathService, IMapDataService mapDataService,
            IProgressService progressService, IUrlHelperFactory urlHelperFactory)
        {
            _physicalWorldBasePath = worldBasePathService.PhysicalWorldBasePath;
            _mapDataService = mapDataService;
            _progressService = progressService;
            _urlHelperFactory = urlHelperFactory;
            _virtualWorldBasePath = worldBasePathService.VirtualWorldBasePath;
        }

        #endregion Public Constructors

        #region Private Fields

        private const int TilePixelSize = 250;
        private readonly IMapDataService _mapDataService;
        private readonly string _physicalWorldBasePath;
        private readonly IProgressService _progressService;

        // ReSharper disable once NotAccessedField.Local
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly string _virtualWorldBasePath;

        #endregion Private Fields

        #region Public Methods

        /// <inheritdoc />
        /// <summary>
        ///     create tiles as an asynchronous operation.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> CreateTilesAsync(Guid mapId, Stream stream)
        {
            // ****************************************
            //        Precondition Tests
            // ****************************************
            // Image data not provided?
            if (stream == null) return false;

            // Map id doesn't exist?
            var map = _mapDataService.Find($"{mapId}");
            if (map == null) return false;

            // Map directory already exists?
            var worldFolderPath = Path.Combine(_physicalWorldBasePath, $"{mapId}");
            if (Directory.Exists(worldFolderPath)) return false;

            // ****************************************
            //  Create Map Folder and Base Image File
            // ****************************************
            // Create world folder path
            Directory.CreateDirectory(worldFolderPath);

            // Create image file from provided image data
            var originalFilePath = Path.Combine(worldFolderPath, $"original-file{map.FileExtension}");
            using (stream)
            using (var originalFileStream = new FileStream(originalFilePath, FileMode.CreateNew))
            {
                stream.CopyTo(originalFileStream);
            }

            // ****************************************
            //        Create Tile Image Files
            // ****************************************
            // Setup progress indicator
            var progressIndicator = _progressService.CreateIndicator($"{mapId}");
            progressIndicator.SetProgress(0.0);

            // Create master image file (sync)
            var masterFilePath = Path.Combine(worldFolderPath, "master-file.png");
            CreateMasterFileAndUpdateMapData(map, originalFilePath, masterFilePath);

            // Calculate number of zoom levels and steps required
            var stepCounter = 1;
            var totalNumberOfSteps = 1; /* preparing the master file */
            totalNumberOfSteps += map.MaxZoomLevel + 1; /* preparing one zoom-level file per zoom level */
            totalNumberOfSteps += Sum(0, map.MaxZoomLevel, /* preparing the tiles of each zoom level */
                zoomLevel => (int) Math.Pow((int) Math.Pow(2, zoomLevel), 2));

            // Set the number of steps in the progress indicator
            var progressUpdater = new Action(() =>
                progressIndicator.SetProgress(++stepCounter / (double) totalNumberOfSteps));

            // Create collection for tile creation tasks
            List<Task<bool>> tasks = new List<Task<bool>>();

            // Iterate through zoom levels to create required tiles
            for (var zoomLevel = 0; zoomLevel <= map.MaxZoomLevel; zoomLevel++)
            {
                // Calculate the number of tiles required for this zoom level
                var numberOfTilesPerDimension = (int) Math.Pow(2, zoomLevel);

                // Create zoom level directory
                var zoomLevelFolderPath = Path.Combine(worldFolderPath, $"{zoomLevel}");
                Directory.CreateDirectory(zoomLevelFolderPath);

                // Create zoom level base file (sync)
                var zoomLevelBaseFilePath = Path.Combine(zoomLevelFolderPath, "zoom-level.png");
                var zoomLevelBaseImage = CreateZoomLevelBaseFile(numberOfTilesPerDimension, masterFilePath, zoomLevelBaseFilePath);
                progressUpdater.Invoke();

                // Clear the task collection
                tasks.Clear();

                for (var x = 0; x < numberOfTilesPerDimension; x++)
                {
                    for (var y = 0; y < numberOfTilesPerDimension; y++)
                    {
                        var zoomLevelTileFilePath = Path.Combine(zoomLevelFolderPath, $"{x}_{y}.png");
                        // Experienced issue where CreateZoomLevelTileFile was receiving:
                        //  x=1, y=1, zoomLevelTileFilePath="...0_0.png"
                        // A local copy of the x,y variables are required having to do with the way closures work in threaded scenarios.
                        // Explanation can be found here: https://stackoverflow.com/questions/10179691/passing-arguments-with-changing-values-to-task-behaviour
                        int localX = x;
                        int localY = y;
                        tasks.Add(Task.Run(() => CreateZoomLevelTileFile(zoomLevelBaseImage.Clone(), localX, localY, zoomLevelTileFilePath)));
                        progressUpdater.Invoke();
                    }
                }

                // Wait for all tile creation tasks to complete
                var results = await Task.WhenAll(tasks);

            }
            
            return true;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Removes the tiles.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        public void RemoveTiles(Guid mapId)
        {
            var worldFolderPath = Path.Combine(_physicalWorldBasePath, $"{mapId}");
            if (Directory.Exists(worldFolderPath)) Directory.Delete(worldFolderPath, true);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Creates the zoom level base file.
        /// </summary>
        /// <param name="numberOfTilesPerDimension">The number of tiles per dimension.</param>
        /// <param name="masterFilePath">The master file path.</param>
        /// <param name="zoomLevelBaseFilePath">The zoom level base file path.</param>
        private Image<Rgba32> CreateZoomLevelBaseFile(int numberOfTilesPerDimension, string masterFilePath, string zoomLevelBaseFilePath)
        {

            using (Image<Rgba32> masterBaseImage = Image.Load(masterFilePath)) 
            {
                var size = numberOfTilesPerDimension * TilePixelSize;

                masterBaseImage.Mutate(context => context.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Pad,
                    Position = AnchorPositionMode.Center,
                    Size = new Size(size, size)
                }));

                masterBaseImage.Save(zoomLevelBaseFilePath);

            }

            return Image.Load(zoomLevelBaseFilePath);
        }

        /// <summary>
        ///     Creates the zoom level tile file.
        /// </summary>
        /// <param name="zoomLevelBaseFilePath">The zoom level base file path.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="zoomLevelTileFilePath">The zoom level tile file path.</param>
        private bool CreateZoomLevelTileFile(Image<Rgba32> baseImage, int x, int y, string zoomLevelTileFilePath)
        {

            baseImage.Mutate(context => context.Crop(
                new Rectangle(x * TilePixelSize, y * TilePixelSize, TilePixelSize, TilePixelSize)));

            baseImage.Save(zoomLevelTileFilePath);

            return true;
        }
            
        /// <summary>
        ///     Creates the master file and updates the map data.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="originalFilePath">The original file path.</param>
        /// <param name="masterFilePath">The master file path.</param>
        private void CreateMasterFileAndUpdateMapData(Map map, string originalFilePath, string masterFilePath)
        {
            using (var originalFileStream = new FileStream(originalFilePath, FileMode.Open))
            using (var masterFileStream = new FileStream(masterFilePath, FileMode.CreateNew))
            {
                var masterImage = Image.Load(Configuration.Default, originalFileStream);
                var width = masterImage.Width;
                var height = masterImage.Height;

                var largestSize = Math.Max(width, height);
                var maxZoomLevel = Math.Log((double) largestSize / TilePixelSize, 2);

                var adjustedMaxZoomLevel = (int) Math.Max(0, Math.Round(maxZoomLevel));
                var adjustedLargestSize = (int) Math.Round(Math.Pow(2, adjustedMaxZoomLevel) * TilePixelSize);

                if (width != height || largestSize != adjustedLargestSize)
                    masterImage = masterImage.Clone(context => context.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Pad,
                        Position = AnchorPositionMode.Center,
                        Size = new Size(width = adjustedLargestSize, height = adjustedLargestSize)
                    }));

                masterImage.SaveAsPng(masterFileStream);

                map.MaxZoomLevel = adjustedMaxZoomLevel;
                map.AdjustedSize = adjustedLargestSize;

                map.ThumbnailPath = $"{_virtualWorldBasePath}/{map.Id}/0/zoom-level.png";

                _mapDataService.Save(map);
            }
        }

        /// <summary>
        ///     Sums the specified values.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="valueGetter">The value getter.</param>
        /// <returns>System.Int32.</returns>
        private static int Sum(int from, int to, Func<int, int> valueGetter)
        {
            var result = 0;
            for (var i = from; i <= to; i++) result += valueGetter.Invoke(i);

            return result;
        }

        #endregion Private Methods
    }
}