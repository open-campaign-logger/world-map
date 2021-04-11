using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.TileProcessor
{
    public static class ProcessTile
    {
        [FunctionName("ProcessTile")]
        public static void Run([QueueTrigger("worldmapqueue", Connection = "")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
