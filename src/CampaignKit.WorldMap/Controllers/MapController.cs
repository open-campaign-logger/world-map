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

using System.IO;
using System.Linq;
using System.Threading.Tasks;

using CampaignKit.WorldMap.Data;
using CampaignKit.WorldMap.Entities;
using CampaignKit.WorldMap.Services;
using CampaignKit.WorldMap.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Map MVC controller for application.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    public class MapController : Controller
    {
        #region Fields

        /// <summary>
        ///     The database context
        /// </summary>
        private readonly WorldMapDBContext _dbContext;

        /// <summary>
        ///     The file path service
        /// </summary>
        private readonly IFilePathService _filePathService;

        /// <summary>
        ///     The application logging service.
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
        ///     The random data service
        /// </summary>
        private readonly IRandomDataService _randomDataService;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MapController" /> class.
        /// </summary>
        /// <param name="randomDataService">The random data service.</param>
        /// <param name="mapRepository">The map repository.</param>
        /// <param name="progressService">The progress service.</param>
        /// <param name="filePathService">The file path service.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="loggerService">The logger service.</param>
        public MapController(IRandomDataService randomDataService,
            IMapRepository mapRepository,
            IProgressService progressService,
            IFilePathService filePathService,
            WorldMapDBContext dbContext,
            ILogger<MapController> loggerService)
        {
            _randomDataService = randomDataService;
            _mapRepository = mapRepository;
            _progressService = progressService;
            _filePathService = filePathService;
            _dbContext = dbContext;
            _loggerService = loggerService;
        }

        #endregion

        #region Methods

        private IActionResult DeleteErrorView()
        {
            return View("Error", new ErrorViewModel
            {
                Title = "Not allowed",
                Message =
                    "You are not allowed to delete this map. It either does not exist (anymore) on this server or your secret key is wrong."
            });
        }

        private IActionResult EditErrorView()
        {
            return View("Error", new ErrorViewModel
            {
                Title = "Not allowed",
                Message =
                    "You are not allowed to edit this map. It either does not exist (anymore) on this server or your secret key is wrong."
            });
        }

        private ViewResult ShowErrorView()
        {
            return View("Error", new ErrorViewModel
            {
                Title = "Unknown map",
                Message =
                    "The map you requested does not exist on this server. It may have been deleted or you might have followed an invalid link."
            });
        }

        #endregion

        #region Map Related Actions

        /// <summary>
        ///     GET: /Map/Create
        /// </summary>
        /// <returns>Map creation view containing new randomly generated secret.</returns>
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            var model = new MapCreateViewModel { Secret = _randomDataService.GetRandomText(8) };
            return View(model);
        }

        /// <summary>
        ///     POST: /Map/Create
        /// </summary>
        /// <param name="model">The map model to create.</param>
        /// <returns>Map view with tile progress creation window displayed.</returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MapCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            if (!model.ThisIsMyOwnCreationPublishedRightfully)
                ModelState.AddModelError(nameof(model.ThisIsMyOwnCreationPublishedRightfully),
                    "You have to confirm that this map is your creation and that your are publishing it rightfully.");

            if (!model.ThisIsNotOffensiveNorObviouslyIllegalContent)
                ModelState.AddModelError(nameof(model.ThisIsNotOffensiveNorObviouslyIllegalContent),
                    "You have to confirm that this map image is not offensive nor obviously illegal.");

            if (!model.ProcessingSavingPublishingRightsGrantedForThisSite)
                ModelState.AddModelError(nameof(model.ProcessingSavingPublishingRightsGrantedForThisSite),
                    "You have to allow us to process, save, and publish your image on this site.");

            if (!model.ThisIsMyOwnCreationPublishedRightfully ||
                !model.ThisIsNotOffensiveNorObviouslyIllegalContent ||
                !model.ProcessingSavingPublishingRightsGrantedForThisSite)
                return View();

            var map = new Map
            {
                Name = model.Name,
                Copyright = model.Copyright,
                ContentType = model.Image.ContentType,
                FileExtension = Path.GetExtension(model.Image.FileName ?? string.Empty).ToLower(),
                RepeatMapInX = model.RepeatMapInX,
                IsPublic = model.IsPublic,
                Secret = model.Secret,
                MarkerData = string.Empty
            };

            var id = await _mapRepository.Create(map, model.Image.OpenReadStream(), User);
            if (id == 0)
                ModelState.AddModelError(string.Empty,
                    "Your map could not be saved. Please try again.");
            else
                return RedirectToAction(nameof(Show), new { id, map.Secret, ShowProgress = true });

            return View();
        }

        /// <summary>
        ///     GET: /Map/Delete/{id?}
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Delete view displaying confirmation popup.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _mapRepository.Find(id, User, string.Empty);

            return model == null
                ? DeleteErrorView()
                : View(new MapDeleteViewModel { Name = model.Name, Id = model.MapId });
        }

        /// <summary>
        ///     POST: /Map/Delete/{id?}
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="_">The .</param>
        /// <returns>Redirect to home view.</returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, MapDeleteViewModel _)
        {
            if (!ModelState.IsValid)
                return View();

            var map = await _mapRepository.Delete(id, User);

            if (!map)
            {
                ModelState.AddModelError(string.Empty,
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
        ///     GET: /Map/Edit/{id?}
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Map edit view for the specified map.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            // Determine if user has rights to edit this map
            var canEdit = await _mapRepository.CanEdit(id, User);
            if (!canEdit) return EditErrorView();

            // Load model
            var map = await _mapRepository.Find(id, User, string.Empty);
            if (map == null) return EditErrorView();

            // Return edit screen
            var protocol = Request.IsHttps ? "https" : "http";
            return View(new MapEditViewModel
            {
                Name = map.Name,
                Copyright = map.Copyright,
                RepeatMapInX = map.RepeatMapInX,
                MakeMapPublic = map.IsPublic,
                ShowUrl = Url.Action(nameof(Show), "Map", new { Id = id, map.Secret }, protocol, Request.Host.Value)
            });
        }

        /// <summary>
        ///     POST: /Map/Edit/{id?}
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>Map show view.</returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MapEditViewModel model)
        {
            // Determine if provided data is valid
            if (!ModelState.IsValid) return View();

            // Determine if user has rights to edit this map
            var canEdit = await _mapRepository.CanEdit(id, User);
            if (!canEdit) return EditErrorView();

            // Load model
            var map = await _mapRepository.Find(id, User, string.Empty);
            if (map == null) return EditErrorView();

            map.Name = model.Name;
            map.Copyright = model.Copyright;
            map.RepeatMapInX = model.RepeatMapInX;
            map.IsPublic = model.MakeMapPublic;

            var result = await _mapRepository.Save(map, User);
            if (!result)
                ModelState.AddModelError(string.Empty,
                    "Your map could not be saved. Please try again.");
            else
                return RedirectToAction(nameof(Show), new { model.Id });

            return View();
        }

        /// <summary>
        ///     GET: /Map/
        /// </summary>
        /// <returns>Show all user maps.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _mapRepository.FindAll(User, true);
            model = model.OrderByDescending(m => m.CreationTimestamp);
            return View(model);
        }

        /// <summary>
        ///     GET: /Map/Progress/{id?}
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Tile creation progress for map in JSON format.</returns>
        [HttpGet]
        [Authorize]
        public IActionResult Progress(int id)
        {
            return Json(new { Progress = _progressService.GetMapProgress($"{id}") });
        }

        /// <summary>
        ///     GET: /Map/Sample
        /// </summary>
        /// <returns>Map sample view.</returns>
        [HttpGet]
        public async Task<IActionResult> Sample()
        {
            var map = await _mapRepository.Find(1, User, string.Empty);

            if (map == null)
                return ShowErrorView();

            var model = new MapShowViewModel
            {
                Name = map.Name,
                Id = map.MapId,
                UserId = map.UserId
            };

            ViewBag.MaxZoomLevel = map.MaxZoomLevel;
            ViewBag.WorldPath = Url.Content($"{_filePathService.VirtualWorldBasePath}/1");
            ViewBag.NoWrap = !map.RepeatMapInX;

            return View(model);
        }

        /// <summary>
        ///     GET: /Map/Show/{id?}
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="secret">The secret.</param>
        /// <param name="showProgress">if set to <c>true</c> [show progress].</param>
        /// <returns>The selected map.</returns>
        [HttpGet]
        public async Task<IActionResult> Show(int id, string secret = null, bool showProgress = false)
        {
            // Determine if user can view map
            var canView = await _mapRepository.CanView(id, User, secret);
            if (!canView) return ShowErrorView();

            // Determine if user can edit map
            var canEdit = await _mapRepository.CanEdit(id, User);

            // Retrieve the map
            var map = await _mapRepository.Find(id, User, secret);

            // Create a view model
            var protocol = Request.IsHttps ? "https" : "http";
            var model = new MapShowViewModel
            {
                Name = map.Name,
                Secret = secret,
                UserId = map.UserId,
                ShowProgress = showProgress,
                ProgressUrl = Url.Action(nameof(Progress), new { Id = id }),
                ShowUrl = Url.Action(nameof(Show), "Map", new { Id = id, Secret = secret }, protocol, Request.Host.Value),
                DeleteUrl = Url.Action(nameof(Delete), "Map", new { Id = id }, protocol, Request.Host.Value),
                EditUrl = Url.Action(nameof(Edit), "Map", new { Id = id }, protocol, Request.Host.Value),
                Id = id,
                CanEdit = canEdit
            };

            ViewBag.MaxZoomLevel = map.MaxZoomLevel;
            ViewBag.WorldPath = Url.Content($"{_filePathService.VirtualWorldBasePath}/{id}");
            ViewBag.NoWrap = !map.RepeatMapInX;

            return View(model);
        }

        #endregion

        #region  Marker Related Actions    

        /// <summary>
        ///     GET: /Map/MarkerData/{id?}
        /// </summary>
        /// <param name="id">The map identifier.</param>
        /// <param name="secret">The secret.  Required if </param>
        /// <returns>The map's marker data in JSON format.</returns>
        [HttpGet]
        public async Task<IActionResult> MarkerData(int id, string secret)
        {
            // Determine if user can view map
            var canView = await _mapRepository.CanView(id, User, secret);
            if (!canView) return ShowErrorView();

            // Retrieve the map
            var map = await _mapRepository.Find(id, User, secret);

            // Create response data
            var markerData = map.MarkerData;
            if (string.IsNullOrEmpty(markerData)) markerData = "[]";
            return Json(markerData);
        }

        /// <summary>
        ///     POST: Map/MarkerData/{MapId}
        /// </summary>
        /// <param name="model">Map marker data.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MarkerData([FromBody] MarkerEditViewModel model)
        {
            // Ensure that user has privileges to update map
            var canEdit = await _mapRepository.CanEdit(model.MapId, User);
            if (!canEdit) return Json("Not Authorized");

            // Attempt to retrieve the map
            var map = await _mapRepository.Find(model.MapId, User, string.Empty);
            if (map == null)
            {
                _loggerService.LogError($"Map with id:{model.MapId} not found");
                return Json("Failed to update marker");
            }

            // Update the map
            map.MarkerData = model.MarkerData;

            await _dbContext.SaveChangesAsync();

            return Json("Success");
        }

        #endregion
    }
}