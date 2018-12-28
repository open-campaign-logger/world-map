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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignKit.WorldMap.Entities
{
	/// <summary>Map Entity</summary>
	public class Map
	{
		#region Public Properties

		/// <summary>
		///     Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public int MapId { get; set; }

		/// <summary>
		///     Gets or sets the size of the adjusted.
		/// </summary>
		/// <value>The size of the adjusted.</value>
		public int AdjustedSize { get; set; }

		/// <summary>
		///     Gets or sets the type of the content.
		/// </summary>
		/// <value>The type of the content.</value>
		public string ContentType { get; set; }

		/// <summary>
		///     Gets or sets the copyright.
		/// </summary>
		/// <value>The copyright.</value>
		public string Copyright { get; set; }

		/// <summary>
		///     Gets or sets the creation timestamp.
		/// </summary>
		/// <value>The creation timestamp.</value>
		[Column(TypeName = "datetime")]
		public DateTime CreationTimestamp { get; set; }

		/// <summary>
		///     Gets or sets the file extension.
		/// </summary>
		/// <value>The file extension.</value>
		public string FileExtension { get; set; }

		/// <summary>
		///     Gets or sets the maximum zoom level.
		/// </summary>
		/// <value>The maximum zoom level.</value>
		public int MaxZoomLevel { get; set; }

		/// <summary>
		///     Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether [repeat map in x].
		/// </summary>
		/// <value><c>true</c> if [repeat map in x]; otherwise, <c>false</c>.</value>
		[DefaultValue(true)]
		public bool RepeatMapInX { get; set; }

		/// <summary>
		///     Gets or sets the secret.
		/// </summary>
		/// <value>The secret.</value>
		public string Secret { get; set; }

		/// <summary>
		///     Gets or sets the world folder path.
		/// </summary>
		/// <value>The world folder path.</value>
		public string WorldFolderPath { get; set; }

		/// <summary>
		///     Gets or sets the map's thumbnail path.
		/// </summary>
		/// <value>The map's thumbnail path.</value>
		public string ThumbnailPath { get; set; }

		/// <summary>
		///     Gets or sets the tile collection for this map.
		/// </summary>
		/// <value>A collection of child tile entities.</value>
		public ICollection<Tile> Tiles { get; set; }

		/// <summary>
		///     Gets or sets the marker collection for this map.
		/// </summary>
		/// <value>A collection of child marker entities.</value>
		public ICollection<Marker> Markers { get; set; }

		#endregion Public Properties
	}
}
