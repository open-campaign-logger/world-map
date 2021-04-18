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

        public Task<string> CreateTileRecordAsync(Tile tile)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMapRecordAsync(Map map)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTileRecordAsync(Tile tile)
        {
            throw new NotImplementedException();
        }

        public Task<Map> GetMapRecordAsync(string mapId)
        {
            return Task.Run(() => this.GetSampleMap());

        }

        public async Task<List<Map>> GetMapRecordsForUserAsync(string userId, bool includePublic)
        {
            var userMapList = new List<Map>()
            {
                this.GetSampleMap(),
            };
            return await Task.Run(() => userMapList);
        }

        public Task<Tile> GetTileRecordAsync(string tileId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tile>> GetUnprocessedTileRecordsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMapRecordAsync(Map map)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTileRecordAsync(Tile tile)
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
                CreationTimestamp = DateTime.Today.AddDays(-2),
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
                Tiles = new List<Tile>(),
            };
        }
    }
}
