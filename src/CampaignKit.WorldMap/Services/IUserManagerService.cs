// <copyright file="IUserManagerService.cs" company="Jochen Linnemann - IT-Service">
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
    using System.Security.Claims;

    /// <summary>
    ///     Interface IUserManagerService.
    /// </summary>
    public interface IUserManagerService
    {
        /// <summary>
        ///     Derives the user's userId from the list of their claims.
        /// </summary>
        /// <param name="user">The authorized user.</param>
        /// <returns>UserId (String) if found otherwise Null.</returns>
        string GetUserId(ClaimsPrincipal user);
    }
}
