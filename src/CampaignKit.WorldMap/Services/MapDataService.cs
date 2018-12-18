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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CampaignKit.WorldMap.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using SixLabors.ImageSharp.Formats;
using Size = SixLabors.Primitives.Size;

namespace CampaignKit.WorldMap.Services
{
	/// <summary>
	///     Interface IMapDataService
	/// </summary>
	public interface IMapDataService
	{
		#region Public Methods

		/// <summary>
		///     Deletes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		Task<bool> Delete(int id);

		/// <summary>
		///     Finds the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Map.</returns>
		Task<Map> Find(int id);

		/// <summary>
		///     Finds all maps.
		/// </summary>
		/// <returns>IEnumerable&lt;Map&gt;.</returns>
		Task<IEnumerable<Map>> FindAll();

		/// <summary>
		///     Creates the specified map.
		/// </summary>
		/// <param name="map">The map.</param>
		/// <param name="stream">Streamed image data.</param>
		/// <returns>MapId.</returns>
		Task<int> Create(Map map, Stream stream);

		/// <summary>
		///     Saves changes to the specified map.
		/// </summary>
		/// <param name="map">The map.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		Task<bool> Save(Map map);

		#endregion Public Methods
	}

	/// <inheritdoc />
	/// <summary>
	///     Class DefaultMapDataService.
	/// </summary>
	/// <seealso cref="T:CampaignKit.WorldMap.Services.IMapDataService" />
	public class DefaultMapDataService : IMapDataService
	{
		#region Private Fields

		private readonly MappingContext _context;
		private readonly IFilePathService _filePathService;
		private readonly ILogger _logger;
		
		private const int TilePixelSize = 250;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		///     Initializes a new instance of the <see cref="DefaultMapDataService" /> class.
		/// </summary>
		/// <param name="filePathService">The application data path service.</param>
		public DefaultMapDataService(MappingContext context, 
			IFilePathService filePathService, 
			ILogger<TileCreationService> logger)
		{
			_context = context;
			_filePathService = filePathService;
			_logger = logger;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		/// <summary>
		///     Deletes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public async Task<bool> Delete(int id)
		{
			// Delete map db entry.  This includes cascade delete to children.
			var map = await _context.Maps.FindAsync(id);
			if (map == null)
			{
				_logger.LogError($"Map with id:{id} not found");
				return false;
			}
			_context.Maps.Remove(map);
			await _context.SaveChangesAsync();

			// Delete map directory and files
			if (Directory.Exists(map.WorldFolderPath))
			{
				Directory.Delete(map.WorldFolderPath, true);
			}

			return true;
		}

		/// <inheritdoc />
		/// <summary>
		///     Finds the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Map.</returns>
		public async Task<Map> Find(int id)
		{
			// Retrieve the map db entry.
			var map = await _context.Maps.FirstOrDefaultAsync(m => m.MapId == id);
			if (map == null)
			{
				_logger.LogError($"Map with id:{id} not found");
				return null;
			}

			return map;
		}
		
		/// <inheritdoc />
		/// <summary>
		///     Finds all maps.
		/// </summary>
		/// <returns>IEnumerable&lt;Map&gt;.</returns>
		public async Task<IEnumerable<Map>> FindAll()
		{
			return await _context.Maps.ToListAsync();
		}

		/// <inheritdoc />
		/// <summary>
		///     Saves the specified map.
		/// </summary>
		/// <param name="map">The map.</param>
		/// <param name="stream">Streamed image data.</param>
		/// <returns>MapId for the created map.</returns>
		public async Task<int> Create(Map map, Stream stream)
		{

			// **********************
			//   Precondition Tests
			// **********************
			// Image data not provided?
			if (stream == null) return 0;

			// ************************************
			//  Create DB entity (Generate Map ID)
			// ************************************
			_context.Add(map);
			await _context.SaveChangesAsync();

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
			_context.Update(map);
			await _context.SaveChangesAsync();
			
			return map.MapId;
			
		}

		/// <inheritdoc />
		/// <summary>
		///     Saves changes to the specified map.
		/// </summary>
		/// <param name="map">The map.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public async Task<bool> Save(Map map)
		{
			_context.Update(map);
			await _context.SaveChangesAsync();
			return true;

		}

		#endregion Public Methods

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