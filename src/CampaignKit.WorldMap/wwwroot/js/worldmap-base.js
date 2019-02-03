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

// Determine if user is logged in.
if (isAuthenticated) {
    $(".loggedout").hide();
} else {
    $(".loggedin").hide();
}


