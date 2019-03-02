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

// Setup oidc-connect-js settings
const { protocol, hostname, port } = window.location;
const rootUri = `${protocol}//${hostname}${port ? `:${port}` : ''}`;

const settings = {
    authority: 'https://campaign-identity.com',
    client_id: 'worldmap.ui',
    redirect_uri: `${rootUri}/oidc-callback.html`,
    post_logout_redirect_uri: `${rootUri}/oidc-logout.html`,
    automaticSilentRenew: true,
    silent_redirect_uri: `${rootUri}/oidc_renew.html`,
    loadUserInfo: true,
    response_type: 'token id_token',
    scope: 'openid profile logger.ro',
};

// Create an oidc-connect-js user manager
var mgr = new Oidc.UserManager(settings);

// Convert JWT token to JWT cookie
function getJWTCookie(user) {

    const url = `${rootUri}/Home/JwtCookie`;
    const xhr = new XMLHttpRequest();
    xhr.open('GET', url, false);
    xhr.setRequestHeader('Authorization', `Bearer ${user.access_token}`);
    xhr.send();

}

// Cookie handling functions
function deleteJWTCookie() {
    document.cookie = '.worldmap.ui=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}