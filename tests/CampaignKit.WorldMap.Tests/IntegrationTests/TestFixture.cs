﻿// Copyright 2017 Jochen Linnemann
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
using System.Net.Http;
using System.Reflection;
using CampaignKit.WorldMap.Entities;
using CampaignKit.WorldMap.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CampaignKit.WorldMap.Tests.IntegrationTests
{
	/// <inheritdoc />
	/// <summary>
	///     A test fixture which hosts the target project (project we wish to test) in an in-memory server.
	/// </summary>
	/// <typeparam name="TStartup">Target project's startup type</typeparam>
	public sealed class TestFixture<TStartup> : IDisposable
	{
		#region Static Fields

		private const string SolutionName = "WorldMap.sln";

		#endregion

		#region Private Fields

		private readonly TestServer _server;

		#endregion

		#region Public Properties


		/// <summary>
		///     Gets the client.
		/// </summary>
		/// <value>The client.</value>
		public HttpClient Client { get; }

		/// <summary>
		/// Gets or sets the file path service.
		/// </summary>
		/// <value>
		/// The file path service.
		/// </value>
		public IFilePathService FilePathService { get; set; }

		/// <summary>
		/// Gets or sets the map data service.
		/// </summary>
		/// <value>
		/// The map data service.
		/// </value>
		public IMapRepository MapDataService { get; set; }

		/// <summary>
		/// Gets or sets the marker data service.
		/// </summary>
		/// <value>
		/// The marker data service.
		/// </value>
		public IMarkerRepository MarkerDataService { get; set; }

		/// <summary>
		/// Gets or sets the progress service.
		/// </summary>
		/// <value>
		/// The progress service.
		/// </value>
		public IProgressService ProgressService { get; set; }

		/// <summary>
		/// Gets or sets the random data service.
		/// </summary>
		/// <value>
		/// The random data service.
		/// </value>
		public IRandomDataService RandomDataService { get; set; }

		/// <summary>
		/// Gets or sets the tile creation service.
		/// </summary>
		/// <value>
		/// The tile creation service.
		/// </value>
		public TileCreationService TileCreationService { get; set; }

		/// <summary>
		/// Gets or sets the database service.
		/// </summary>
		/// <value>
		/// The database service.
		/// </value>
		public WorldMapDBContext DatabaseService { get; set; }

		/// <summary>
		/// Gets or sets the logger service.
		/// </summary>
		/// <value>
		/// The logger service.
		/// </value>
		public ILogger LoggerService { get; set; }

		#endregion

		#region Constructors

		/// <inheritdoc />
		/// <summary>
		///     Initializes a new instance of the
		///     <see cref="T:CampaignKit.WorldMap.Tests.IntegrationTests.TestFixture`1" /> class.
		/// </summary>
		public TestFixture()
			: this(Path.Combine("src"))
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="TestFixture{TStartup}" /> class.
		/// </summary>
		/// <param name="solutionRelativeTargetProjectParentDir">The solution relative target project parent dir.</param>
		private TestFixture(string solutionRelativeTargetProjectParentDir)
		{
			var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
			var contentRoot = GetProjectPath(solutionRelativeTargetProjectParentDir, startupAssembly);

			// Create a web host builder
			var builder = new WebHostBuilder()
				.UseContentRoot(contentRoot)
				.UseEnvironment("Testing")
				.UseStartup(typeof(Startup));


			// Instantiate the web host
			_server = new TestServer(builder);

			// Create the http client.
			Client = _server.CreateClient();
			Client.BaseAddress = new Uri("http://localhost");
			
		}

		#endregion

		#region Implementations

		/// <inheritdoc />
		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Client.Dispose();
			_server.Dispose();
		}

		#endregion

		#region Methods

		/// <summary>
		///     Gets the full path to the target project path that we wish to test
		/// </summary>
		/// <param name="solutionRelativePath">
		///     The parent directory of the target project.
		///     e.g. src, samples, test, or test/Websites
		/// </param>
		/// <param name="startupAssembly">The target project's assembly.</param>
		/// <returns>The full path to the target project.</returns>
		private static string GetProjectPath(string solutionRelativePath, Assembly startupAssembly)
		{
			// Get name of the target project which we want to test
			var projectName = startupAssembly.GetName().Name;
			// Get currently executing test project path
			var applicationBasePath = AppContext.BaseDirectory;
			// Find the folder which contains the solution file.
			// We then use this information to find the target project which we want to test.
			var directoryInfo = new DirectoryInfo(applicationBasePath);
			do
			{
				var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, SolutionName));
				if (solutionFileInfo.Exists)
					return Path.GetFullPath(Path.Combine(directoryInfo.FullName, solutionRelativePath, projectName));

				directoryInfo = directoryInfo.Parent;
			}
			while (directoryInfo?.Parent != null);

			throw new Exception($"Solution root could not be located using application root {applicationBasePath}.");
		}

		#endregion
	}
}
