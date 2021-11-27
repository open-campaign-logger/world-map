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

namespace CampaignKit.WorldMap.Function
{
    using System;
    using System.Threading.Tasks;

    using CampaignKit.WorldMap.Core.Services;

    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Main program.
    /// </summary>
    public class CleanupTrigger
    {
        /// <summary>
        /// The map processing service.
        /// </summary>
        private readonly ITableStorageService _tableStorageService;

        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

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
            this._configuration = configuration;
            this._tableStorageService = tableStorageService;
        }

        /// <summary>
        /// Runs the specified my timer according to the specified CRON schedule.
        /// see: https://arminreiter.com/2017/02/azure-functions-time-trigger-cron-cheat-sheet/
        /// </summary>
        /// <param name="myTimer">The time to execute..</param>
        /// <param name="context">The context that the timer will execute in.</param>
        /// <exception cref="System.Exception">Failed to delete procesed tiles.</exception>
        /// <returns>A <see cref="Task"/> A Task representing the asynchronous operation.</returns>
        [Function("CleanupTrigger")]
        public async Task Run([TimerTrigger("0 0 0 * * *")] MyInfo myTimer, FunctionContext context)
        {
            var logger = context.GetLogger("CampaignKit.WorldMap.Function.CleanupTrigger");
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            try
            {
                var result = await this._tableStorageService.DeleteProcessedTileRecordsAsync(1, 500);
                if (result < 0)
                {
                    throw new Exception("Failed to delete procesed tiles.");
                }

                logger.LogInformation("Successfully delete {0} processed tiles.", result);
            }
            catch (Exception e)
            {
                logger.LogError("Unable to delete processed tiles: {0}", e.Message);
            }

            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
