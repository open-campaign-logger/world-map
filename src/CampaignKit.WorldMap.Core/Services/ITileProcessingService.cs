// <copyright file="IMapProcessingService.cs" company="Jochen Linnemann - IT-Service">
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

using System.Threading.Tasks;

namespace CampaignKit.WorldMap.Core.Services
{
    /// <summary>
    /// Tile Processing Service Interface
    /// </summary>
    public interface ITileProcessingService
    {
        /// <summary>
        /// Creates a tile for a map.
        /// </summary>
        /// <param name="tileId">The id of the tile to create.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public Task<bool> ProcessTile(string tileId);
    }
}
