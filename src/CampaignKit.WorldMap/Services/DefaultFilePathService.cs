// <copyright file="DefaultFilePathService.cs" company="Jochen Linnemann - IT-Service">
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

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.Services
{
    /// <summary>
    /// The default file path service.
    /// </summary>
    /// <seealso cref="CampaignKit.WorldMap.Services.IFilePathService" />
    public class DefaultFilePathService : IFilePathService
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultFilePathService" /> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public DefaultFilePathService(IWebHostEnvironment env)
        {
            this.AppDataPath = Path.Combine(env.ContentRootPath, "App_Data");
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the application data path.
        /// </summary>
        /// <value>The application data path.</value>
        public string AppDataPath { get; }
    }
}
