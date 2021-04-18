// <copyright file="MapEditViewModel.cs" company="Jochen Linnemann - IT-Service">
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
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Class MapEditViewModel.
    /// </summary>
    public class MapEditViewModel
    {
        /// <summary>
        ///     Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        [Display(Description = "You might want to provide copyright information for your creation.")]
        public string Copyright { get; set; }

        /// <summary>
        ///     Gets or sets the Map identifier.
        /// </summary>
        /// <value>The map identifier.</value>
        [HiddenInput]
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this map should be made public.
        /// </summary>
        /// <value><c>true</c> if this map should be made public; otherwise, <c>false</c>.</value>
        [Display(Name = "This map should be made public so others can view it.")]
        [Required]
        public bool MakeMapPublic { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Display(Name = "World Name")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [repeat map in x].
        /// </summary>
        /// <value><c>true</c> if [repeat map in x]; otherwise, <c>false</c>.</value>
        [Display(Name = "Repeat map horizontally")]
        public bool RepeatMapInX { get; set; }

        /// <summary>
        ///     Gets or sets the map show URL.
        /// </summary>
        /// <value>The map show URL.</value>
        public string ShowUrl { get; set; }
    }
}