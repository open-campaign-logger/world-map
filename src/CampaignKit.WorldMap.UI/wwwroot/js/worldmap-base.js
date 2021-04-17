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

// Attach event listeners to screen actions
document.getElementById('login').addEventListener('click', login, false);
document.getElementById('logout').addEventListener('click', logout, false);

// Login screen action
function login() {
    mgr.signinRedirect();
}

// Logout screen action
function logout() {
    mgr.signoutRedirect();
}

// Determine if user has valid access token
var isAuthenticated = false;
var user;
mgr.getUser().then(function(obj) {
    if (obj) {
        isAuthenticated = true;
        user = obj;
        $('.loggedout').hide();
    } else {
        isAuthenticated = false;
        $('.loggedin').hide();
    }
});