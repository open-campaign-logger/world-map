// <copyright file="ImageTrigger.cs" company="Jochen Linnemann - IT-Service">
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
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Azure.Messaging.EventGrid;
    using CampaignKit.WorldMap.Core.Services;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Processes master and zoom level images uploaded to blob storage.
    /// </summary>
    public class ImageTrigger
    {
        /// <summary>
        /// The map processing service.
        /// </summary>
        private readonly IMapProcessingService mapProcessingService;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageTrigger"/> class.
        /// </summary>
        /// <param name="log">The application logger.</param>
        /// <param name="mapProcessingService">The map processing service.</param>
        public ImageTrigger(
            ILogger<ImageTrigger> log,
            IMapProcessingService mapProcessingService)
        {
            this.mapProcessingService = mapProcessingService ?? throw new ArgumentNullException(nameof(mapProcessingService));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        /// <summary>
        /// Processes the EventGridEvent.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Function("ImageTrigger")]
        public async Task Run([EventGridTrigger] EventGridEvent input, FunctionContext context)
        {
            try
            {
                // Validate the event.
                if (!input.EventType.Equals("Microsoft.Storage.BlobCreated"))
                {
                    this.log.LogError($"Unable to process event of type: {input.EventType}");
                    return;
                }

                // Parse the payload.
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var payload = input.Data.ToObjectFromJson<BlobCreatedEventData>(options);

                // Validate the payload.
                var urlPattern = @".*\/map(?<mapId>.*)\/(?<fileName>.*)";
                var regexMatch = Regex.Match(payload.Url, urlPattern);
                if (!regexMatch.Success)
                {
                    this.log.LogError($"Unable to parse url in event data: {payload.Url}");
                    return;
                }

                if (!regexMatch.Groups.ContainsKey("mapId"))
                {
                    this.log.LogError($"Unable to parse map id from url in event data: {payload.Url}");
                    return;
                }

                var mapId = regexMatch.Groups["mapId"].Value;

                if (!regexMatch.Groups.ContainsKey("fileName"))
                {
                    this.log.LogError($"Unable to file name from url in event data: {payload.Url}");
                    return;
                }

                var fileName = regexMatch.Groups["fileName"].Value;

                if (fileName.Contains("master-file", StringComparison.InvariantCultureIgnoreCase))
                {
                    var result = await this.mapProcessingService.ProcessMasterImage(mapId);
                    if (!result)
                    {
                        throw new Exception("Failed to process map.");
                    }
                }
                else if (fileName.Contains("zoom-level.png", StringComparison.InvariantCultureIgnoreCase))
                {
                    var zoomLevelStr = fileName.Split("_")[0].Trim();
                    var zoomLevel = 0;
                    if (!int.TryParse(zoomLevelStr, out zoomLevel))
                    {
                        this.log.LogError($"Unable to parse zoom level from file name: {fileName}");
                    }

                    var result = await this.mapProcessingService.ProcessZoomLevelImage(mapId, zoomLevel);
                    if (!result)
                    {
                        throw new Exception("Failed to process map.");
                    }
                }
                else
                {
                    this.log.LogInformation($"Processing not required for file: {fileName}");
                    return;
                }

                this.log.LogInformation($"MasterImageTrigger successfully processed map image: {payload.Url}");
            }
            catch (Exception e)
            {
                this.log.LogError("Unable to process map: {0}", e.Message);
            }
        }
    }
}
