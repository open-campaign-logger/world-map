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

using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace CampaignKit.WorldMap.Services
{
    /// <summary>
    ///     This application uses the implicit oidc authentication flow which means that the client
    ///     obtains and manages access tokens itself.
    ///     see: https://developer.okta.com/authentication-guide/implementing-authentication/implicit
    ///     Requests from the client are coded to include the token as a "Bearer" in the http request
    ///     as shown below:
    ///     function api() {
    ///     mgr.getUser().then(function (user) {
    ///     var url = "http://localhost:3000/Home/Identity";
    ///     var xhr = new XMLHttpRequest();
    ///     xhr.open("GET", url);
    ///     xhr.onload = function() {
    ///     log(xhr.status, JSON.parse(xhr.responseText));
    ///     }
    ///     xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
    ///     xhr.send();
    ///     });
    ///     }
    ///     This works well for WebAPI scenarios but presents a challenge for MVC style applications
    ///     that do not use custom JavaScript to perform each http request.  In order to include the
    ///     access token in each request it must be converted into a cookie so that the browser can
    ///     automatically pass it with each request without the need for JavaScript coding.
    ///     The following middleware component looks for JWT information stored in a cookie and adds it
    ///     to the request headers as if it was submitted via JavaScript code.
    ///     see: https://stackoverflow.com/questions/37398276/how-can-i-validate-a-jwt-passed-via-cookies
    /// </summary>
    public class JWTInHeaderMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Constructors

        /// <summary>
        /// </summary>
        /// <param name="next"></param>
        public JWTInHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var name = ".worldmap.ui";
            var cookie = context.Request.Cookies[name];

            if (cookie != null)
                if (!context.Request.Headers.ContainsKey("Authorization"))
                    context.Request.Headers.Append("Authorization", cookie);

            await _next.Invoke(context);
        }

        #endregion
    }
}