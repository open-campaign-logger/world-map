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
using System.Threading;

using System.Linq;
using System.Threading.Tasks;
using CampaignKit.WorldMap.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using SixLabors.ImageSharp.Formats;
using System.IO;

namespace CampaignKit.WorldMap.Services
{
	public class TileCreationService : BackgroundService
	{
		#region Private Members
		
		private ILogger _logger;
		private IServiceProvider _serviceProvider { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="serviceProvider">The context service provider</param>
		/// <param name="logger">The context logger</param>
		public TileCreationService(IServiceProvider serviceProvider, ILogger<TileCreationService> logger)
		{
			_logger = logger;
			_serviceProvider = serviceProvider;
		}

		#endregion

		#region Protected Methods
		/// <summary>
		/// Executes the timed background process for generating tiles.
		/// </summary>
		/// <param name="stoppingToken">Cancellation process to be passed by container when shutting down.</param>
		/// <returns></returns>
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogDebug($"TileCreationService is starting.");

			stoppingToken.Register(() =>
					_logger.LogDebug($" GracePeriod background task is stopping."));

			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogDebug($"TileCreationService is doing background work.");

				using (IServiceScope scope = _serviceProvider.CreateScope())
				{
					// Retrieve the db context from the container
					var dbContext = scope.ServiceProvider.GetRequiredService<MappingContext>();
					var filePathService = scope.ServiceProvider.GetRequiredService<IFilePathService>();

					// Open the db connection
					dbContext.Database.OpenConnection();

					// Query the tiles table to see if there are any unprocessed tiles.
					var tiles = (from t in dbContext.Tiles select t)
						.Where(t => t.CompletionTimestamp != null)
						.OrderBy(t => t.MapId).ThenBy(t => t.ZoomLevel)
						.ToList();
					
					if (tiles.Count > 0)
					{
						_logger.LogDebug($"{tiles.Count} Tiles found.");

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
									= CreateZoomLevelBaseFile(
										numberOfTilesPerDimension, 
										masterFilePath, 
										zoomLevelBaseFilePath, 
										tilePixelSize);

								// Create collection for tile creation tasks
								List<Task<bool>> tasks = new List<Task<bool>>();
								tasks.Clear();
															   
								// Cycle through the tiles selected for the current map and zoom and process them
								foreach (var tile in tilesToProcess)
								{
									var zoomLevelTileFilePath = Path.Combine(zoomLevelFolderPath, $"{tile.X}_{tile.Y}.png");
									tasks.Add(Task.Run(() => CreateZoomLevelTileFile(zoomLevelBaseImage.Clone(), tile, zoomLevelTileFilePath)));
								}

								// Wait for all tile creation tasks to complete
								var results = await Task.WhenAll(tasks);

								// Update tile completion timestampts
								dbContext.SaveChanges();
							}
						}
					}
				}

				await Task.Delay(5000, stoppingToken);
			}

			_logger.LogDebug($"TileCreationService is stopping.");

		}

		#endregion

		#region Private Methods

		/// <summary>
		///     Creates the zoom level base file.
		/// </summary>
		/// <param name="numberOfTilesPerDimension">The number of tiles per dimension.</param>
		/// <param name="masterFilePath">The master file path.</param>
		/// <param name="zoomLevelBaseFilePath">The zoom level base file path.</param>
		/// <param name="tilePixelSize">Tile pixel size.</param>
		private Image<Rgba32> CreateZoomLevelBaseFile(int numberOfTilesPerDimension, string masterFilePath, string zoomLevelBaseFilePath, int tilePixelSize)
		{

			using (Image<Rgba32> masterBaseImage = Image.Load(masterFilePath))
			{
				var size = numberOfTilesPerDimension * tilePixelSize;

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
		/// <param name="x">The tile to create.</param>
		/// <param name="zoomLevelTileFilePath">The zoom level tile file path.</param>
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

		#endregion

	}
}
