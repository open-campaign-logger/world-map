﻿// Copyright 2017-2019 Jochen Linnemann, Cory Gill
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

using Microsoft.Extensions.Hosting;

namespace CampaignKit.WorldMap.Services
{
    /// <summary>
    ///     Interface IFilePathService
    /// </summary>
    public interface IFilePathService
    {
        #region Properties

        /// <summary>
        ///     Gets the application data path.
        /// </summary>
        /// <value>The application data path.</value>
        string AppDataPath { get; }

        /// <summary>
        ///     Gets the world base path.
        /// </summary>
        /// <value>The world base path.</value>
        string PhysicalWorldBasePath { get; }

        /// <summary>
        ///     Gets the seed data path.
        /// </summary>
        /// <value>The seed data path.</value>
        string SeedDataPath { get; }

        /// <summary>
        ///     Gets the virtual world base path.
        /// </summary>
        /// <value>The virtual world base path.</value>
        string VirtualWorldBasePath { get; }

        #endregion
    }

    /// <inheritdoc />
    /// <summary>
    ///     Class DefaultAppDataPathService.
    /// </summary>
    /// <seealso cref="T:CampaignKit.WorldMap.Services.IAppDataPathService" />
    public class DefaultFilePathService : IFilePathService
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultFilePathService" /> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public DefaultFilePathService(IHostEnvironment env)
        {
            AppDataPath = Path.Combine(env.ContentRootPath, "App_Data");
            SeedDataPath = Path.Combine(AppDataPath, "Sample");
            PhysicalWorldBasePath = Path.Combine(env.ContentRootPath, "world");
            VirtualWorldBasePath = "~/world";
        }

        #endregion

        #region IAppDataPathService Members

        #region Public Properties

        /// <inheritdoc />
        /// <summary>
        ///     Gets the application data path.
        /// </summary>
        /// <value>The application data path.</value>
        /// <exception cref="!:NotImplementedException"></exception>
        public string AppDataPath { get; }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the world base path.
        /// </summary>
        /// <value>The world base path.</value>
        public string PhysicalWorldBasePath { get; }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the virtual world base path.
        /// </summary>
        /// <value>The virtual world base path.</value>
        public string VirtualWorldBasePath { get; }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the seed data path.
        /// </summary>
        /// <value>The seed data path.</value>
        public string SeedDataPath { get; }

        #endregion Public Properties

        #endregion
    }
}