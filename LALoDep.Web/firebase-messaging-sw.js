importScripts('https://www.gstatic.com/firebasejs/7.8.2/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/7.8.2/firebase-messaging.js');

var firebaseConfig = {
    apiKey: "AIzaSyCatFzycIsFvpTRBkJ93QgeiRAHL7pRYJ0",
    authDomain: "jcats-test.firebaseapp.com",
    databaseURL: "https://jcats-test.firebaseio.com",
    projectId: "jcats-test",
    storageBucket: "jcats-test.appspot.com",
    messagingSenderId: "937220853094",
    appId: "1:937220853094:web:9b1b6b86a18401539e0ca7",
    measurementId: "G-YMFX7TGDY4"
};

firebase.initializeApp(firebaseConfig);

const messaging = firebase.messaging();
//messaging.usePublicVapidKey("BK0XqTJ34h-mVDsyUEnsiDvo-tz9agpaaoKXYR8bR4O8T9Gb9tsy2mUBT9Mbk-mA5MsKCZWJMcp3fVR2mjug8dM");    

// If you would like to customize notifications that are received in the
// background (Web app is closed or not in browser focus) then you should
// implement this optional method.
// [START background_handler]

messaging.setBackgroundMessageHandler(function (payload) {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);

    var data = payload || {};
    var shinyData = decoder.run(data);

    // Customize notification here
    const notificationTitle = shinyData.title;
    const notificationOptions = {
        body: shinyData.body,
        icon: shinyData.icon,
        data: payload.data
    };

    self.addEventListener('notificationclick', function (event) {
        var data = event.notification.data;
        event.notification.close();

        const urlToOpen = data.pageUrl;
        const promiseChain = clients.matchAll({type: 'window',includeUncontrolled: true}).then((windowClients) => {
            let matchingClient = null;
            for (let i = 0; i < windowClients.length; i++) {
                const windowClient = windowClients[i];
                if (windowClient.url === urlToOpen) {
                    matchingClient = windowClient;
                    break;
                }
            }
            if (matchingClient) {
                return matchingClient.focus();
            } else {
                return clients.openWindow(urlToOpen);
            }
        });

        event.waitUntil(promiseChain);
    });

    return self.registration.showNotification(notificationTitle, notificationOptions);
});

// [END background_handler]