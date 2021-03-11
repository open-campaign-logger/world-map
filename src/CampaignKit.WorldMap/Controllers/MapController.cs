// <copyright file="MapController.cs" company="Jochen Linnemann - IT-Service">
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
using System.Linq;
using System.Threading.Tasks;

using CampaignKit.WorldMap.Data;
using CampaignKit.WorldMap.Entities;
using CampaignKit.WorldMap.Services;
using CampaignKit.WorldMap.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Map MVC controller for application.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    public class MapController : Controller
    {
        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The application logging service.
        /// </summary>
        private readonly ILogger _loggerService;

        /// <summary>
        ///     The EntityFramework repository for Map data elements.
        /// </summary>
        private readonly IMapRepository _mapRepository;

        /// <summary>
        ///     The progress service.
        /// </summary>
        private readonly IProgressService _progressService;

        /// <summary>
        ///     The random data service.
        /// </summary>
        private readonly IRandomDataService _randomDataService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MapController" /> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="loggerService">The logger service.</param>
        /// <param name="randomDataService">The random data service.</param>
        /// <param name="mapRepository">The map repository.</param>
        /// <param name="progressService">The progress service.</param>
        public MapController(
            IConfiguration configuration,
            ILogger<MapController> loggerService,
            IRandomDataService randomDataService,
            IMapRepository mapRepository,
            IProgressService progressService)
        {
            _configuration = configuration;
            _loggerService = loggerService;
            _randomDataService = randomDataService;
            _mapRepository = mapRepository;
            _progressService = progressService;
        }

        /// <summary>
        ///     GET: /Map/Create.
        /// </summary>
        /// <returns>Map creation view containing new randomly generated secret.</returns>
        [HttpGet("Create")]
        [Authorize]
        public IActionResult Create()
        {
            _loggerService.LogDebug("Enter GET Map/Create");

            var model = new MapCreateViewModel { Share = _randomDataService.GetRandomText(8) };
            return View(model);
        }

        /// <summary>
        ///     POST: /Map/Create.
        /// </summary>
        /// <param name="model">The map model to create.</param>
        /// <returns>Map view with tile progress creation window displayed.</returns>
        [HttpPost("Create")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MapCreateViewModel model)
        {
            _loggerService.LogDebug("Enter POST Map/Create");

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!model.ThisIsMyOwnCreationPublishedRightfully)
            {
                ModelState.AddModelError(
                    nameof(model.ThisIsMyOwnCreationPublishedRightfully),
                    "You have to confirm that this map is your creation and that your are publishing it rightfully.");
            }

            if (!model.ThisIsNotOffensiveNorObviouslyIllegalContent)
            {
                ModelState.AddModelError(
                    nameof(model.ThisIsNotOffensiveNorObviouslyIllegalContent),
                    "You have to confirm that this map image is not offensive nor obviously illegal.");
            }

            if (!model.ProcessingSavingPublishingRightsGrantedForThisSite)
            {
                ModelState.AddModelError(
                    nameof(model.ProcessingSavingPublishingRightsGrantedForThisSite),
                    "You have to allow us to process, save, and publish your image on this site.");
            }

            if (!model.ThisIsMyOwnCreationPublishedRightfully ||
                !model.ThisIsNotOffensiveNorObviouslyIllegalContent ||
                !model.ProcessingSavingPublishingRightsGrantedForThisSite)
            {
                return View();
            }

            var map = new Map
            {
                Name = model.Name,
                Copyright = model.Copyright,
                ContentType = model.Image.ContentType,
                FileExtension = Path.GetExtension(model.Image.FileName ?? string.Empty).ToLower(),
                RepeatMapInX = model.RepeatMapInX,
                IsPublic = model.IsPublic,
                ShareKey = model.Share,
                MarkerData = string.Empty,
            };

            var id = await _mapRepository.Create(map, model.Image.OpenReadStream(), User);
            if (id is null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your map could not be saved. Please try again.");
            }
            else
            {
                return RedirectToAction(nameof(Show), new { id, map.ShareKey, ShowProgress = true });
            }

            return View();
        }

        /// <summary>
        ///     GET: /Map/Delete/{id}.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Delete view displaying confirmation popup.</returns>
        [HttpGet("Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await _mapRepository.Find(id, User, string.Empty);

            return model == null
                ? DeleteErrorView()
                : View(new MapDeleteViewModel { Name = model.Name, Id = model.MapId });
        }

        /// <summary>
        ///     POST: /Map/Delete/{id}.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">Unused.</param>
        /// <returns>Redirect to home view.</returns>
        [HttpPost("Delete/{id}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, MapDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var map = await _mapRepository.Delete(id, User);

            if (!map)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your map could not be deleted. Please try again.");
            }
            else
            {
                await _mapRepository.Delete(id, User);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        /// <summary>
        ///     GET: /Map/Edit/{id}.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Map edit view for the specified map.</returns>
        [HttpGet("Edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            // Determine if user has rights to edit this map
            var canEdit = await _mapRepository.CanEdit(id, User);
            if (!canEdit)
            {
                return EditErrorView();
            }

            // Load model
            var map = await _mapRepository.Find(id, User, string.Empty);
            if (map == null)
            {
                return EditErrorView();
            }

            // Return edit screen
            var protocol = Request.IsHttps ? "https" : "http";
            return View(new MapEditViewModel
            {
                Name = map.Name,
                Copyright = map.Copyright,
                RepeatMapInX = map.RepeatMapInX,
                MakeMapPublic = map.IsPublic,
                ShowUrl = Url.Action(nameof(Show), "Map", new { Id = id, map.ShareKey }, protocol, Request.Host.Value),
            });
        }

        /// <summary>
        ///     POST: /Map/Edit/{id}.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>Map show view.</returns>
        [HttpPost("Edit/{id}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, MapEditViewModel model)
        {
            // Determine if provided data is valid
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Determine if user has rights to edit this map
            var canEdit = await _mapRepository.CanEdit(id, User);
            if (!canEdit)
            {
                return EditErrorView();
            }

            // Load model
            var map = await _mapRepository.Find(id, User, string.Empty);
            if (map == null)
            {
                return EditErrorView();
            }

            map.Name = model.Name;
            map.Copyright = model.Copyright;
            map.RepeatMapInX = model.RepeatMapInX;
            map.IsPublic = model.MakeMapPublic;

            var result = await _mapRepository.Save(map, User);
            if (!result)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your map could not be saved. Please try again.");
            }
            else
            {
                return RedirectToAction(nameof(Show), new { model.Id });
            }

            return View();
        }

        /// <summary>
        ///     GET: /Map/.
        /// </summary>
        /// <returns>Show all user maps.</returns>
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var model = await _mapRepository.FindAll(User, true);
            model = model.OrderByDescending(m => m.CreationTimestamp);
            return View(model);
        }

        /// <summary>
        ///     GET: /Map/Progress/{id}.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Tile creation progress for map in JSON format.</returns>
        [HttpGet("Progress/{id}")]
        [Authorize]
        public async Task<IActionResult> Progress(string id)
        {
            var json = Json(new { Progress = await _progressService.GetMapProgress(id) });
            return json;
        }

        /// <summary>
        ///     GET: /Map/Sample.
        /// </summary>
        /// <returns>Map sample view.</returns>
        [HttpGet("Sample")]
        public async Task<IActionResult> Sample()
        {
            var map = await _mapRepository.Find("sample", User, string.Empty);

            if (map == null)
            {
                return ShowErrorView();
            }

            var model = new MapShowViewModel
            {
                Name = map.Name,
                Id = map.MapId,
                UserId = map.UserId,
            };

            ViewBag.MaxZoomLevel = map.MaxZoomLevel;
            ViewBag.WorldPath = map.WorldFolderPath;
            ViewBag.NoWrap = !map.RepeatMapInX;

            return View(model);
        }

        /// <summary>
        ///     GET: /Map/Show/{id}.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="share">The share key.</param>
        /// <param name="showProgress">if set to <c>true</c> [show progress].</param>
        /// <returns>The selected map.</returns>
        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id, string share = null, bool showProgress = false)
        {
            // Determine if user can view map
            var canView = await _mapRepository.CanView(id, User, share);
            if (!canView)
            {
                return ShowErrorView();
            }

            // Determine if user can edit map
            var canEdit = await _mapRepository.CanEdit(id, User);

            // Retrieve the map
            var map = await _mapRepository.Find(id, User, share);

            // Create a view model
            var protocol = Request.IsHttps ? "https" : "http";
            var model = new MapShowViewModel
            {
                Name = map.Name,
                Share = share,
                UserId = map.UserId,
                ShowProgress = showProgress,
                ProgressUrl = Url.Action(nameof(Progress), new { Id = id }),
                ShowUrl = Url.Action(nameof(Show), "Map", new { Id = id, Share = share }, protocol, Request.Host.Value),
                DeleteUrl = Url.Action(nameof(Delete), "Map", new { Id = id }, protocol, Request.Host.Value),
                EditUrl = Url.Action(nameof(Edit), "Map", new { Id = id }, protocol, Request.Host.Value),
                Id = id,
                CanEdit = canEdit,
            };

            ViewBag.MaxZoomLevel = map.MaxZoomLevel;
            ViewBag.WorldPath = $"{_configuration.GetValue<string>("AzureBlobBaseURL")}/map{map.MapId}";
            ViewBag.NoWrap = !map.RepeatMapInX;

            return View(model);
        }

        /// <summary>
        ///     GET: /Map/MarkerData/{id}.
        /// </summary>
        /// <param name="id">The map identifier.</param>
        /// <param name="shareKey">The map's shareKey.</param>
        /// <returns>The map's marker data in JSON format.</returns>
        [HttpGet("MarkerData/{id}")]
        public async Task<IActionResult> MarkerData(string id, string shareKey)
        {
            // Determine if user can view map
            var canView = await _mapRepository.CanView(id, User, shareKey);
            if (!canView)
            {
                return ShowErrorView();
            }

            // Retrieve the map
            var map = await _mapRepository.Find(id, User, shareKey);

            // Create response data
            var markerData = map.MarkerData;
            if (string.IsNullOrEmpty(markerData))
            {
                markerData = "[]";
            }

            return Json(markerData);
        }

        /// <summary>
        ///     POST: Map/MarkerData.
        /// </summary>
        /// <param name="model">Map marker data.</param>
        /// <returns>Response JSON.</returns>
        [HttpPost("MarkerData")]
        [Authorize]
        public async Task<IActionResult> MarkerData([FromBody] MarkerEditViewModel model)
        {
            // Ensure that user has privileges to update map
            var canEdit = await _mapRepository.CanEdit(model.MapId, User);
            if (!canEdit)
            {
                return Json("Not Authorized");
            }

            // Attempt to retrieve the map
            var map = await _mapRepository.Find(model.MapId, User, string.Empty);
            if (map == null)
            {
                _loggerService.LogDebug($"Map with id:{model.MapId} not found");
                return Json("Failed to update marker");
            }

            // Update the map
            map.MarkerData = model.MarkerData;

            await _mapRepository.Save(map, User);

            return Json("Success");
        }

        private IActionResult DeleteErrorView()
        {
            return View("Error", new ErrorViewModel
            {
                Title = "Not allowed",
                Message =
                    "You are not allowed to delete this map. It either does not exist (anymore) on this server or your share key is wrong.",
            });
        }

        private IActionResult EditErrorView()
        {
            return View("Error", new ErrorViewModel
            {
                Title = "Not allowed",
                Message =
                    "You are not allowed to edit this map. It either does not exist (anymore) on this server or your share key is wrong.",
            });
        }

        private ViewResult ShowErrorView()
        {
            return View("Error", new ErrorViewModel
            {
                Title = "Unknown map",
                Message =
                    "The map you requested does not exist on this server. It may have been deleted or you might have followed an invalid link.",
            });
        }
    }
}