// <copyright file="IBlobStorageService.cs" company="Jochen Linnemann - IT-Service">
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
    using System.Threading.Tasks;

    /// <summary>
    /// IBlobStorageService interface.
    /// </summary>
    public interface IBlobStorageService
    {
        /// <summary>
        /// Creates the Azure Blob container asynchronously.
        /// </summary>
        /// <param name="containerName">Unique name of the Azure blob container.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public Task<bool> CreateContainerAsync(string containerName);

        /// <summary>
        /// Deletes the Azure Blob container asynchronously.
        /// </summary>
        /// <param name="containerName">Unique name of the Azure blob container.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public Task<bool> DeleteContainerAsync(string containerName);

        /// <summary>
        /// Creates the Azure Blob asynchronously.
        /// </summary>
        /// <param name="containerName">Name of the Azure Blob container.</param>
        /// <param name="blobName">Name of the blob to create in the Azure Blob container.</param>
        /// <param name="blob">Byte array containing the blob data.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public Task<bool> CreateBlobAsync(string containerName, string blobName, byte[] blob);

        /// <summary>
        /// Retrieves the Azure Blob asynchronously.
        /// </summary>
        /// <param name="containerName">Name of the Azure Blob container.</param>
        /// <param name="blobName">Name of the blob to create in the Azure Blob container.</param>
        /// <returns>Byte array containing the blob data.</returns>
        public Task<byte[]> ReadBlobAsync(string containerName, string blobName);
    }
}
