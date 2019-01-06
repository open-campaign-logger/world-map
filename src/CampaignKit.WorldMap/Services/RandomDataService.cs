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
using System.Text;

namespace CampaignKit.WorldMap.Entities
{
	/// <summary>
	///     Interface IRandomDataService
	/// </summary>
	public interface IRandomDataService
    {
        #region Public Methods

        /// <summary>
        ///     Gets the random text.
        /// </summary>
        /// <param name="numberOfCharacters">The number of characters.</param>
        /// <returns>System.String.</returns>
        string GetRandomText(int numberOfCharacters);

        #endregion Public Methods
    }

    /// <inheritdoc />
    /// <summary>
    ///     Class DefaultRandomDataService.
    /// </summary>
    /// <seealso cref="T:CampaignKit.WorldMap.Services.IRandomDataService" />
    public class DefaultRandomDataService : IRandomDataService
    {
        #region Private Fields

        private readonly Random _rand = new Random();

        #endregion Private Fields

        #region IRandomDataService Members

        #region Public Methods

        /// <inheritdoc />
        /// <summary>
        ///     Gets the random text.
        /// </summary>
        /// <param name="numberOfCharacters">The number of characters.</param>
        /// <returns>System.String.</returns>
        public string GetRandomText(int numberOfCharacters)
        {
            // ReSharper disable once StringLiteralTypo
            const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var sb = new StringBuilder();
            for (var i = 0; i < numberOfCharacters; i++)
            {
                var next = _rand.Next(characters.Length);
                sb.Append(characters[next]);
            }

            return sb.ToString();
        }

        #endregion Public Methods

        #endregion
    }
}