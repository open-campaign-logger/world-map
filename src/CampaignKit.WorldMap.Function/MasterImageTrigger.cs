// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
namespace CampaignKit.WorldMap.Function
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Azure.Messaging.EventGrid;
    using CampaignKit.WorldMap.Core.Services;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Function responsible for processing master map files uploaded to blob storage.
    /// see: https://github.com/Azure-Samples/function-image-upload-resize/blob/master/ImageFunctions/Thumbnail.cs .
    /// </summary>
    public class MasterImageTrigger
    {
        /// <summary>
        /// The map processing service.
        /// </summary>
        private readonly IMapProcessingService mapProcessingService;

        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterImageTrigger"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="log">The application logger.</param>
        /// <param name="mapProcessingService">The map processing service.</param>
        public MasterImageTrigger(
            IConfiguration configuration,
            ILogger<ProcessMapTrigger> log,
            IMapProcessingService mapProcessingService)
        {
            this.configuration = configuration;
            this.mapProcessingService = mapProcessingService;
            this.log = log;
        }

        /// <summary>
        /// Runs the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Function("MasterImageTrigger")]
        public async Task Run([EventGridTrigger] EventGridEvent input, FunctionContext context)
        {
            try
            {
                // Validate the event type.
                if (!input.EventType.Equals("Microsoft.Storage.DirectoryCreated"))
                {
                    this.log.LogError($"Unable to process event of type: {input.EventType}");
                    return;
                }

                // Parse the payload.
                var payload = input.Data.ToObjectFromJson<Data>();

                // Validate the payload.
                var subjectPattern = @"(?<=\/map).*";
                var regexMatch = Regex.Match(payload.url, subjectPattern);
                if (!regexMatch.Success)
                {
                    this.log.LogError($"Unable to determine map id from event subject: {input.Subject}");
                    return;
                }

                // Process the map.
                var result = await this.mapProcessingService.ProcessMap(regexMatch.Value);
                if (!result)
                {
                    throw new Exception("Failed to process map.");
                }

                this.log.LogInformation("ProcessMapTrigger successfully processed map: {0}", regexMatch.Value);
            }
            catch (Exception e)
            {
                this.log.LogError("Unable to process map: {0}", e.Message);
            }

        }
    }
}
