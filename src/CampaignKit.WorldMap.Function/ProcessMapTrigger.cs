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

using CampaignKit.WorldMap.Core.Services;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.TileProcessor
{
    public class ProcessMapTrigger
    {
        /// <summary>
        /// The tile creation service.
        /// </summary>
        private readonly ITileCreationService _tileCreationService;

        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger<ProcessMapTrigger> _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMapTrigger"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="log">The log.</param>
        /// <param name="tileCreationService">The tile creation service.</param>
        public ProcessMapTrigger(
            IConfiguration configuration, 
            ILogger<ProcessMapTrigger> log,
            ITileCreationService tileCreationService)
        {
            this._configuration = configuration;
            this._log = log;
            this._tileCreationService = tileCreationService;
        }

        /// <summary>
        /// Runs the specified my queue item.
        /// </summary>
        /// <param name="myQueueItem">My queue item.</param>
        [FunctionName("ProcessMapTrigger")]
        public void Run([QueueTrigger("worldmapqueue", Connection = "")]string myQueueItem)
        {
            _log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
