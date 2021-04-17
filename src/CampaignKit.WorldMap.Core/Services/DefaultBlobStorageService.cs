// <copyright file="DefaultBlobStorageService.cs" company="Jochen Linnemann - IT-Service">
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

using System;
using System.IO;
using System.Threading.Tasks;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.Core
{
    /// <summary>
    /// Default Blob storage service.
    /// </summary>
    /// <seealso cref="CampaignKit.WorldMap.Services.IBlobStorageService" />
    public class DefaultBlobStorageService : IBlobStorageService
    {
        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBlobStorageService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="loggerService">The logger service.</param>
        public DefaultBlobStorageService(IConfiguration configuration, ILogger<DefaultBlobStorageService> loggerService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
        }

        /// <summary>
        /// Creates the Azure Blob asynchronously.
        /// </summary>
        /// <param name="folderName">Name of the Azure Blob folder.</param>
        /// <param name="blobName">Name of the blob to create in the Azure Blob container.</param>
        /// <param name="blob">Byte array containing the blob data.</param>
        /// <returns>
        /// True if successful, false otherwise.
        /// </returns>
        public async Task<bool> CreateBlobAsync(string folderName, string blobName, byte[] blob)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("AzureBlobStorage"));

            // Create the container and return a container client object
            try
            {
                var blobContainerClient = blobServiceClient.GetBlobContainerClient("world-map");
                var blobClient = blobContainerClient.GetBlobClient($"{folderName}/{blobName}");
                using (var ms = new MemoryStream(blob, false))
                {
                    await blobClient.UploadAsync(ms);
                }
            }
            catch (Azure.RequestFailedException ex)
            {
                _loggerService.LogError("Unable to create blob: {0}/{1}.  Error message: {2}.", folderName, blobName, ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Retrieves the Azure Blob asynchronously.
        /// </summary>
        /// <param name="folderName">Name of the Azure Blob folder.</param>
        /// <param name="blobName">Name of the blob to create in the Azure Blob container.</param>
        /// <returns>
        /// Byte array containing the blob data.
        /// </returns>
        public async Task<byte[]> ReadBlobAsync(string folderName, string blobName)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("AzureBlobStorage"));

            // Create the container and return a container client object
            try
            {
                var blobContainerClient = blobServiceClient.GetBlobContainerClient("world-map");
                var blobClient = blobContainerClient.GetBlobClient($"{folderName}/{blobName}");
                using (var ms = new MemoryStream())
                {
                    await blobClient.DownloadToAsync(ms);
                    return ms.ToArray();
                }
            }
            catch (Azure.RequestFailedException ex)
            {
                _loggerService.LogError("Unable to read blob: {0}/{1}.  Error message: {2}.", folderName, blobName, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Deletes the Azure Blob folder asynchronously.
        /// </summary>
        /// <param name="folderName">Unique name of the Azure blob folder.</param>
        /// <returns>
        /// True if successful, false otherwise.
        /// </returns>
        public async Task<bool> DeleteFolderAsync(string folderName)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            var blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("AzureBlobStorage"));

            // Create the container and return a container client object
            try
            {
                var blobContainerClient = blobServiceClient.GetBlobContainerClient("world-map");

                // Call the listing operation and return pages of the specified size.
                var resultSegment = blobContainerClient.GetBlobsAsync(prefix: folderName).AsPages(default, 500);

                // Enumerate the blobs returned for each page.
                await foreach (Azure.Page<BlobItem> blobPage in resultSegment)
                {
                    foreach (BlobItem blobItem in blobPage.Values)
                    {
                        await blobContainerClient.DeleteBlobAsync(blobItem.Name);
                    }
                }
            }
            catch (Azure.RequestFailedException ex)
            {
                _loggerService.LogError("Unable to delete Azure container: {0}.  Error message: {1}.", folderName, ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the folder exists.
        /// </summary>
        /// <param name="folderName">Unique name of the Azure blob folder.</param>
        /// <returns>
        /// True if successful, false otherwise.
        /// </returns>
        public async Task<bool> FolderExistsAsync(string folderName)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("AzureBlobStorage"));

            // Create the container and return a container client object
            try
            {
                var blobContainerClient = blobServiceClient.GetBlobContainerClient("world-map");
                var blobClient = blobContainerClient.GetBlobClient($"{folderName}");
                return await blobClient.ExistsAsync();
            }
            catch (Azure.RequestFailedException ex)
            {
                _loggerService.LogError("Unable to create Azure container: {0}.  Error message: {1}.", folderName, ex.Message);
                return false;
            }
        }
    }
}
