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

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using CampaignKit.WorldMap.Entities;
using CampaignKit.WorldMap.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

namespace CampaignKit.WorldMap.Controllers
{
	/// <summary>
	///		Map MVC controller for application.
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
	public class MapController : Controller
	{
		#region Private Fields

		/// <summary>
		///		The EntityFramework repository for Map data elements.
		/// </summary>
		private readonly IMapRepository _mapRepository;

		/// <summary>
		///		The progress service.
		/// </summary>
		private readonly IProgressService _progressService;

		/// <summary>
		///		The random data service
		/// </summary>
		private readonly IRandomDataService _randomDataService;

		/// <summary>
		/// The file path service
		/// </summary>
		private readonly IFilePathService _filePathService;

		/// <summary>
		/// The database context
		/// </summary>
		private readonly WorldMapDBContext _dbContext;

		/// <summary>
		///		The application logging service.
		/// </summary>
		private readonly ILogger _loggerService;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MapController"/> class.
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

		#endregion Public Constructors

		#region Public Methods

		#region Map Related Actions

		/// <summary>
		///		GET: /Map/Create
		/// </summary>
		/// <returns>Map creation view containing new randomly generated secret.</returns>
		[HttpGet]
		public IActionResult Create()
		{
			var model = new MapCreateViewModel { Secret = _randomDataService.GetRandomText(8) };

			return View(model);
		}

		/// <summary>
		///		POST: /Map/Create
		/// </summary>
		/// <param name="model">The map model to create.</param>
		/// <returns>Map view with tile progress creation window displayed.</returns>
		[HttpPost]
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

			var map = new Entities.Map
			{
				Name = model.Name,
				Secret = model.Secret,
				Copyright = model.Copyright,
				ContentType = model.MapImage.ContentType,
				FileExtension = Path.GetExtension(model.MapImage.FileName ?? string.Empty).ToLower(),
				CreationTimestamp = DateTime.UtcNow,
				RepeatMapInX = model.RepeatMapInX
			};

			var id = await _mapRepository.Create(map, model.MapImage.OpenReadStream());
			if (id == 0)
			{
				ModelState.AddModelError(string.Empty,
					"Your map could not be saved. Please try again.");
			}
			else
			{
				return RedirectToAction(nameof(Show), new { id, map.Secret, ShowProgress = true });
			}

			return View();
		}

		/// <summary>
		///		GET: /Map/Delete/{id?}
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="secret">The secret.</param>
		/// <returns>Delete view displaying confirmation popup.</returns>
		[HttpGet]
		public async Task<IActionResult> Delete(int id, string secret)
		{
			var model = await _mapRepository.Find(id);

			if (model == null || model.Secret != secret)
			{
				return DeleteErrorView();
			}
				
			return View(new MapDeleteViewModel { Name = model.Name, HiddenId = model.MapId, HiddenSecret = model.Secret });
		}

		/// <summary>
		///		POST: /Map/Delete/{id?}
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="secret">The secret.</param>
		/// <param name="model">The model.</param>
		/// <returns>Redirect to home view.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id, string secret, MapDeleteViewModel model)
		{
			if (!ModelState.IsValid)
				return View();

			var map = await _mapRepository.Find(id);

			if (map == null || map.Secret != secret)
				return DeleteErrorView();

			if (map.MapId != model.HiddenId || map.Secret != model.HiddenSecret)
			{
				ModelState.AddModelError(string.Empty,
					"Your map could not be deleted. Please try again.");
			}
			else
			{
				await _mapRepository.Delete(id);
				return RedirectToAction(nameof(Index));
			}

			return View();
		}

		/// <summary>
		///		GET: /Map/Edit/{id?}
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="secret">The secret.</param>
		/// <returns>Map edit view for the specified map.</returns>
		[HttpGet]
		public async Task<IActionResult> Edit(int id, string secret)
		{
			var model = await _mapRepository.Find(id);

			if (model == null || model.Secret != secret) return EditErrorView();

			return View(new MapEditViewModel
			{
				Name = model.Name,
				Copyright = model.Copyright,
				RepeatMapInX = model.RepeatMapInX
			});

		}

		/// <summary>
		///		POST: /Map/Edit/{id?}
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="secret">The secret.</param>
		/// <param name="model">The model.</param>
		/// <returns>Map show view.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, string secret, MapEditViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var map = await _mapRepository.Find(id);

			if (map == null || map.Secret != secret) return EditErrorView();

			map.Name = model.Name;
			map.Copyright = model.Copyright;
			map.RepeatMapInX = model.RepeatMapInX;

			var result = await _mapRepository.Save(map);
			if (!result)
				ModelState.AddModelError(string.Empty,
					"Your map could not be saved. Please try again.");
			else
				return RedirectToAction(nameof(Show), new { map.MapId, map.Secret, ShowProgress = true });

			return View();
		}

		/// <summary>
		///		GET: /Map/
		/// </summary>
		/// <returns>View showin all maps.</returns>
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var model = await _mapRepository.FindAll();
 			model = model.OrderByDescending(m => m.CreationTimestamp);
			return View(model);
		}

		/// <summary>
		///		GET: /Map/Progress/{id?}
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Tile creation progress for map in JSON format.</returns>
		[HttpGet]
		public IActionResult Progress(int id)
		{
			return Json(new { Progress = _progressService.GetMapProgress($"{id}") });
		}

		/// <summary>
		///		GET: /Map/Sample
		/// </summary>
		/// <returns>Map sample view.</returns>
		[HttpGet]
		public async Task<IActionResult> Sample()
		{
			var map = await _mapRepository.Find(1);

			if (map == null)
				return ShowErrorView();

			var model = new MapShowViewModel
			{
				Name = map.Name,
				Id = map.MapId,
				MarkerData = map.MarkerData
			};

			ViewBag.MaxZoomLevel = map.MaxZoomLevel;
			ViewBag.WorldPath = Url.Content($"{_filePathService.VirtualWorldBasePath}/1");
			ViewBag.NoWrap = !map.RepeatMapInX;

			return View(model);
		}

		/// <summary>
		///		GET: /Map/Show/{id?}
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="secret">The secret.</param>
		/// <param name="showProgress">if set to <c>true</c> [show progress].</param>
		/// <returns>The selected map.</returns>
		[HttpGet]
		public async Task<IActionResult> Show(int id, string secret = null, bool showProgress = false)
		{
			var map = await _mapRepository.Find(id);

			if (map == null)
				return ShowErrorView();

			var protocol = Request.IsHttps ? "https" : "http";

			var model = new MapShowViewModel
			{
				Name = map.Name,
				Secret = secret,
				ShowProgress = showProgress,
				ProgressUrl = Url.Action(nameof(Progress), new { Id = id }),
				MapEditUrl = Url.Action(nameof(Edit), "Map", new { Id = id, Secret = secret }, protocol, Request.Host.Value),
				MapShowUrl = Url.Action(nameof(Show), "Map", new { Id = id }, protocol, Request.Host.Value),
				MapBaseDeleteUrl = Url.Action(nameof(Delete), "Map", new { Id = id }, protocol, Request.Host.Value),
				MapBaseEditUrl = Url.Action(nameof(Edit), "Map", new { Id = id }, protocol, Request.Host.Value),
				Id = id,
				MarkerData = map.MarkerData
			};

			ViewBag.MaxZoomLevel = map.MaxZoomLevel;
			ViewBag.WorldPath = Url.Content($"{_filePathService.VirtualWorldBasePath}/{id}");
			ViewBag.NoWrap = !map.RepeatMapInX;

			return View(model);
		}

		/// <summary>
		///		GET: /Map/MarkerData/{id?}
		/// </summary>
		/// <param name="id">The map identifier.</param>
		/// <returns>The selected map.</returns>
		[HttpGet]
		public async Task<IActionResult> GetMarkerData(int id)
		{
			var map = await _mapRepository.Find(id);

			if (map == null)
				return ShowErrorView();

			return Json(map.MarkerData);
		}

		#endregion

		#region  Marker Related Actions    

		/// <summary>
		///		POST: Map/Update/{MapId}
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="markerData">Map marker data in JSON format.</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, string markerData)
		{
			var map = _mapRepository.Find(id);

			if (map == null)
			{
				_loggerService.LogError($"Map with id:{id} not found");
				return Json("Failed to update marker");
			}

			await _dbContext.SaveChangesAsync();

			return View();
		}
		
		#endregion

		#endregion Public Methods

		#region Private Methods

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

		#endregion Private Methods
	}
}
