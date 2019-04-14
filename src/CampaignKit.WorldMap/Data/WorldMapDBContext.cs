﻿// Copyright 2017-2019 Jochen Linnemann, Cory Gill
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

using CampaignKit.WorldMap.Entities;

using Microsoft.EntityFrameworkCore;

namespace CampaignKit.WorldMap.Data
{
    /// <inheritdoc />
    /// <summary>
    ///     Database context service.
    /// </summary>
    public class WorldMapDBContext : DbContext
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="WorldMapDBContext" /> class.</summary>
        /// <param name="options">The options.</param>
        public WorldMapDBContext(DbContextOptions<WorldMapDBContext> options)
            : base(options)
        {
        }

        #endregion

        #region Properties

        /// <summary>Gets or sets the maps.</summary>
        /// <value>The maps.</value>
        public DbSet<Map> Maps { get; set; }

        /// <summary>Gets or sets the tiles.</summary>
        /// <value>The tiles.</value>
        public DbSet<Tile> Tiles { get; set; }

        #endregion
    }
}