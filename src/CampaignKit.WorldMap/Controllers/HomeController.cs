﻿// Copyright 2017-2018 Jochen Linnemann
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

using CampaignKit.WorldMap.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.Controllers
{

	/// <summary>
	///		Main MVC controller for application.
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
	public class HomeController : Controller
	{
		#region Private Fields

		/// <summary>
		///		The EntityFramework repository for Map data elements.
		/// </summary>
		private readonly IMapRepository _mapRepository;

		/// <summary>
		///		The application logging service.
		/// </summary>
		private readonly ILogger _loggerService;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="HomeController"/> class.
		/// </summary>
		/// <param name="mapDataService">The map data service.</param>
		/// <param name="logger">The logger.</param>
		public HomeController(IMapRepository mapDataService,
			ILogger<HomeController> logger)
		{
			_mapRepository = mapDataService;
			_loggerService = logger;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <summary>
		///		Get: /
		/// </summary>
		/// <returns>Home view showing last three created maps.</returns>
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var model = (await _mapRepository.FindAll())
				.Where(m => m.MapId != 1)
				.OrderByDescending(m => m.CreationTimestamp)
				.Take(3);
			
			return View(model);
		}

		/// <summary>
		///		Get: /Legalities
		/// </summary>
		/// <returns>Application legalities view.</returns>
		[HttpGet]
		public ActionResult Legalities()
		{
			return View();
		}

		#endregion Public Methods
	}
}