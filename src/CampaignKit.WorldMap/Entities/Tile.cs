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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignKit.WorldMap.Entities
{
	/// <summary>
	///		Class tile.
	/// </summary>
	public class Tile
	{
		#region Public Properties

		/// <summary>
		///     Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public int TileId { get; set; }

		/// <summary>
		///     Gets or sets the parent identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public int MapId { get; set; }

		/// <summary>
		///     Gets or sets the tile path.
		/// </summary>
		/// <value>The path.</value>
		public string Path { get; set; }

		/// <summary>
		///     Gets or sets the tile creation timestamp
		/// </summary>
		/// <value>Time that the tile creation process started.</value>
		[Column(TypeName = "DateTime")]
		public DateTime CreationTimestamp { get; set; }

		/// <summary>
		///     Gets or sets the tile completion timestamp
		/// </summary>
		/// <value>Time that the tile creation process completed.</value>
		[Column(TypeName = "DateTime")]
		public DateTime CompletionTimestamp { get; set; }

		/// <summary>
		///     Gets or sets the size of the tile in bytes.
		/// </summary>
		/// <value>The size of the tile.</value>
		public int TileSize { get; set; }

		#endregion

	}
}
