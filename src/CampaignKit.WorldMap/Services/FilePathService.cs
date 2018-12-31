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

using System.IO;

using Microsoft.AspNetCore.Hosting;

namespace CampaignKit.WorldMap.Entities
{
	/// <summary>
	///     Interface IFilePathService
	/// </summary>
	public interface IFilePathService
	{
		#region Public Properties

		/// <summary>
		///     Gets the application data path.
		/// </summary>
		/// <value>The application data path.</value>
		string AppDataPath { get; }

		/// <summary>
		///     Gets the seed data path.
		/// </summary>
		/// <value>The seed data path.</value>
		string SeedDataPath { get; }

		/// <summary>
		///     Gets the world base path.
		/// </summary>
		/// <value>The world base path.</value>
		string PhysicalWorldBasePath { get; }

		/// <summary>
		///     Gets the virtual world base path.
		/// </summary>
		/// <value>The virtual world base path.</value>
		string VirtualWorldBasePath { get; }


		#endregion Public Properties
	}

	/// <inheritdoc />
	/// <summary>
	///     Class DefaultAppDataPathService.
	/// </summary>
	/// <seealso cref="T:CampaignKit.WorldMap.Services.IAppDataPathService" />
	public class DefaultFilePathService : IFilePathService
	{
		#region Public Constructors

		/// <summary>
		///     Initializes a new instance of the <see cref="DefaultFilePathService" /> class.
		/// </summary>
		/// <param name="env">The env.</param>
		public DefaultFilePathService(IHostingEnvironment env)
		{
			AppDataPath = Path.Combine(env.ContentRootPath, "App_Data");
			SeedDataPath = Path.Combine(AppDataPath, "Sample");
			PhysicalWorldBasePath = Path.Combine(env.WebRootPath, "world");
			VirtualWorldBasePath = "~/world";
		}

		#endregion Public Constructors

		#region IAppDataPathService Members

		#region Public Properties

		/// <inheritdoc />
		/// <summary>
		///     Gets the application data path.
		/// </summary>
		/// <value>The application data path.</value>
		/// <exception cref="!:NotImplementedException"></exception>
		public string AppDataPath { get; }

		/// <inheritdoc />
		/// <summary>
		///     Gets the world base path.
		/// </summary>
		/// <value>The world base path.</value>
		public string PhysicalWorldBasePath { get; }

		/// <inheritdoc />
		/// <summary>
		///     Gets or sets the virtual world base path.
		/// </summary>
		/// <value>The virtual world base path.</value>
		public string VirtualWorldBasePath { get; }

		/// <inheritdoc />		
		/// <summary>
		///     Gets the seed data path.
		/// </summary>
		/// <value>The seed data path.</value>
		public string SeedDataPath { get; }

		#endregion Public Properties

		#endregion
	}
}