using Microsoft.Extensions.Options;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;
using Demo.AspNetCore.PushNotifications.Services.PushService.Client;
using Demo.AspNetCore.PushNotifications.Services.PushService.Client.Authentication;

namespace Demo.AspNetCore.PushNotifications.Services.PushService
{
    internal class PushServicePushNotificationService : IPushNotificationService
    {
        private readonly PushNotificationServiceOptions _options;
        private readonly PushServiceClient _pushClient;

        public string PublicKey { get { return _options.PublicKey; } }

        public PushServicePushNotificationService(IOptions<PushNotificationServiceOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;

            _pushClient = new PushServiceClient
            {
                DefaultAuthentication = new VapidAuthentication(_options.PublicKey, _options.PrivateKey)
                {
                    Subject = _options.Subject
                }
            };
        }

        public void SendNotification(PushSubscription subscription, string payload)
        {
            _pushClient.RequestPushMessageDeliveryAsync(subscription, new PushMessage(payload)).Wait();
        }
    }
}
