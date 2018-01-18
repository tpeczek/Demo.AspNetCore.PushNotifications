using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;

namespace Demo.AspNetCore.PushNotifications.Services.PushService
{
    internal class PushServicePushNotificationService : IPushNotificationService
    {
        private readonly PushNotificationServiceOptions _options;
        private readonly PushServiceClient _pushClient;

        private readonly ILogger _logger;

        public string PublicKey { get { return _options.PublicKey; } }

        public PushServicePushNotificationService(IOptions<PushNotificationServiceOptions> optionsAccessor, IVapidTokenCache vapidTokenCache, ILogger<PushServicePushNotificationService> logger)
        {
            _options = optionsAccessor.Value;

            _pushClient = new PushServiceClient
            {
                DefaultAuthentication = new VapidAuthentication(_options.PublicKey, _options.PrivateKey)
                {
                    Subject = _options.Subject,
                    TokenCache = vapidTokenCache
                }
            };

            _logger = logger;
        }

        public void SendNotification(PushSubscription subscription, PushMessage message)
        {
            try
            {
                _pushClient.RequestPushMessageDeliveryAsync(subscription, message).Wait();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed requesting push message delivery to {0}.", subscription.Endpoint);
            }
        }
    }
}
