// Copyright 2017-2019 Jochen Linnemann, Cory Gill
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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignKit.WorldMap.Entities
{
    /// <summary>
    ///     Tile entity.
    /// </summary>
    public class Tile
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the tile completion timestamp
        /// </summary>
        /// <value>Time that the tile creation process completed.</value>
        [Column(TypeName = "DateTime")]
        public DateTime CompletionTimestamp { get; set; }

        /// <summary>
        ///     Gets or sets the tile creation timestamp
        /// </summary>
        /// <value>Time that the tile creation process started.</value>
        [Column(TypeName = "DateTime")]
        [Required]
        public DateTime CreationTimestamp { get; set; }

        /// <summary>
        ///     Gets  or sets the parent entity.
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

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int TileId { get; set; }

        /// <summary>
        ///     Gets or sets the size of the tile in bytes.
        /// </summary>
        /// <value>The size of the tile.</value>
        [Required]
        public int TileSize { get; set; }

        /// <summary>
        ///     Gets or set the tile's x coordinate.
        /// </summary>
        /// <value>The tile's x coordinate</value>
        [Required]
        public int X { get; set; }

        /// <summary>
        ///     Gets or set the tile's y coordinate.
        /// </summary>
        /// <value>The tile's y coordinate</value>
        [Required]
        public int Y { get; set; }

        /// <summary>
        ///     Gets or sets the tile's zoom level.
        /// </summary>
        /// <value>The tile's zoom level value.</value>
        [Required]
        public int ZoomLevel { get; set; }

        #endregion
    }
}