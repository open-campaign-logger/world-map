// Copyright 2017-2020 Jochen Linnemann, Cory Gill
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
using System.Threading.Tasks;
using CampaignKit.WorldMap.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampaignKit.WorldMap.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Main MVC controller for application.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    [Route("")]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HomeController" /> class.
        /// </summary>
        /// <param name="mapDataService">The map data service.</param>
        public HomeController(IMapRepository mapDataService)
        {
            _mapRepository = mapDataService;
        }

        #endregion

        #region Fields

        /// <summary>
        ///     The EntityFramework repository for Map data elements.
        /// </summary>
        private readonly IMapRepository _mapRepository;

        #endregion

        #region Methods

        /// <summary>
        ///     GET: /
        /// </summary>
        /// <returns>Home view showing last three created maps.</returns>
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            // Retrieve a listing of maps for this user.  
            // Anonymous User: all public maps
            // Authenticated User: all public and owned maps.
            var model = (await _mapRepository.FindAll(User, true))
                .Where(m => m.MapId != 1)
                .OrderByDescending(m => m.CreationTimestamp)
                .Take(3);

            return View(model);
        }

        /// <summary>
        ///     This action is called via an Ajax call with the
        ///     JWT bearer details in the request header.  The action itself
        ///     does nothing but the middleware will intercept the
        ///     JWT authorization token in the request header and create a
        ///     new client side cookie containing the JWT auth token so that
        ///     subsequent form submits will be able to send in the auth details
        ///     automatically.
        /// </summary>
        /// <returns></returns>
        [HttpGet("JwtCookie")]
        [Authorize]
        public ActionResult JwtCookie()
        {
            return View();
        }

        /// <summary>
        ///     GET: /Home/Legalities
        /// </summary>
        /// <returns>Application legalities view.</returns>
        [HttpGet("Legalities")]
        public ActionResult Legalities()
        {
            return View();
        }

        #endregion
    }
}