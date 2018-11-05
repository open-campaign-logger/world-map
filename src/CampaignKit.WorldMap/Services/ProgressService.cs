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

using System.Collections.Generic;

namespace CampaignKit.WorldMap.Services
{
    /// <summary>
    ///     Interface IProgressIndicator
    /// </summary>
    public interface IProgressIndicator
    {
        #region Public Methods

        /// <summary>
        ///     Sets the progress.
        ///     0.0 = 0% .. 1.0 = 100%
        /// </summary>
        /// <param name="progress">The progress.</param>
        void SetProgress(double progress);

        #endregion Public Methods
    }

    /// <summary>
    ///     Interface IProgressService
    /// </summary>
    public interface IProgressService
    {
        #region Public Methods

        /// <summary>
        ///     Creates the indicator.
        /// </summary>
        /// <param name="indicatorId">The indicator identifier.</param>
        /// <returns>IProgressIndicator.</returns>
        IProgressIndicator CreateIndicator(string indicatorId);

        /// <summary>
        ///     Gets the progress.
        ///     0.0 = 0% .. 1.0 = 100%
        /// </summary>
        /// <param name="indicatorId">The indicator identifier.</param>
        /// <returns>System.Double.</returns>
        double? GetProgress(string indicatorId);

        #endregion Public Methods
    }

    /// <inheritdoc />
    /// <summary>
    ///     Class DefaultProgressService.
    /// </summary>
    /// <seealso cref="T:CampaignKit.WorldMap.Services.IProgressService" />
    public class DefaultProgressService : IProgressService
    {
        #region Private Fields

        private readonly SortedDictionary<string, DefaultProgressIndicator> _indicators =
            new SortedDictionary<string, DefaultProgressIndicator>();

        #endregion Private Fields

        #region Nested type: DefaultProgressIndicator

        #region Private Classes

        /// <inheritdoc />
        /// <summary>
        ///     Class DefaultProgressIndicator.
        /// </summary>
        /// <seealso cref="T:CampaignKit.WorldMap.Services.IProgressIndicator" />
        private class DefaultProgressIndicator : IProgressIndicator
        {
            #region Public Properties

            /// <summary>
            ///     Gets or sets the progress.
            /// </summary>
            /// <value>The progress.</value>
            public double Progress { get; private set; }

            #endregion Public Properties

            #region IProgressIndicator Members

            #region Public Methods

            /// <inheritdoc />
            /// <summary>
            ///     Sets the progress.
            /// </summary>
            /// <param name="progress">The progress.</param>
            public void SetProgress(double progress)
            {
                Progress = progress;
            }

            #endregion Public Methods

            #endregion
        }

        #endregion Private Classes

        #endregion

        #region Public Methods

        /// <inheritdoc />
        /// <summary>
        ///     Creates the indicator.
        /// </summary>
        /// <param name="indicatorId">The indicator identifier.</param>
        /// <returns>IProgressIndicator.</returns>
        public IProgressIndicator CreateIndicator(string indicatorId)
        {
            var indicator = new DefaultProgressIndicator();
            _indicators.Add(indicatorId, indicator);

            return indicator;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the progress.
        /// </summary>
        /// <param name="indicatorId">The indicator identifier.</param>
        /// <returns>System.Double.</returns>
        public double? GetProgress(string indicatorId)
        {
            return _indicators.ContainsKey(indicatorId) ? _indicators[indicatorId].Progress : (double?) null;
        }

        #endregion Public Methods
    }
}