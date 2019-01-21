// Attach event listeners to screen actions
document.getElementById("login").addEventListener("click", login, false);
document.getElementById("logout").addEventListener("click", logout, false);

// Cookie handling functions
function delete_cookie(name) {
    document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}

// Login screen action
function login() {
    mgr.signinRedirect();
}

// Logout screen action
function logout() {
    delete_cookie(".worldmap.ui");
    mgr.signoutRedirect();
}

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

// Determine if user is logged in.
mgr.getUser().then(function (user) {
    if (user) {
        $(".loggedout").hide();
        log("User logged in", user.profile);
    }
    else {
        $(".loggedin").hide();
        log("User not logged in");
    }
});


