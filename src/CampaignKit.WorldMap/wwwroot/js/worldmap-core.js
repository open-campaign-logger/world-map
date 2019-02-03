﻿// Setup oidc-connect-js settings
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

// Convert JWT token to cookie
function getJWTCookie() {
    mgr.getUser().then(function (user) {

        var url = `${rootUri}/Home/JwtCookie`;

        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();

    });
}

// Cookie handling functions
function deleteJWTCookie() {
    document.cookie = '.worldmap.ui=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}

// Determine if user has valid access token
var isAuthenticated = false;
var userId = '';
mgr.getUser().then(function (user) {
    if (user) {
        isAuthenticated = true;
        if (document.cookie.indexOf('.worldmap.ui=') <= 0) {
            getJWTCookie();
        }
        userId = user.profile.sub;
    }
    else {
        isAuthenticated = false;
        deleteJWTCookie();
    }
});