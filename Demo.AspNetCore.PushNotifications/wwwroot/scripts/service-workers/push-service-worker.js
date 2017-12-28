const pushNotificationTitle = 'Demo.AspNetCore.PushNotifications';

self.addEventListener('push', function (event) {
    event.waitUntil(self.registration.showNotification(pushNotificationTitle, {
        body: event.data.text(),
        icon: '/images/push-notification-icon.png'
    }));
});

self.addEventListener('notificationclick', function (event) {
    event.notification.close();
});