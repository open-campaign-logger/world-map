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

namespace CampaignKit.WorldMap.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// IBlobStorageService interface.
    /// </summary>
    public interface IBlobStorageService
    {
        /// <summary>
        /// Deletes the Azure Blob folder asynchronously.
        /// </summary>
        /// <param name="folderName">Unique name of the Azure blob folder.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public Task<bool> DeleteFolderAsync(string folderName);

        /// <summary>
        /// Checks if the folder exists.
        /// </summary>
        /// <param name="folderName">Unique name of the Azure blob folder.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public Task<bool> FolderExistsAsync(string folderName);

        /// <summary>
        /// Gets a listing of folder contents asynchronously.
        /// </summary>
        /// <param name="folderName">Unique name of the Azure blob folder.</param>
        /// <returns>
        /// Number of files.
        /// </returns>
        public Task<List<String>> ListFolderContentsAsync(string folderName);

        /// <summary>
        /// Checks if the blob exists.
        /// </summary>
        /// <param name="folderName">Unique name of the Azure blob folder.</param>
        /// <param name="blobName">Unique name of the Azure blob.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public Task<bool> BlobExistsAsync(string folderName, string blobName);

        /// <summary>
        /// Creates the Azure Blob asynchronously.
        /// </summary>
        /// <param name="folderName">Name of the Azure Blob folder.</param>
        /// <param name="blobName">Name of the blob to create in the Azure Blob folder.</param>
        /// <param name="blob">Byte array containing the blob data.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public Task<bool> CreateBlobAsync(string folderName, string blobName, byte[] blob);

        /// <summary>
        /// Retrieves the Azure Blob asynchronously.
        /// </summary>
        /// <param name="folderName">Name of the Azure Blob folder.</param>
        /// <param name="blobName">Name of the blob to create in the Azure Blob folder.</param>
        /// <returns>Byte array containing the blob data.</returns>
        public Task<byte[]> ReadBlobAsync(string folderName, string blobName);
    }
}
