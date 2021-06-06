using CampaignKit.WorldMap.Core.Entities;
using CampaignKit.WorldMap.Core.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignKit.WorldMap.Tests.MockServices
{
    public class MockQueueStorageService : IQueueStorageService
    {
        public Task<bool> QueueMapForProcessing(Map map)
        {
            throw new NotImplementedException();
        }

        public Task<bool> QueueTileForProcessing(Tile tile)
        {
            throw new NotImplementedException();
        }
    }
}
