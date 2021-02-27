// <copyright file="Tile.cs" company="Jochen Linnemann - IT-Service">
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

namespace CampaignKit.WorldMap.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    ///     Tile entity.
    /// </summary>
    public class Tile : TableEntity
    {
        /// <summary>
        /// Gets or sets the parent identifier.
        ///
        /// Azure Table Storage PartitionKey.
        ///
        /// </summary>
        /// <value>The parent identifier.</value>
        public string MapId { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        ///
        /// Azure Table Storage: RowKey.
        ///
        /// </summary>
        /// <value>The identifier.</value>
        public string TileId { get; set; }

        /// <summary>
        ///     Gets or sets the size of the tile in bytes.
        /// </summary>
        /// <value>The size of the tile.</value>
        public int TileSize { get; set; }

        /// <summary>
        ///     Gets or sets or set the tile's x coordinate.
        /// </summary>
        /// <value>The tile's x coordinate.</value>
        public int X { get; set; }

        /// <summary>
        ///     Gets or sets or set the tile's y coordinate.
        /// </summary>
        /// <value>The tile's y coordinate.</value>
        public int Y { get; set; }

        /// <summary>
        ///     Gets or sets the tile's zoom level.
        /// </summary>
        /// <value>The tile's zoom level value.</value>
        public int ZoomLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is rendered.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is completed; otherwise, <c>false</c>.
        /// </value>
        public bool IsRendered { get; set; }

        /// <summary>
        ///     Gets or sets the tile creation timestamp.
        /// </summary>
        /// <value>Time that the tile creation process started.</value>
        public DateTime CreationTimestamp { get; set; }
    }
}