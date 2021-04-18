// <copyright file="ITileCreationService.cs" company="Jochen Linnemann - IT-Service">
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

using Azure.Storage.Queues;

using CampaignKit.WorldMap.Core.Entities;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CampaignKit.WorldMap.Core.Services
{
    public class DefaultQueueStorageService : IQueueStorageService
    {
        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger _loggerService;

        public async Task<bool> QueueMapForProcessing(Map map)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            QueueServiceClient queueServiceClient = new QueueServiceClient(_configuration.GetConnectionString("AzureQueueStorage"));

            // Create the container and return a container client object
            try
            {
                var queueClient = queueServiceClient.GetQueueClient("worldmapqueue");
                await queueClient.SendMessageAsync(map.MapId);
            }
            catch (Azure.RequestFailedException ex)
            {
                _loggerService.LogError("Unable to queue map for processing: {0}.  Error message: {2}.", map.MapId, ex.Message);
                return false;
            }

            return true;
        }
    }
}
