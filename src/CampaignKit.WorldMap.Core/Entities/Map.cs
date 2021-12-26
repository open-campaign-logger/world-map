// <copyright file="Map.cs" company="Jochen Linnemann - IT-Service">
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

using System;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.Azure.Cosmos.Table;

namespace CampaignKit.WorldMap.Core.Entities
{
    /// <summary>Map Entity.</summary>
    public class Map : TableEntity
    {
        /// <summary>
        /// Gets or sets the id of the user this map belongs to.
        ///
        /// Azure Table Storage PartitionKey.
        ///
        /// </summary>
        /// <value>The user id.</value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        ///
        /// Azure Table Storage: RowKey.
        ///
        /// </summary>
        /// <value>The identifier.</value>
        public string MapId { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; set; }

        /// <summary>
        ///     Gets or sets the file extension.
        /// </summary>
        /// <value>The file extension.</value>
        public string FileExtension { get; set; }

        /// <summary>
        ///     Gets or sets the size of the adjusted.
        /// </summary>
        /// <value>The size of the adjusted.</value>
        public int AdjustedSize { get; set; }

        /// <summary>
        ///     Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        public string Copyright { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is public.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is public; otherwise, <c>false</c>.
        /// </value>
        public bool IsPublic { get; set; }

        /// <summary>
        ///     Gets or sets the marker data for this map.
        /// </summary>
        /// <value>A JSON representation of child marker entities for this map.</value>
        public string MarkerData { get; set; }

        /// <summary>
        ///     Gets or sets the maximum zoom level.
        /// </summary>
        /// <value>The maximum zoom level.</value>
        public int MaxZoomLevel { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [repeat map in x].
        /// </summary>
        /// <value><c>true</c> if [repeat map in x]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool RepeatMapInX { get; set; }

        /// <summary>
        ///     Gets or sets the map share key.
        /// </summary>
        /// <value>
        ///     The map share key.  Used for providing access to non-registered users.
        /// </value>
        public string ShareKey { get; set; }

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

    }
}