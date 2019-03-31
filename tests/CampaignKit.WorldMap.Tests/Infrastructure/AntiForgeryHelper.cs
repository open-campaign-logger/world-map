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

using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.Net.Http.Headers;

using Xunit;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
    public class AntiForgeryHelper
    {
        #region Static Fields

        public static Regex AntiForgeryFormFieldRegex = new Regex(
            @"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");

        private static SetCookieHeaderValue _antiForgeryCookie;
        private static string _antiForgeryToken;

        #endregion

        #region Methods

        public static async Task<string> EnsureAntiForgeryTokenAsync(HttpClient client, string relativeUrl)
        {
            if (_antiForgeryToken != null)
                return _antiForgeryToken;

            // Retrieve the resource and ensure that it loads correctly.
            var response = await client.GetAsync(relativeUrl);
            response.EnsureSuccessStatusCode();

            // Ensure antiforgery cookie has been received.
            _antiForgeryCookie = TryGetAntiForgeryCookie(response);
            Assert.NotNull(_antiForgeryCookie);

            // Add antiforgery cookie to request header.
            AddCookieToDefaultRequestHeader(client, _antiForgeryCookie);

            // Extract antiforgery token from form data
            _antiForgeryToken = await GetAntiForgeryToken(response);

            return _antiForgeryToken;
        }

        private static void AddCookieToDefaultRequestHeader(
            HttpClient client,
            SetCookieHeaderValue antiForgeryCookie)
        {
            client.DefaultRequestHeaders.Add(
                "Cookie",
                new CookieHeaderValue(antiForgeryCookie.Name, antiForgeryCookie.Value)
                    .ToString());
        }

        private static async Task<string> GetAntiForgeryToken(HttpResponseMessage response)
        {
            var responseHtml = await response.Content.ReadAsStringAsync();
            var match = AntiForgeryFormFieldRegex.Match(responseHtml);

            return match.Success ? match.Groups[1].Captures[0].Value : null;
        }

        private static SetCookieHeaderValue TryGetAntiForgeryCookie(HttpResponseMessage response)
        {
            if (response.Headers.TryGetValues("Set-Cookie", out var values))
                return SetCookieHeaderValue.ParseList(values.ToList())
                    .SingleOrDefault(
                        c => c.Name.StartsWith(
                            ".AspNetCore.AntiForgery.",
                            StringComparison.InvariantCultureIgnoreCase));

            return null;
        }

        #endregion
    }
}