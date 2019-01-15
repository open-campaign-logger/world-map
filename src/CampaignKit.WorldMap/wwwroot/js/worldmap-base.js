// Used for testing purposes only
function log() {
    document.getElementById('results').innerText = '';

    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerHTML += msg + '\r\n';
    });
}

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

// Attach event listeners to screen actions
document.getElementById("login").addEventListener("click", login, false);
document.getElementById("logout").addEventListener("click", logout, false);
document.getElementById("create").addEventListener("click", create, false);
document.getElementById("index").addEventListener("click", index, false);
document.getElementById("api").addEventListener("click", api, false);

// Create an oidc-connect-js user manager
var mgr = new Oidc.UserManager(settings);

// Determine if user has valid access token
var isAuthenticated = false;
mgr.getUser().then(function (user) {
    if (user) {
        isAuthenticated = true;
        $(".loggedout").hide();
        log("User logged in", user.profile);
    }
    else {
        isAuthenticated = false;
        $(".loggedin").hide();
        log("User not logged in");
    }
});

// Screen action handlers

// Login screen action
function login() {
    mgr.signinRedirect();
}

// Logout screen action
function logout() {
    mgr.signoutRedirect();
}

// Create screen action
function create() {
}

// Index screen action
function index() {
}

function api() {
    mgr.getUser().then(function (user) {
        var url = "http://localhost:3000/Home/Identity";

        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
            log(xhr.status, JSON.parse(xhr.responseText));
        }
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
}
