// Copyright 2017-2018 Jochen Linnemann
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

using System.IO;

using Microsoft.AspNetCore.Hosting;

namespace CampaignKit.WorldMap.Services
{
    /// <summary>
    ///     Interface IAppDataPathService
    /// </summary>
    public interface IAppDataPathService
    {
        #region Public Properties

        /// <summary>
        ///     Gets the application data path.
        /// </summary>
        /// <value>The application data path.</value>
        string AppDataPath { get; }

        #endregion Public Properties
    }

    /// <inheritdoc />
    /// <summary>
    ///     Class DefaultAppDataPathService.
    /// </summary>
    /// <seealso cref="T:CampaignKit.WorldMap.Services.IAppDataPathService" />
    public class DefaultAppDataPathService : IAppDataPathService
    {
        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultAppDataPathService" /> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public DefaultAppDataPathService(IHostingEnvironment env)
        {
            AppDataPath = Path.Combine(env.ContentRootPath, "App_Data");
        }

        #endregion Public Constructors

        #region IAppDataPathService Members

        #region Public Properties

        /// <inheritdoc />
        /// <summary>
        ///     Gets the application data path.
        /// </summary>
        /// <value>The application data path.</value>
        /// <exception cref="!:NotImplementedException"></exception>
        public string AppDataPath { get; }

        #endregion Public Properties

        #endregion
    }
}