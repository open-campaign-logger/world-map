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

using CampaignKit.WorldMap.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
    public class TestStartupNoAuth : Startup
    {
        #region Constructors

        public TestStartupNoAuth(IHostingEnvironment env) : base(env)
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
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Add a database context (MappingContext) using an in-memory 
            // database for testing.
            services.AddDbContext<WorldMapDBContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
                options.UseInternalServiceProvider(serviceProvider);
            });

            // Build the service provider.
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database
            // and other services
            using (var scope = sp.CreateScope())
            {
                // Get a handle to the service provider
                var scopedServices = scope.ServiceProvider;

                // Get a handle to the database service
                var databaseService = scopedServices.GetRequiredService<WorldMapDBContext>();
                databaseService.Database.EnsureCreated();
            }
        }

        #endregion
    }
}