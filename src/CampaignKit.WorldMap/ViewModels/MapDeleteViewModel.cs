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

using Microsoft.AspNetCore.Mvc;

namespace CampaignKit.WorldMap.ViewModels
{
    /// <summary>
    ///     Class MapDeleteViewModel.
    /// </summary>
    public class MapDeleteViewModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the hidden identifier.
        /// </summary>
        /// <value>The hidden identifier.</value>
        [HiddenInput]
        public int HiddenId { get; set; }

        /// <summary>
        ///     Gets or sets the hidden secret.
        /// </summary>
        /// <value>The hidden secret.</value>
        [HiddenInput]
        public string HiddenSecret { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        #endregion Public Properties
    }
}