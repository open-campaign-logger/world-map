// Copyright 2017-2019 Jochen Linnemann, Cory Gill
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


using System.Linq;
using System.Security.Claims;

namespace CampaignKit.WorldMap.Services
{
    /// <summary>
    ///     Interface IUserManagerService
    /// </summary>
    public interface IUserManagerService
    {
        #region Methods

        /// <summary>
        ///     Derives the user's userId from the list of their claims.
        /// </summary>
        /// <param name="user">The authorized user.</param>
        /// <returns>UserId (String) if found otherwise Null.</returns>
        string GetUserId(ClaimsPrincipal user);

        #endregion
    }

    /// <inheritdoc />
    /// <summary>
    ///     Class DefaultUserManagerService.
    /// </summary>
    /// <seealso cref="T:CampaignKit.WorldMap.Services.IUserManagerService" />
    public class DefaultUserManagerService : IUserManagerService
    {
        #region Implementations

        /// <summary>
        ///     Derives the user's userId from the list of their claims.
        /// </summary>
        /// <param name="user">The authorized user.</param>
        /// <returns>UserId (String) if found otherwise Null.</returns>
        public string GetUserId(ClaimsPrincipal user)
        {
            if (user == null)
                return null;
            if (user.Claims.Count(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")) == 0)
                return null;

            return user.Claims.First(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value;
        }

        #endregion
    }
}