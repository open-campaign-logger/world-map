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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CampaignKit.WorldMap.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.Services
{
	public class TileCreationService : IHostedService, IDisposable
	{
		// Private Members
		private readonly ILogger _logger;
		private Timer _timer;
		private MappingContext _context;

		// Create collection for tile creation tasks
		List<Task<Tile>> tasks;

		// Public properties
		IServiceProvider ServiceProvider { get; }

		// Default Constructor
		public TileCreationService(IServiceProvider serviceProvider, ILogger<TileCreationService> logger)
		{
			_logger = logger;
			ServiceProvider = serviceProvider;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Timed Background Service is starting.");

			_timer = new Timer(DoWork, null, TimeSpan.Zero,
				TimeSpan.FromSeconds(5));

			return Task.CompletedTask;
		}

		private void DoWork(object state)
		{
			_logger.LogInformation("Timed Background Service is working.");

			using (IServiceScope scope = ServiceProvider.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<MappingContext>();

				dbContext.Database.OpenConnection();

				var tiles = from t in dbContext.Tiles select t;
				tiles.Where(t => t.CompletionTimestamp != null);
				var tileList = tiles.ToList();

				_logger.LogInformation($"{0} Tiles found.", tileList.Count);

			}

		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Timed Background Service is stopping.");

			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}
