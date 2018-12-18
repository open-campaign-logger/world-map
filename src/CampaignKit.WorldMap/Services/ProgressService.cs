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

using System.Collections.Generic;
using CampaignKit.WorldMap.Entities;
using Microsoft.Extensions.Logging;

using System.Linq;
using System;

namespace CampaignKit.WorldMap.Services
{

	/// <summary>
	///     Interface IProgressService
	/// </summary>
	public interface IProgressService
	{
		#region Public Methods

		/// <summary>
		///     Gets the map creation progress.
		///     0.0 = 0% .. 1.0 = 100%
		/// </summary>
		/// <param name="mapId">The map identifier.</param>
		/// <returns>System.Double.</returns>
		double? GetMapProgress(string mapId);

		#endregion Public Methods
	}

	/// <inheritdoc />
	/// <summary>
	///     Class DefaultProgressService.
	/// </summary>
	/// <seealso cref="T:CampaignKit.WorldMap.Services.IProgressService" />
	public class DefaultProgressService : IProgressService
	{
		#region Private Fields

		private readonly MappingContext _context;
		private readonly ILogger _logger;

		#endregion

		#region Public Constructors

		/// <summary>
		///     Initializes a new instance of the <see cref="DefaultProgressService" /> class.
		/// </summary>
		public DefaultProgressService(MappingContext context,
				ILogger<TileCreationService> logger)
		{
			_context = context;
			_logger = logger;
		}

		#endregion

		#region Public Methods

		/// <inheritdoc />
		/// <summary>
		///     Gets the progress.
		/// </summary>
		/// <param name="mapId">The map identifier.</param>
		/// <returns>System.Double.</returns>
		public double? GetMapProgress(string indicatorId)
		{
			// Create a default null return value
			var progress = (double?)null;

			// Setup query datasource
			// The following is equivalent to: foreach "t" in "_context.Tiles" select "t"
			var tiles = (from t  // This defines the local variable
			   in _context.Tiles // This defines the database
						select t); // This defines what will be selected

			// Establish query parameters
			tiles.Where(t => t.MapId == Convert.ToInt32(indicatorId));

			// Execute query
			// Since we're using a database we need to call this step
			// so that the query will actually execute.  (lazy loading)
			var tileList = tiles.ToList();

			// Are there tiles defined for this map?
			if (tileList.Count > 0)
			{

				// Count the number of completed items in the set retrieved from the database
				var completed = (from x in tileList where x.CompletionTimestamp != null select x).Count();

				// Calculate the percentage complete ensuring to strongly type the 
				// dividend and divisor so that the rounding does not occur during calculation
				// and the resulting quotient is the correct data type.
				progress = (double)completed / (double)tileList.Count();

			}
			else
			{
				_logger.LogError($"No tiles found for map with id={indicatorId}");
			}

			// Return the progress value
			return progress;
		}

		#endregion Public Methods
	}
}