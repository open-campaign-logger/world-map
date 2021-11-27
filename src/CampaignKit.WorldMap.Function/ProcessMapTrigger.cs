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

namespace CampaignKit.WorldMap.Function
{
    using System;
    using System.Threading.Tasks;

    using CampaignKit.WorldMap.Core.Services;

    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class ProcessMapTrigger
    {
        /// <summary>
        /// The map processing service.
        /// </summary>
        private readonly IMapProcessingService _mapProcessingService;

        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMapTrigger"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="mapProcessingService">The map processing service.</param>
        public ProcessMapTrigger(
            IConfiguration configuration,
            ILogger<ProcessMapTrigger> log,
            IMapProcessingService mapProcessingService)
        {
            this._configuration = configuration;
            this._mapProcessingService = mapProcessingService;
        }

        /// <summary>
        /// Executes the function for the specified queue item.
        /// </summary>
        /// <param name="myQueueItem">The queue item.</param>
        [Function("ProcessMapTrigger")]
        public async Task Run([QueueTrigger("worldmapqueue", Connection = "ConnectionStrings:AzureQueueStorage")] string myQueueItem, FunctionContext context)
        {
            var logger = context.GetLogger("CampaignKit.WorldMap.Function.ProcessMapTrigger");
            try
            {
                var result = await this._mapProcessingService.ProcessMap(myQueueItem);
                if (!result)
                {
                    throw new Exception("Failed to process map.");
                }

                logger.LogInformation("ProcessMapTrigger successfully processed map: {0}", myQueueItem);
            }
            catch (Exception e)
            {
                logger.LogError("Unable to process map: {0}", e.Message);
            }
        }
    }
}
