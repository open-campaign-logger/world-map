using CampaignKit.WorldMap.Core.Services;

using System;
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

        public Task<byte[]> ReadBlobAsync(string folderName, string blobName)
        {
            throw new NotImplementedException();
        }
    }
}
