// <copyright file="MapShowViewModel.cs" company="Jochen Linnemann - IT-Service">
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

namespace CampaignKit.WorldMap.UI.ViewModels
{
    /// <summary>
    ///     Class MapShowViewModel.
    /// </summary>
    public class MapShowViewModel
    {
        /// <summary>
        ///     Gets or sets a value indicating whether the user can edit.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can edit; otherwise, <c>false</c>.
        /// </value>
        public bool CanEdit { get; set; }

        /// <summary>
        ///     Gets or sets the map base delete URL.
        /// </summary>
        /// <value>The map base delete URL.</value>
        public string DeleteUrl { get; set; }

        /// <summary>
        ///     Gets or sets the map base edit URL.
        /// </summary>
        /// <value>The map base edit URL.</value>
        public string EditUrl { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

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
        ///     Gets or sets the map share key.
        /// </summary>
        /// <value>The share key.</value>
        public string Share { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [show progress].
        /// </summary>
        /// <value><c>true</c> if [show progress]; otherwise, <c>false</c>.</value>
        public bool ShowProgress { get; set; }

        /// <summary>
        ///     Gets or sets the map show URL.
        /// </summary>
        /// <value>The map show URL.</value>
        public string ShowUrl { get; set; }

        /// <summary>
        ///     Gets or sets the userid of the map owner.
        /// </summary>
        /// <value>The user id.</value>
        public string UserId { get; set; }
    }
}