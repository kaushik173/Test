function notifyWithTitle(title, message, onclick ) {

    toastr.options.positionClass = 'toast-bottom-right';
    toastr.options.extendedTimeOut = 0; //1000;
    toastr.options.timeOut = 5000;
    toastr.options.closeButton = true;
    toastr.options.iconClass = 'fa-comment-o toast-info';
    toastr.options.onclick = onclick;
    toastr['custom'](title, message);
}
// Callback fired if Instance ID token is updated.
messaging.onTokenRefresh(() => {
    messaging.getToken().then((currentToken) => {
        if (currentToken) {
            console.log(currentToken);
            sendTokenToServer(currentToken);
            //updateUIForPushEnabled(currentToken);
        } else {
            // Show permission request.
            console.log('No Instance ID token available. Request permission to generate one.');
            // Show permission UI.
            //updateUIForPushPermissionRequired();
            setTokenSentToServer(false);
        }
    }).catch((err) => {
        console.log('An error occurred while retrieving token. ', err);
        //showToken('Error retrieving Instance ID token. ', err);
        setTokenSentToServer(false);
    });
});



messaging.onMessage((payload) => {
    console.log('Message received. ', payload);

    notifyWithTitle(payload.notification.title, payload.notification.body, function () {
        window.open("/Case/Main/" + payload.data.encryptedCaseID);
    });
});



function getInitialToken() {
    if (!isTokenSentToServer()) {
        // Get Instance ID token. Initially this makes a network call, once retrieved
        // subsequent calls to getToken will return from cache.
        messaging.getToken().then((currentToken) => {
            if (currentToken) {
                console.log(currentToken);
                sendTokenToServer(currentToken);
                //updateUIForPushEnabled(currentToken);
            } else {
                // Show permission request.
                console.log('No Instance ID token available. Request permission to generate one.');
                // Show permission UI.
                //updateUIForPushPermissionRequired();
                setTokenSentToServer(false);
            }
        }).catch((err) => {
            console.log('An error occurred while retrieving token. ', err);
            //showToken('Error retrieving Instance ID token. ', err);
            setTokenSentToServer(false);
        });
    }
}

function sendTokenToServer(token) {
    if (!isTokenSentToServer()) {
        var data = { token: token, deviceType: "Web" };
        $.post("/Home/UpdateDeviceToken", data, function (result) {
            if (result.isSuccess)
                console.log("FCM device token saved successfully.");
            else
                console.log("There is some issue while savinb FCM device token.");
        });
        setTokenSentToServer(true);
    } else {
        console.log('Token already sent to server so won\'t send it again unless it changes');
    }
}

function isTokenSentToServer() {
    return window.localStorage.getItem('sentToServer') === '1';
}

function setTokenSentToServer(sent) {
    window.localStorage.setItem('sentToServer', sent ? '1' : '0');
}


//inial calls
getInitialToken();