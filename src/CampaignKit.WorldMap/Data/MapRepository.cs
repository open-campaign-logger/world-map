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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using Size = SixLabors.Primitives.Size;

namespace CampaignKit.WorldMap.Entities
{
	/// <summary>
	///		EntityFramework interface for <c>Map</c> data elements.
	/// </summary>
	public interface IMapRepository
	{
		#region Public Methods

		/// <summary>Deletes the specified map and all child entities.</summary>
		/// <param name="id">The map identifier.</param>
		/// <returns><c>true</c> if successful, <c>false</c> otherwise</returns>
		Task<bool> Delete(int id);

		/// <summary>Finds the map associated with the identifier.</summary>
		/// <param name="id">The map identifier.</param>
		/// <returns><c>Map</c> if successful, <c>null</c> otherwise</returns>
		Task<Entities.Map> Find(int id);

		/// <summary>Finds all maps.</summary>
		/// <returns>IEnumerable&lt;Map&gt;.</returns>
		Task<IEnumerable<Entities.Map>> FindAll();

		/// <summary> Creates the specified map. </summary>
		/// <param name="map">The map entity to create.</param>
		/// <param name="stream">Map image data stream.</param>
		/// <returns><c>id</c> if successful, <c>0</c> otherwise</returns>
		Task<int> Create(Entities.Map map, Stream stream);

		/// <summary> Saves changes to the specified map.</summary>
		/// <param name="map">The map entity to save.</param>
		/// <returns><c>id</c> if successful, <c>false</c> otherwise</returns>
		Task<bool> Save(Entities.Map map);

		#endregion Public Methods
	}

	/// <summary>
	///		Default implementation of the EntityFramework respository for Map data elements.
	/// </summary>
	/// <seealso cref="CampaignKit.WorldMap.Entities.IMapRepository" />
	public class DefaultMapRepository : IMapRepository
	{
		#region Private Fields

		/// <summary>
		/// The database context
		/// </summary>
		private readonly WorldMapDBContext _dbContext;

		/// <summary>
		/// The file path service
		/// </summary>
		private readonly IFilePathService _filePathService;

		/// <summary>
		///		The application logging service.
		/// </summary>
		private readonly ILogger _loggerService;

		private const int TilePixelSize = 250;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultMapRepository"/> class.
		/// </summary>
		/// <param name="dbContext">The database context.</param>
		/// <param name="filePathService">The file path service.</param>
		/// <param name="loggerService">The logger service.</param>
		public DefaultMapRepository(WorldMapDBContext dbContext, 
			IFilePathService filePathService, 
			ILogger<DefaultMapRepository> loggerService)
		{
			_dbContext = dbContext;
			_filePathService = filePathService;
			_loggerService = loggerService;
		}

		#endregion Public Constructors

		#region IMapDataService Members

		#region Public Methods

		/// <summary>Deletes the specified map and all child entities.</summary>
		/// <param name="id">The map identifier.</param>
		/// <returns>
		///   <c>true</c> if successful, <c>false</c> otherwise</returns>
		public async Task<bool> Delete(int id)
		{
			// Determine if this map exists
			var map = await _dbContext.Maps.FindAsync(id);
			if (map == null)
			{
				_loggerService.LogError($"Map with id:{id} not found");
				return false;
			}

			// Remove the map from the context.
			_dbContext.Maps.Remove(map);
			await _dbContext.SaveChangesAsync();

			// Delete map directory and files
			if (Directory.Exists(map.WorldFolderPath))
			{
				Directory.Delete(map.WorldFolderPath, true);
			}

			// Return result
			return true;
		}

		/// <summary>Finds the map associated with the identifier.</summary>
		/// <param name="id">The map identifier.</param>
		/// <returns>
		///   <c>Map</c> if successful, <c>null</c> otherwise</returns>
		public async Task<Entities.Map> Find(int id)
		{
			// Retrieve the map entry and any associated markers.
			var map = await _dbContext.Maps
				.Include(m => m.Markers)
				.FirstOrDefaultAsync(m => m.MapId == id);

			if (map == null)
			{
				_loggerService.LogError($"Map with id:{id} not found");
				return null;
			}

			return map;
		}

		/// <summary>Finds all maps.</summary>
		/// <returns>IEnumerable&lt;Map&gt;.</returns>
		public async Task<IEnumerable<Entities.Map>> FindAll()
		{
			return await _dbContext.Maps.ToListAsync();
		}

		/// <summary>Creates the specified map.</summary>
		/// <param name="map">The map entity to create.</param>
		/// <param name="stream">Map image data stream.</param>
		/// <returns>
		///   <c>id</c> if successful, <c>0</c> otherwise</returns>
		public async Task<int> Create(Entities.Map map, Stream stream)
		{

			// **********************
			//   Precondition Tests
			// **********************
			// Image data not provided?
			if (stream == null) return 0;

			// ************************************
			//  Create DB entity (Generate Map ID)
			// ************************************
			_dbContext.Add(map);
			await _dbContext.SaveChangesAsync();

			// **********************
			//   Create Map Folder 
			// **********************
			var worldFolderPath = Path.Combine(_filePathService.PhysicalWorldBasePath, $"{map.MapId}");
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
					masterImage = masterImage.Clone(context => context.Resize(new ResizeOptions
					{
						Mode = ResizeMode.Pad,
						Position = AnchorPositionMode.Center,
						Size = new Size(width = adjustedLargestSize, height = adjustedLargestSize)
					}));

				masterImage.SaveAsPng(masterFileStream);

				map.MaxZoomLevel = adjustedMaxZoomLevel;
				map.AdjustedSize = adjustedLargestSize;

				map.ThumbnailPath = $"{_filePathService.VirtualWorldBasePath}/{map.MapId}/0/zoom-level.png";

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
						var tile = new Tile()
						{
							MapId = map.MapId,
							ZoomLevel = zoomLevel,
							CreationTimestamp = DateTime.UtcNow,
							TileSize = TilePixelSize,
							X = x,
							Y = y
						};
						map.Tiles.Add(tile);

					}
				}
			}

			// ************************************
			//   Update Map Entity
			// ************************************
			map.WorldFolderPath = worldFolderPath;
			_dbContext.Update(map);
			await _dbContext.SaveChangesAsync();
			
			return map.MapId;
			
		}

		/// <summary>Saves changes to the specified map.</summary>
		/// <param name="map">The map entity to save.</param>
		/// <returns>
		///   <c>id</c> if successful, <c>false</c> otherwise</returns>
		public async Task<bool> Save(Entities.Map map)
		{
			_dbContext.Update(map);
			await _dbContext.SaveChangesAsync();
			return true;

		}

		#endregion Public Methods

		#endregion

		#region Private Methods


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


		#endregion

	}
}