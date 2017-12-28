using Microsoft.Extensions.Options;
using WebPush;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;

namespace Demo.AspNetCore.PushNotifications.Services.WebPush
{
    internal class WebPushPushNotificationService : IPushNotificationService
    {
        private readonly PushNotificationServiceOptions _options;
        private readonly WebPushClient _pushClient;

        public string PublicKey { get { return _options.PublicKey; } }

        public WebPushPushNotificationService(IOptions<PushNotificationServiceOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;

            _pushClient = new WebPushClient();
            _pushClient.SetVapidDetails(_options.Subject, _options.PublicKey, _options.PrivateKey);
        }

        public void SendNotification(Abstractions.PushSubscription subscription, string payload)
        {
            var webPushSubscription = new global::WebPush.PushSubscription(
                subscription.Endpoint,
                subscription.GetKey(PushEncryptionKeyName.P256DH),
                subscription.GetKey(PushEncryptionKeyName.Auth));

            _pushClient.SendNotification(webPushSubscription, payload);
        }
    }
}
