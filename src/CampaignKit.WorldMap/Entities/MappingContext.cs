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

using Microsoft.EntityFrameworkCore;

namespace CampaignKit.WorldMap.Entities
{
	public class MappingContext: DbContext
	{

		public MappingContext(DbContextOptions<MappingContext> options)
			: base(options)
		{ }

		public DbSet<Map> Maps { get; set; }
		public DbSet<Tile> Tiles { get; set; }
		public DbSet<Marker> Markers { get; set; }

		/// <summary>
		/// Seeds the database with data for the sample map.
		/// 
		/// This method is called when the model for a derived context has been initialized, 
		/// but before the model has been locked down and used to initialize the context. 
		/// The default implementation of this method does nothing, but it can be overridden in a 
		/// derived class such that the model can be further configured before it is locked down.
		/// </summary>
		/// <param name="modelBuilder">Edit Provides a simple API surface for configuring a IMutableModel that defines the shape of your entities, the relationships between them, and how they map to the database.</param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}
		
	}
}
