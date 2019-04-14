﻿// Copyright 2017-2019 Jochen Linnemann, Cory Gill
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

namespace CampaignKit.WorldMap.ViewModels
{
    /// <summary>
    ///     Class MapShowViewModel.
    /// </summary>
    public class MarkerEditViewModel
    {
        #region Constructors

        #region Public Constructor

        #endregion

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int MapId { get; set; }

        /// <summary>
        ///     Map marker data in JSON format.
        /// </summary>
        public string MarkerData { get; set; }

        #endregion
    }
}