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

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;
using CampaignKit.WorldMap.Attributes;

namespace CampaignKit.WorldMap.ViewModels
{
    /// <summary>
    ///     Class MapCreateViewModel.
    /// </summary>
    public class MapCreateViewModel
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        [Display(Description = "You might want to provide copyright information for your creation.")]
        public string Copyright { get; set; }

        /// <summary>
        ///     Gets or sets the map image.
        /// </summary>
        /// <value>The map image.</value>
        [Display(Name = "World Map Image")]
        [Required]
        [DataType(DataType.Upload)]
		[MapFile(MaxLength = 10485760, Extensions = "png,jpg,jpeg,gif,bmp")]
        public IFormFile Image { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this map should be made public.
        /// </summary>
        /// <value><c>true</c> if this map should be made public; otherwise, <c>false</c>.</value>
        [Display(Name = "This map should be made public.")]
        public bool IsPublic { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Display(Name = "World Name")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [processing saving publishing rights granted for this site].
        /// </summary>
        /// <value><c>true</c> if [processing saving publishing rights granted for this site]; otherwise, <c>false</c>.</value>
        [Display(Name =
            "I am granting you (the site owner/maintainer) the right to process, save, and publish this map for display on this site.")]
        [Required]
        public bool ProcessingSavingPublishingRightsGrantedForThisSite { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [repeat map in x].
        /// </summary>
        /// <value><c>true</c> if [repeat map in x]; otherwise, <c>false</c>.</value>
        [Display(Name = "Repeat map horizontally")]
        public bool RepeatMapInX { get; set; }

        /// <summary>
        ///     Gets or sets the map sharing key.
        /// </summary>
        /// <value>The sharing key.</value>
        [Display(Name = "Share Key", Description = "You will need this key to share your map.")]
        [Required]
        public string Share { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [this is my own creation published rightfully].
        /// </summary>
        /// <value><c>true</c> if [this is my own creation published rightfully]; otherwise, <c>false</c>.</value>
        [Display(Name = "This map is of my own creation and I am not violating any copyright laws by publishing it.")]
        [Required]
        public bool ThisIsMyOwnCreationPublishedRightfully { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [this is not offensive or obviously illegal].
        /// </summary>
        /// <value><c>true</c> if [this is not offensive or obviously illegal]; otherwise, <c>false</c>.</value>
        [Display(Name = "This map image does not present offensive nor obviously illegal content.")]
        [Required]
        public bool ThisIsNotOffensiveNorObviouslyIllegalContent { get; set; }

        #endregion
    }
}