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

namespace CampaignKit.WorldMap.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Default Blob storage service.
    /// </summary>
    /// <seealso cref="CampaignKit.WorldMap.Services.IBlobStorageService" />
    public class DefaultBlobStorageService : IBlobStorageService
    {
        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBlobStorageService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="loggerService">The application logger service.</param>
        public DefaultBlobStorageService(IConfiguration configuration, ILogger<DefaultBlobStorageService> loggerService)
        {
            this.configuration = configuration;
            this.loggerService = loggerService;
        }

        /// <summary>
        /// Creates the Azure Blob container asynchronously.
        /// </summary>
        /// <param name="containerName">Unique name of the Azure blob container.</param>
        /// <returns>
        /// True if successful, false otherwise.
        /// </returns>
        public async Task<bool> CreateContainerAsync(string containerName)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(this.configuration.GetConnectionString("AzureBlobStorage"));

            // Create the container and return a container client object
            try
            {
                await blobServiceClient.CreateBlobContainerAsync(containerName, Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            } catch (Azure.RequestFailedException ex)
            {
                this.loggerService.LogError("Unable to create Azure container: {0}.  Error message: {1}.", containerName, ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates the Azure Blob asynchronously.
        /// </summary>
        /// <param name="containerName">Name of the Azure Blob container.</param>
        /// <param name="blobName">Name of the blob to create in the Azure Blob container.</param>
        /// <param name="blob">Byte array containing the blob data.</param>
        /// <returns>
        /// True if successful, false otherwise.
        /// </returns>
        public async Task<bool> CreateBlobAsync(string containerName, string blobName, byte[] blob)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(this.configuration.GetConnectionString("AzureBlobStorage"));

            // Create the container and return a container client object
            try
            {
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = blobContainerClient.GetBlobClient(blobName);
                using (var ms = new MemoryStream(blob, false))
                {
                    await blobClient.UploadAsync(ms);
                }
            }
            catch (Azure.RequestFailedException ex)
            {
                this.loggerService.LogError("Unable to create Azure blob: {0}/{1}.  Error message: {2}.", containerName, blobName, ex.Message);
                return false;
            }

            return true;
        }

    }
}
