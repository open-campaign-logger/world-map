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
using CampaignKit.WorldMap.Services;
using CampaignKit.WorldMap.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace CampaignKit.WorldMap.Controllers
{
	/// <inheritdoc />
	/// <summary>
	///     Class MapController.
	/// </summary>
	/// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
	public class MapController : Controller
	{
		#region Private Fields

		private readonly IMapDataService _mapDataService;
		private readonly IProgressService _progressService;
		private readonly IRandomDataService _randomDataService;
		private readonly IFilePathService _filePathService;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		///     Initializes a new instance of the <see cref="MapController" /> class.
		/// </summary>
		/// <param name="randomDataService">The random data service.</param>
		/// <param name="mapDataService">The map data service.</param>
		/// <param name="progressService">The progress service.</param>
		/// <param name="filePathService">The file path service.</param>
		public MapController(IRandomDataService randomDataService, 
			IMapDataService mapDataService, 
			IProgressService progressService, 
			IFilePathService filePathService)
		{
			_randomDataService = randomDataService;
			_mapDataService = mapDataService;
			_progressService = progressService;
			_filePathService = filePathService;
		}

		#endregion Public Constructors

		#region Public Methods

		[HttpGet]
		public IActionResult Create()
		{
			var model = new MapCreateViewModel { Secret = _randomDataService.GetRandomText(8) };

			return View(model);
		}

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

			var map = new Map
			{
				Name = model.Name,
				Secret = model.Secret,
				Copyright = model.Copyright,
				ContentType = model.MapImage.ContentType,
				FileExtension = Path.GetExtension(model.MapImage.FileName ?? string.Empty).ToLower(),
				CreationTimestamp = DateTime.UtcNow,
				RepeatMapInX = model.RepeatMapInX
			};

			var saveResult = await _mapDataService.Create(map, model.MapImage.OpenReadStream());
			if (!saveResult)
			{
				ModelState.AddModelError(string.Empty,
					"Your map could not be saved. Please try again.");
			}
			else
			{
				return RedirectToAction(nameof(Show), new { map.MapId, map.Secret, ShowProgress = true });
			}

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id, string secret)
		{
			var model = await _mapDataService.Find(id);

			if (model == null || model.Secret != secret)
			{
				return DeleteErrorView();
			}
				
			return View(new MapDeleteViewModel { Name = model.Name, HiddenId = model.MapId, HiddenSecret = model.Secret });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id, string secret, MapDeleteViewModel model)
		{
			if (!ModelState.IsValid)
				return View();

			var map = await _mapDataService.Find(id);

			if (map == null || map.Secret != secret)
				return DeleteErrorView();

			if (map.MapId != model.HiddenId || map.Secret != model.HiddenSecret)
			{
				ModelState.AddModelError(string.Empty,
					"Your map could not be deleted. Please try again.");
			}
			else
			{
				await _mapDataService.Delete(id);
				return RedirectToAction(nameof(Index));
			}

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id, string secret)
		{
			var model = await _mapDataService.Find(id);

			if (model == null || model.Secret != secret) return EditErrorView();

			return View(new MapEditViewModel
			{
				Name = model.Name,
				Copyright = model.Copyright,
				RepeatMapInX = model.RepeatMapInX
			});

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, string secret, MapEditViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var map = await _mapDataService.Find(id);

			if (map == null || map.Secret != secret) return EditErrorView();

			map.Name = model.Name;
			map.Copyright = model.Copyright;
			map.RepeatMapInX = model.RepeatMapInX;

			var result = await _mapDataService.Save(map);
			if (!result)
				ModelState.AddModelError(string.Empty,
					"Your map could not be saved. Please try again.");
			else
				return RedirectToAction(nameof(Show), new { map.MapId, map.Secret, ShowProgress = true });

			return View();
		}

		public async Task<IActionResult> Index()
		{
			var model = await _mapDataService.FindAll();
			model.OrderByDescending(m => m.CreationTimestamp);
			return View(model);
		}

		public IActionResult Progress(int id)
		{
			return Json(new { Progress = _progressService.GetMapProgress($"map_{id}") });
		}

		public async Task<IActionResult> Sample()
		{
			if (!Request.Path.Value.EndsWith("/"))
			{
				var actionUrl = Url.Action("Sample");
				if (!actionUrl.EndsWith("/")) actionUrl += "/";

				return Redirect(actionUrl);
			}

			ViewBag.MaxZoomLevel = 4;
			ViewBag.WorldPath = Url.Content($"{_filePathService.VirtualWorldBasePath}/sample");
			ViewBag.NoWrap = false;

			return View();
		}

		public async Task<IActionResult> Show(int id, string secret = null, bool showProgress = false)
		{
			var map = await _mapDataService.Find(id);

			if (map == null)
				return ShowErrorView();

			var protocol = Request.IsHttps ? "https" : "http";

			var model = new MapShowViewModel
			{
				Name = map.Name,
				Secret = secret,
				ShowProgress = showProgress,
				ProgressUrl = Url.Action(nameof(Progress), new { Id = id }),
				MapEditUrl = Url.Action(nameof(Edit), "Map", new { Id = id, Secret = secret }, protocol,
					Request.Host.Value),
				MapShowUrl = Url.Action(nameof(Show), "Map", new { Id = id }, protocol, Request.Host.Value),

				MapBaseDeleteUrl = Url.Action(nameof(Delete), "Map", new { Id = id }, protocol, Request.Host.Value),
				MapBaseEditUrl = Url.Action(nameof(Edit), "Map", new { Id = id }, protocol, Request.Host.Value)
			};

			ViewBag.MaxZoomLevel = map.MaxZoomLevel;
			ViewBag.WorldPath = Url.Content($"{_filePathService.VirtualWorldBasePath}/{id}");
			ViewBag.NoWrap = !map.RepeatMapInX;

			return View(model);
		}

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