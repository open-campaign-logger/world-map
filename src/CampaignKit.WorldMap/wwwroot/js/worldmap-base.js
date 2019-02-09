// Attach event listeners to screen actions
document.getElementById("login").addEventListener("click", login, false);
document.getElementById("logout").addEventListener("click", logout, false);

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
var userId = '';
mgr.getUser().then(function (user) {
    if (user) {
        isAuthenticated = true;
        userId = user.profile.sub;
        $(".loggedout").hide();
    }
    else {
        isAuthenticated = false;
        $(".loggedin").hide();
    }
});



