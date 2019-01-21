// Setup oidc-connect-js settings
const { protocol, hostname, port } = window.location;
const rootUri = `${protocol}//${hostname}${port ? `:${port}` : ''}`;

const settings = {
    authority: 'https://campaign-identity.com',
    client_id: 'worldmap.ui',
    redirect_uri: `${rootUri}/oidc-callback`,
    post_logout_redirect_uri: `${rootUri}/oidc-callback`,
    automaticSilentRenew: true,
    silent_redirect_uri: `${rootUri}/silent_renew.html`,
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

// Determine if user has valid access token
var isAuthenticated = false;
mgr.getUser().then(function (user) {
    if (user) {
        isAuthenticated = true;
        if (document.cookie.indexOf('.worldmap.ui=') <= 0) {
            getJWTCookie();
        }
    }
    else {
        isAuthenticated = false;
    }
});