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


namespace CampaignKit.WorldMap.Entities
{
	public class Marker
	{
		#region Public Properties

		/// <summary>
		///     Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public int MarkerId { get; set; }

		/// <summary>
		///     Gets or sets the X coordinate of the marker
		/// </summary>
		/// <value>Marker x coordinate</value>
		public int x { get; set; }

		/// <summary>
		///     Gets or sets the Y coordinate of the marker
		/// </summary>
		/// <value>Marker y coordinate</value>
		public int y { get; set; }

		/// <summary>
		///     Gets or sets the marker title
		/// </summary>
		/// <value>Marker title.</value>
		public string Title { get; set; }

		/// <summary>
		///     Gets or sets the marker data
		/// </summary>
		/// <value>Marker data in JSON format.</value>
		public string MarkerData { get; set; }

		#endregion
	}
}
