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

using System.Threading.Tasks;

using CampaignKit.WorldMap.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.Services
{
	/// <summary>
	///     Interface IMarkerDataService
	/// </summary>
	public interface IMarkerDataService
	{
		#region Public Methods

		/// <summary>
		///     Deletes the specified marker.
		/// </summary>
		/// <param name="id">The marker's identifier.</param>
		/// <returns><c>true</c> if deletion successful, <c>false</c> otherwise.</returns>
		Task<bool> Delete(int id);

		/// <summary>
		///     Finds the specified marker.
		/// </summary>
		/// <param name="id">The marker's identifier.</param>
		/// <returns><c>Map</c> if found, <c>null</c> otherwise.</returns>
		Task<Marker> Find(int id);

		/// <summary>
		///     Creates the specified marker.
		/// </summary>
		/// <param name="mapId">The parent map's identifier.</param>
		/// <param name="marker">The marker to create.</param>
		/// <returns><c>marker identifier</c> if created, <c>0</c> otherwise.</returns>
		Task<int> Create(int mapId, Marker marker);

		/// <summary>
		///     Saves changes to the specified marker.
		/// </summary>
		/// <param name="marker">The marker's data.</param>
		/// <returns><c>true</c> if save successful, <c>false</c> otherwise.</returns>
		Task<bool> Save(Marker marker);

		#endregion Public Methods
	}
	   
	/// <inheritdoc />
	/// <summary>
	///     Class DefaultMarkerDataService.
	/// </summary>
	/// <seealso cref="T:CampaignKit.WorldMap.Services.IMarkerDataService" />
	public class DefaultMarkerDataService : IMarkerDataService
	{

		#region Private Fields

		private readonly MappingContext _context;
		private readonly ILogger _logger;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		///     Initializes a new instance of the <see cref="DefaultMarkerDataService" /> class.
		/// </summary>
		/// <param name="context">The database context.</param>
		/// <param name="logger">The logging service.</param>
		public DefaultMarkerDataService(MappingContext context,
			ILogger<TileCreationService> logger)
		{
			_context = context;
			_logger = logger;
		}

		#endregion Public Constructors

		#region IMapDataService Members

		#region Public Members

		/// <summary>Deletes the specified marker.</summary>
		/// <param name="id">The marker's identifier.</param>
		/// <returns>
		///   <c>true</c> if deletion successful, <c>false</c> otherwise.</returns>
		public async Task<bool> Delete(int id)
		{

			// Determine if this map exists
			var marker = await _context.Markers.FindAsync(id);
			if (marker == null)
			{
				_logger.LogError($"Marker with id:{id} not found");
				return false;
			}

			// Remove the marker from the context.
			_context.Markers.Remove(marker);
			await _context.SaveChangesAsync();

			// Return result
			return true;

		}

		/// <summary>Finds the specified marker.</summary>
		/// <param name="id">The marker's identifier.</param>
		/// <returns>
		///   <c>Map</c> if found, <c>null</c> otherwise.</returns>
		public async Task<Marker> Find(int id)
		{
			// Retrieve the marker entry
			var marker = await _context.Markers
				.FirstOrDefaultAsync(m => m.MarkerId == id);

			if (marker == null)
			{
				_logger.LogError($"Marker with id:{id} not found");
				return null;
			}

			return marker;
		}

		/// <summary>Creates the specified marker.</summary>
		/// <param name="mapId">The parent map's identifier.</param>
		/// <param name="marker">The marker to create.</param>
		/// <returns>
		///   <c>marker identifier</c> if created, <c>0</c> otherwise.</returns>
		public async Task<int> Create(int mapId, Marker marker)
		{
			// **********************
			//   Precondition Tests
			// **********************
			// Map id provided?
			if (mapId == 0)
			{
				_logger.LogError($"Map id not provided");
				return 0;
			}
			// Marker data provided?
			if (marker == null)
			{
				_logger.LogError($"Marker data not provided");
				return 0;
			}			

			// ************************************
			//  Create DB entity (Generate Marker ID)
			// ************************************
			_context.Add(marker);
			await _context.SaveChangesAsync();
			
			return marker.MarkerId;
		}
		
		/// <summary>Saves changes to the specified marker.</summary>
		/// <param name="marker">The marker's data.</param>
		/// <returns>
		///   <c>true</c> if save successful, <c>false</c> otherwise.</returns>
		public async Task<bool> Save(Marker marker)
		{
			_context.Update(marker);
			await _context.SaveChangesAsync();
			return true;
		}

		#endregion

		#endregion

	}

}