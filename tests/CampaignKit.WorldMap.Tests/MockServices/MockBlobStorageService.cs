using CampaignKit.WorldMap.Core.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampaignKit.WorldMap.Tests.MockServices
{
    public class MockBlobStorageService : IBlobStorageService
    {
        public Task<bool> BlobExistsAsync(string folderName, string blobName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateBlobAsync(string folderName, string blobName, byte[] blob)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteFolderAsync(string folderName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FolderExistsAsync(string folderName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> ListFolderContentsAsync(string folderName)
        {
            var results = new List<String>
            {
                "master-image.png",
            };
            return await Task.Run(() => results);
        }

        public Task<byte[]> ReadBlobAsync(string folderName, string blobName)
        {
            throw new NotImplementedException();
        }
    }
}
