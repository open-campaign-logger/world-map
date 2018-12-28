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

using System;

namespace CampaignKit.WorldMap.ViewModels
{
    /// <summary>
    ///     Class MapShowViewModel.
    /// </summary>
    public class MapShowViewModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets the map base delete URL.
        /// </summary>
        /// <value>The map base delete URL.</value>
        public string MapBaseDeleteUrl { get; set; }

        /// <summary>
        ///     Gets the map base edit URL.
        /// </summary>
        /// <value>The map base edit URL.</value>
        public string MapBaseEditUrl { get; set; }

        /// <summary>
        ///     Gets or sets the map edit URL.
        /// </summary>
        /// <value>The map edit URL.</value>
        public string MapEditUrl { get; set; }

        /// <summary>
        ///     Gets or sets the map show URL.
        /// </summary>
        /// <value>The map show URL.</value>
        public string MapShowUrl { get; set; }
				
		/// <summary>
		///     Gets or sets the marker update URL.
		///     HTTPPost: Marker data to update.
		/// </summary>
		/// <value>The marker update URL.</value>
		public string MarkerUpdateUrl { get; set; }

		/// <summary>
		///     Gets or sets the marker delete URL.
		///     HTTPPost: Marker data to delete.
		/// </summary>
		/// <value>The marker delete URL.</value>
		public string MarkerDeleteUrl { get; set; }

		/// <summary>
		///     Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the progress URL.
        /// </summary>
        /// <value>The progress URL.</value>
        public string ProgressUrl { get; set; }

        /// <summary>
        ///     Gets the secret.
        /// </summary>
        /// <value>The secret.</value>
        public string Secret { get; set; }

        /// <summary>
        ///     Gets a value indicating whether [show progress].
        /// </summary>
        /// <value><c>true</c> if [show progress]; otherwise, <c>false</c>.</value>
        public bool ShowProgress { get; set; }

        #endregion Public Properties
    }
}