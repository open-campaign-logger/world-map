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

using System.Collections.Generic;
using System.IO;
using System.Linq;

using CampaignKit.WorldMap.Entities;

using Newtonsoft.Json;

namespace CampaignKit.WorldMap.Services
{
    /// <summary>
    ///     Interface IMapDataService
    /// </summary>
    public interface IMapDataService
    {
        #region Public Methods

        /// <summary>
        ///     Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(string id);

        /// <summary>
        ///     Finds the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Map.</returns>
        Map Find(string id);

        /// <summary>
        ///     Finds all maps.
        /// </summary>
        /// <returns>IEnumerable&lt;Map&gt;.</returns>
        IEnumerable<Map> FindAll();

        /// <summary>
        ///     Saves the specified map.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Save(Map map);

        #endregion Public Methods
    }

    /// <inheritdoc />
    /// <summary>
    ///     Class DefaultMapDataService.
    /// </summary>
    /// <seealso cref="T:CampaignKit.WorldMap.Services.IMapDataService" />
    public class DefaultMapDataService : IMapDataService
    {
        #region Private Fields

        private readonly string _appDataPath;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMapDataService" /> class.
        /// </summary>
        /// <param name="appDataPathService">The application data path service.</param>
        public DefaultMapDataService(IAppDataPathService appDataPathService)
        {
            _appDataPath = appDataPathService.AppDataPath;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <inheritdoc />
        /// <summary>
        ///     Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(string id)
        {
            var mapFilePath = GetMapFilePath(id);
            if (File.Exists(mapFilePath)) File.Delete(mapFilePath);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Finds the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Map.</returns>
        public Map Find(string id)
        {
            var mapFilePath = GetMapFilePath(id);

            if (!File.Exists(mapFilePath)) return null;

            var map = JsonConvert.DeserializeObject<Map>(File.ReadAllText(mapFilePath));

            return map;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Finds all maps.
        /// </summary>
        /// <returns>IEnumerable&lt;Map&gt;.</returns>
        public IEnumerable<Map> FindAll()
        {
            var allMapFilePaths = GetAllMapFilePaths();

            return allMapFilePaths.Select(p => JsonConvert.DeserializeObject<Map>(File.ReadAllText(p)));
        }

        /// <inheritdoc />
        /// <summary>
        ///     Saves the specified map.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Save(Map map)
        {
            var mapFilePath = GetMapFilePath($"{map.Id}");

            var json = JsonConvert.SerializeObject(map);
            File.WriteAllText(mapFilePath, json);

            return true;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Gets all map file paths.
        /// </summary>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        private IEnumerable<string> GetAllMapFilePaths()
        {
            return Directory.EnumerateFiles(_appDataPath, "world-*.json");
        }

        /// <summary>
        ///     Gets the map file path.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.String.</returns>
        private string GetMapFilePath(string id)
        {
            return Path.Combine(_appDataPath, $"world-{id}.json");
        }

        #endregion Private Methods
    }
}