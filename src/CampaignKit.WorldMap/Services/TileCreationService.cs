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
using System.IO;
using System.Threading.Tasks;

using CampaignKit.WorldMap.Entities;

using Microsoft.AspNetCore.Mvc.Routing;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

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
            if (stream == null) return false;

            var map = _mapDataService.Find($"{mapId}");

            if (map == null) return false;

            var worldFolderPath = Path.Combine(_physicalWorldBasePath, $"{mapId}");

            if (Directory.Exists(worldFolderPath)) return false;

            Directory.CreateDirectory(worldFolderPath);

            var originalFilePath = Path.Combine(worldFolderPath, $"original-file{map.FileExtension}");

            using (stream)
            using (var originalFileStream = new FileStream(originalFilePath, FileMode.CreateNew))
            {
                stream.CopyTo(originalFileStream);
            }

            var progressIndicator = _progressService.CreateIndicator($"{mapId}");
            progressIndicator.SetProgress(0.0);

            return await Task.Run(() =>
            {
                var masterFilePath = Path.Combine(worldFolderPath, "master-file.png");
                CreateMasterFileAndUpdateMapData(map, originalFilePath, masterFilePath);

                var stepCounter = 1;
                var totalNumberOfSteps = 1; /* preparing the master file */
                totalNumberOfSteps += map.MaxZoomLevel + 1; /* preparing one zoom-level file per zoom level */
                totalNumberOfSteps += Sum(0, map.MaxZoomLevel, /* preparing the tiles of each zoom level */
                    zoomLevel => (int) Math.Pow((int) Math.Pow(2, zoomLevel), 2));

                var progressUpdater = new Action(() =>
                    progressIndicator.SetProgress(++stepCounter / (double) totalNumberOfSteps));

                for (var zoomLevel = 0; zoomLevel <= map.MaxZoomLevel; zoomLevel++)
                {
                    var numberOfTilesPerDimension = (int) Math.Pow(2, zoomLevel);

                    var zoomLevelFolderPath = Path.Combine(worldFolderPath, $"{zoomLevel}");
                    Directory.CreateDirectory(zoomLevelFolderPath);

                    var zoomLevelBaseFilePath = Path.Combine(zoomLevelFolderPath, "zoom-level.png");
                    CreateZoomLevelBaseFile(numberOfTilesPerDimension, masterFilePath, zoomLevelBaseFilePath);
                    progressUpdater.Invoke();

                    for (var x = 0; x < numberOfTilesPerDimension; x++)
                    for (var y = 0; y < numberOfTilesPerDimension; y++)
                    {
                        var zoomLevelTileFilePath = Path.Combine(zoomLevelFolderPath, $"{x}_{y}.png");
                        CreateZoomLevelTileFile(zoomLevelBaseFilePath, x, y, zoomLevelTileFilePath);
                        progressUpdater.Invoke();
                    }
                }

                return true;
            });
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
        private static void CreateZoomLevelBaseFile(
            int numberOfTilesPerDimension, string masterFilePath, string zoomLevelBaseFilePath)
        {
            using (var masterFileStream = new FileStream(masterFilePath, FileMode.Open))
            using (var zoomLevelBaseFileStream = new FileStream(zoomLevelBaseFilePath, FileMode.CreateNew))
            {
                var size = numberOfTilesPerDimension * TilePixelSize;

                var zoomLevelBaseImage = Image.Load(Configuration.Default, masterFileStream);
                zoomLevelBaseImage.Clone(context => context.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Pad,
                    Position = AnchorPositionMode.Center,
                    Size = new Size(size, size)
                })).SaveAsPng(zoomLevelBaseFileStream);
            }
        }

        /// <summary>
        ///     Creates the zoom level tile file.
        /// </summary>
        /// <param name="zoomLevelBaseFilePath">The zoom level base file path.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="zoomLevelTileFilePath">The zoom level tile file path.</param>
        private static void CreateZoomLevelTileFile(
            string zoomLevelBaseFilePath, int x, int y, string zoomLevelTileFilePath)
        {
            using (var inStream = new FileStream(zoomLevelBaseFilePath, FileMode.Open))
            using (var outStream = new FileStream(zoomLevelTileFilePath, FileMode.CreateNew))
            {
                var baseImage = Image.Load(Configuration.Default, inStream);
                baseImage.Clone(context =>
                        context.Crop(new Rectangle(x * TilePixelSize, y * TilePixelSize, TilePixelSize, TilePixelSize)))
                    .SaveAsPng(outStream);
            }
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