// <copyright file="TileCreationService.cs" company="Jochen Linnemann - IT-Service">
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
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CampaignKit.WorldMap.Data;
    using CampaignKit.WorldMap.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;

    /// <summary>
    ///     A timed background service that queries the Tiles table and processes
    ///     tiles that haven't been created yet.
    ///     This article was used to model this timed background service.
    ///     https://thinkrethink.net/2018/02/21/asp-net-core-background-processing/.
    /// </summary>
    public class TileCreationService : BackgroundService
    {
        /// <summary>
        ///     The stopping cancellation token.
        /// </summary>
        private readonly CancellationTokenSource stoppingCts = new CancellationTokenSource();

        /// <summary>
        ///     The application logging service.
        /// </summary>
        private readonly ILogger loggerService;

        /// <summary>
        ///     The executing task.
        /// </summary>
        private Task executingTask;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TileCreationService" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="loggerService">The logger service.</param>
        public TileCreationService(IServiceProvider serviceProvider, ILogger<TileCreationService> loggerService)
        {
            this.loggerService = loggerService;
            this.ServiceProvider = serviceProvider;
        }

        /// <summary>
        ///     Gets the service provider.
        /// </summary>
        /// <value>
        ///     The service provider.
        /// </value>
        private IServiceProvider ServiceProvider { get; }

        /// <summary>
        ///     Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task we're executing
            this.executingTask = this.ExecuteAsync(this.stoppingCts.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (this.executingTask.IsCompleted)
            {
                return this.executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (this.executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                this.stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(this.executingTask, Task.Delay(
                    Timeout.Infinite,
                    cancellationToken));
            }
        }

        /// <summary>
        ///     This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The
        ///     implementation should return a task that represents
        ///     the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">
        ///     Triggered when
        ///     <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is
        ///     called.
        /// </param>
        /// <returns>
        ///     A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running operations.
        /// </returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                await this.Process();

                await Task.Delay(5000, stoppingToken); // 5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        /// <summary>
        ///     Executes the timed background process for generating tiles.
        /// </summary>
        /// <returns>True when processing is completed.</returns>
        protected async Task<bool> Process()
        {
            using (var scope = this.ServiceProvider.CreateScope())
            {
                // Retrieve the db context from the container
                var dbContext = scope.ServiceProvider.GetRequiredService<WorldMapDBContext>();
                var filePathService = scope.ServiceProvider.GetRequiredService<IFilePathService>();

                // Open the db connection
                dbContext.Database.OpenConnection();

                // Query the tiles table to see if there are any unprocessed tiles.
                var tiles = (from t in dbContext.Tiles select t)
                    .Where(t => t.CompletionTimestamp == DateTime.MinValue)
                    .OrderBy(t => t.MapId).ThenBy(t => t.ZoomLevel)
                    .ToList();

                if (tiles.Count > 0)
                {
                    this.loggerService.LogDebug($"{tiles.Count} Tiles found.");

                    // Process each map with unprocessed tiles
                    var mapList = tiles.Select(o => o.MapId).Distinct();
                    foreach (var map in mapList)
                    {
                        // Process each map zoom level with unprocessed tiles
                        var zoomList = tiles.Where(t => t.MapId == map).Select(o => o.ZoomLevel).Distinct();
                        foreach (var zoomLevel in zoomList)
                        {
                            // Calculate Folder Paths
                            var worldFolderPath = Path.Combine(filePathService.PhysicalWorldBasePath, $"{map}");
                            var masterFilePath = Path.Combine(worldFolderPath, "master-file.png");
                            var zoomLevelFolderPath = Path.Combine(worldFolderPath, $"{zoomLevel}");
                            var zoomLevelBaseFilePath = Path.Combine(zoomLevelFolderPath, "zoom-level.png");

                            // Select Tiles for Processing
                            var tilesToProcess = tiles.Where(t => t.MapId == map).Where(t => t.ZoomLevel == zoomLevel);

                            // Calculate Tile Size
                            var tilePixelSize = tiles[0].TileSize;

                            // Calculate the number of tiles required for this zoom level
                            var numberOfTilesPerDimension = (int)Math.Pow(2, zoomLevel);

                            // Create zoom level directory if required
                            if (!Directory.Exists(zoomLevelFolderPath))
                            {
                                Directory.CreateDirectory(zoomLevelFolderPath);
                            }

                            // Create zoom level base file (sync)
                            var zoomLevelBaseImage
                                = this.CreateZoomLevelBaseFile(
                                    numberOfTilesPerDimension,
                                    masterFilePath,
                                    zoomLevelBaseFilePath,
                                    tilePixelSize);

                            // Create collection for tile creation tasks
                            var tasks = new List<Task<bool>>();
                            tasks.Clear();

                            // Cycle through the tiles selected for the current map and zoom and process them
                            foreach (var tile in tilesToProcess)
                            {
                                var zoomLevelTileFilePath = Path.Combine(zoomLevelFolderPath, $"{tile.X}_{tile.Y}.png");
                                tasks.Add(Task.Run(() =>
                                    this.CreateZoomLevelTileFile(zoomLevelBaseImage.Clone(), tile, zoomLevelTileFilePath)));
                            }

                            // Wait for all tile creation tasks to complete
                            var results = await Task.WhenAll(tasks);

                            // Update tile completion timestampts
                            dbContext.SaveChanges();
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     Creates the zoom level base file.
        /// </summary>
        /// <param name="numberOfTilesPerDimension">The number of tiles per dimension.</param>
        /// <param name="masterFilePath">The master file path.</param>
        /// <param name="zoomLevelBaseFilePath">The zoom level base file path.</param>
        /// <param name="tilePixelSize">Tile pixel size.</param>
        private Image<Rgba32> CreateZoomLevelBaseFile(int numberOfTilesPerDimension, string masterFilePath, string zoomLevelBaseFilePath, int tilePixelSize)
        {
            using (var masterBaseImage = Image.Load(masterFilePath))
            {
                var size = numberOfTilesPerDimension * tilePixelSize;

                masterBaseImage.Mutate(context => context.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Pad,
                    Position = AnchorPositionMode.Center,
                    Size = new Size(size, size),
                }));

                masterBaseImage.Save(zoomLevelBaseFilePath);
            }

            return Image.Load<Rgba32>(zoomLevelBaseFilePath);
        }

        /// <summary>
        ///     Creates the zoom level tile file.
        /// </summary>
        /// <param name="baseImage">The base image.</param>
        /// <param name="tile">The tile.</param>
        /// <param name="zoomLevelTileFilePath">The zoom level tile file path.</param>
        /// <returns>True when complete.</returns>
        private bool CreateZoomLevelTileFile(Image<Rgba32> baseImage, Tile tile, string zoomLevelTileFilePath)
        {
            if (!File.Exists(zoomLevelTileFilePath))
            {
                baseImage.Mutate(context => context.Crop(
                    new Rectangle(tile.X * tile.TileSize, tile.Y * tile.TileSize, tile.TileSize, tile.TileSize)));

                baseImage.Save(zoomLevelTileFilePath);

                tile.CompletionTimestamp = DateTime.UtcNow;
            }

            return true;
        }
    }
}