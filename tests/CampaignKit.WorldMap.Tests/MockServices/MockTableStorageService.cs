using CampaignKit.WorldMap.Core.Entities;
using CampaignKit.WorldMap.Core.Services;
using CampaignKit.WorldMap.Tests.Infrastructure;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampaignKit.WorldMap.Tests.MockServices
{
    public class MockTableStorageService : ITableStorageService
    {
        public Task<string> CreateMapRecordAsync(Map map)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMapRecordAsync(Map map)
        {
            throw new NotImplementedException();
        }

        public Task<Map> GetMapRecordAsync(string mapId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Map>> GetMapRecordsForUserAsync(string userId, bool includePublic)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMapRecordAsync(Map map)
        {
            throw new NotImplementedException();
        }

        private Map GetSampleMap()
        {
            return new Map()
            {
                PartitionKey = TestAuthenticationOptions.TEST_ID,
                MapId = "sample",
                AdjustedSize = 4000,
                ContentType = "image/png",
                Copyright = "Copyright 2017 Jochen Linnemann ",
                FileExtension = ".png",
                MaxZoomLevel = 4,
                Name = "Sample",
                RepeatMapInX = false,
                UserId = TestAuthenticationOptions.TEST_ID,
                WorldFolderPath = "C:\\Users\\mf1939\\source\\repos\\open-campaign-logger\\world-map\\src\\CampaignKit.WorldMap\\wwwroot\\world\\1",
                ThumbnailPath = "~/world/1/0/zoom-level.png",
                MarkerData = "[{ \"options\": { }, \"properties\": { } ] ",
                ShareKey = "lNtqjEVQ",
                IsPublic = true,
            };
        }
    }
}
