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


using System.ComponentModel.DataAnnotations;

namespace CampaignKit.WorldMap.Entities
{
	/// <summary>
	/// Marker Entity
	/// </summary>
	public class Marker
	{
		#region Public Properties

		/// <summary>
		///     Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[Key]
		public int MarkerId { get; set; }
		
		/// <summary>
		///     Gets or sets the marker JSON
		/// </summary>
		/// <value>Marker data in JSON format.</value>
		[Required]
		public string JSON { get; set; }

		/// <summary>
		///		Gets  or sets the parent entity.
		/// </summary>
		/// <value>The parent entity.</value>
		[Required]
		public Map Map { get; set; }

		/// <summary>
		///     Gets or sets the parent identifier.
		/// </summary>
		/// <value>The parent identifier.</value>
		[Required]
		public int MapId { get; set; }

		#endregion

	}
}
