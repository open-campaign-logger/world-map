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

using System.Linq;

using CampaignKit.WorldMap.Services;

using Microsoft.AspNetCore.Mvc;

namespace CampaignKit.WorldMap.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Class HomeController.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    public class HomeController : Controller
    {
        #region Private Fields

        private readonly IMapDataService _mapDataService;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HomeController" /> class.
        /// </summary>
        /// <param name="mapDataService">The map data service.</param>
        public HomeController(IMapDataService mapDataService)
        {
            _mapDataService = mapDataService;
        }

        #endregion Public Constructors

        #region Public Methods

        public IActionResult Index()
        {
            var model = _mapDataService.FindAll().OrderByDescending(m => m.CreationTimestamp).Take(3);

            return View(model);
        }

        public ActionResult Legalities()
        {
            return View();
        }

        #endregion Public Methods
    }
}