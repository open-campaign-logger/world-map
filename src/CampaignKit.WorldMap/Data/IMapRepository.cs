// <copyright file="IMapRepository.cs" company="Jochen Linnemann - IT-Service">
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

namespace CampaignKit.WorldMap.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CampaignKit.WorldMap.Entities;
    using CampaignKit.WorldMap.Services;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;

    /// <summary>
    ///     EntityFramework interface for <c>Map</c> data elements.
    /// </summary>
    public interface IMapRepository
    {
        /// <summary>Determines if user has rights to edit the map.</summary>
        /// <param name="id">The map identifier.</param>
        /// <param name="user">The authenticated user.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        Task<bool> CanEdit(int id, ClaimsPrincipal user);

        /// <summary>Determines if user has rights to view the map.</summary>
        /// <param name="id">The map identifier.</param>
        /// <param name="user">The authenticated user.</param>
        /// <param name="shareKey">The map's secret key.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        Task<bool> CanView(int id, ClaimsPrincipal user, string shareKey);

        /// <summary> Creates the specified map. </summary>
        /// <param name="map">The map entity to create.</param>
        /// <param name="stream">Map image data stream.</param>
        /// <param name="user">The authenticated user.</param>
        /// <returns><c>id</c> if successful, <c>0</c> otherwise.</returns>
        Task<int> Create(Map map, Stream stream, ClaimsPrincipal user);

        /// <summary>Deletes the specified map and all child entities.</summary>
        /// <param name="id">The map identifier.</param>
        /// <param name="user">The authenticated user.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        Task<bool> Delete(int id, ClaimsPrincipal user);

        /// <summary>Finds the map associated with the identifier.</summary>
        /// <param name="id">The map identifier.</param>
        /// <param name="user">The authenticated user.</param>
        /// <param name="shareKey">Map secret to be used by friends of map author.</param>
        /// <returns><c>Map</c> if successful, <c>null</c> otherwise.</returns>
        Task<Map> Find(int id, ClaimsPrincipal user, string shareKey);

        /// <summary>Finds all maps.</summary>
        /// <param name="user">The authenticated user.</param>
        /// <param name="includePublic">Specify whether public maps should also be returned.</param>
        /// <returns>IEnumerable&lt;Map&gt;.</returns>
        Task<IEnumerable<Map>> FindAll(ClaimsPrincipal user, bool includePublic);

        /// <summary> Saves changes to the specified map.</summary>
        /// <param name="map">The map entity to save.</param>
        /// <param name="user">The authenticated user.</param>
        /// <returns><c>id</c> if successful, <c>false</c> otherwise.</returns>
        Task<bool> Save(Map map, ClaimsPrincipal user);
    }
}
