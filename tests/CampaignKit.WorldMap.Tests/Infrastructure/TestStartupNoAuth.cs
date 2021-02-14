// Copyright 2017-2020 Jochen Linnemann, Cory Gill
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

using System.Data.Common;

using CampaignKit.WorldMap.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
    public class TestStartupNoAuth : Startup
    {
        #region Constructors

        public TestStartupNoAuth(IWebHostEnvironment env) : base(env)
        {
        }

        #endregion

        #region Methods

        protected override void ConfigureAuth(IServiceCollection services)
        {
        }

        protected override void ConfigureDb(IServiceCollection services)
        {
            // Create a new service provider.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlite()
                .BuildServiceProvider();

            // Add a database context (MappingContext) using an in-memory 
            // database for testing.
            services.AddDbContext<WorldMapDBContext>(options =>
            {
                options.UseSqlite(CreateInMemoryDatabase());
                options.UseInternalServiceProvider(serviceProvider);
            });

            // Build the service provider.
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database
            // and other services
            using var scope = sp.CreateScope();

            // Get a handle to the service provider
            var scopedServices = scope.ServiceProvider;

            // Get a handle to the database service
            var databaseService = scopedServices.GetRequiredService<WorldMapDBContext>();
            databaseService.Database.Migrate();
        }

        /// <summary>
        ///     Creates the in memory database.
        /// </summary>
        /// <returns>System.Data.Common.DbConnection.</returns>
        private DbConnection CreateInMemoryDatabase()
        {
            var cxn = new SqliteConnection("Filename=:memory:");
            cxn.Open();

            //var cmd = cxn.CreateCommand();
            //cmd.CommandText = @"
            //    CREATE TABLE ""__EFMigrationsHistory"" (
            //        ""MigrationId"" TEXT NOT NULL CONSTRAINT ""PK___EFMigrationsHistory"" PRIMARY KEY,
            //        ""ProductVersion"" TEXT NOT NULL
            //    )";
            //cmd.ExecuteNonQuery();

            return cxn;
        }

        #endregion
    }
}