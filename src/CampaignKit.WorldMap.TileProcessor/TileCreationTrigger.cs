using System;

using CampaignKit.WorldMap.TileProcessor.Services;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.TileProcessor
{
    public class TileCreationTrigger
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TileCreationTrigger> _log;

        public TileCreationTrigger(IBlobStorageService blobStorageService, IConfiguration configuration, ILogger<TileCreationTrigger> log)
        {
            this._blobStorageService = blobStorageService;
            this._configuration = configuration;
        }

        [FunctionName("TileCreationTrigger")]
        public void Run([QueueTrigger("worldmapqueue", Connection = "")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
