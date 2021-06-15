// <copyright file="CleanupTrigger.cs" company="Jochen Linnemann - IT-Service">
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

using CampaignKit.WorldMap.Core.Services;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace CampaignKit.WorldMap.Function
{
    /// <summary>
    /// Triggers once a day to cleanup processed tile records.
    /// </summary>
    public class CleanupTrigger
    {
        /// The map processing service.
        /// </summary>
        private readonly ITableStorageService _tableStorageService;

        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger<CleanupTrigger> _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupTrigger"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="log">The log.</param>
        /// <param name="tableStorageService">The table storage service.</param>
        public CleanupTrigger(
            IConfiguration configuration,
            ILogger<CleanupTrigger> log,
            ITableStorageService tableStorageService)
        {
            _configuration = configuration;
            _log = log;
            _tableStorageService = tableStorageService;
        }

        /// <summary>
        /// Runs the specified my timer.
        /// </summary>
        /// <param name="myTimer">My timer.</param>
        [FunctionName("CleanupTrigger")]
        public async Task Run([TimerTrigger("0 */30 * * * * ")] TimerInfo myTimer)
        {
            try
            {
                var result = await _tableStorageService.DeleteProcessedTileRecordsAsync(1, 500);
                if (result < 0)
                {
                    throw new Exception("Failed to delete procesed tiles.");
                }
                _log.LogInformation("Successfully delete {0} processed tiles.", result);
            }
            catch (Exception e)
            {
                _log.LogError("Unable to delete processed tiles: {0}", e.Message);
            }
            _log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
