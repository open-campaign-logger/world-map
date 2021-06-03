// <copyright file="ProcessMapTrigger.cs" company="Jochen Linnemann - IT-Service">
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
using System.Threading.Tasks;

using CampaignKit.WorldMap.Core.Services;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.TileProcessor
{
    public class ProcessTileTrigger
    {
        /// <summary>
        /// The map processing service.
        /// </summary>
        private readonly ITileProcessingService _mapProcessingService;

        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger<ProcessTileTrigger> _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessTileTrigger"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="log">The log.</param>
        /// <param name="mapProcessingService">The map processing service.</param>
        public ProcessTileTrigger(
            IConfiguration configuration, 
            ILogger<ProcessTileTrigger> log,
            ITileProcessingService mapProcessingService)
        {
            this._configuration = configuration;
            this._log = log;
            this._mapProcessingService = mapProcessingService;
        }

        /// <summary>
        /// Executes the function for the specified queue item.
        /// </summary>
        /// <param name="myQueueItem">The queue item.</param>
        [FunctionName("ProcessTileTrigger")]
        public async Task Run([QueueTrigger("worldmapqueue", Connection = "")]string myQueueItem)
        {
            try
            {
                var result = await _mapProcessingService.ProcessTile(myQueueItem);
                if (!result)
                {
                    throw new Exception("Failed to process tile.");
                }
            }
            catch (Exception e)
            {
                _log.LogError("Unable to process tile: {0}", e.Message);
            }
            _log.LogInformation($"ProcessTileTrigger successfully processed tile: {0}", myQueueItem);
        }
    }
}
