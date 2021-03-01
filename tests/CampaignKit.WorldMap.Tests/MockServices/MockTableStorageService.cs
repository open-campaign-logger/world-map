using CampaignKit.WorldMap.Entities;
using CampaignKit.WorldMap.Services;

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
            throw new NotImplementedException();
        }

        public async Task<List<Map>> GetMapRecordsForUserAsync(string userId, bool includePublic)
        {
            return await Task.Run(() => new List<Map>());
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
    }
}
