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


using Microsoft.AspNetCore.Mvc;

namespace CampaignKit.WorldMap.ViewModels
{
	/// <summary>
	///     Class MapDeleteViewModel.
	/// </summary>
	public class MapDeleteViewModel
    {

		#region Hidden Properties

		/// <summary>
		///     Gets or sets the Map identifier.
		/// </summary>
		/// <value>The map identifier.</value>
		[HiddenInput]
		public int Id { get; set; }

		#endregion

		#region Public Properties

		/// <summary>
		///     Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

        #endregion Public Properties

    }
}