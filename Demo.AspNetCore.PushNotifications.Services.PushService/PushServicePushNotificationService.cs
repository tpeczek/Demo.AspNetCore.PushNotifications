using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly IPushSubscriptionStoreAccessorProvider _subscriptionStoreAccessorProvider;

        private readonly ILogger _logger;

        public string PublicKey { get { return _options.PublicKey; } }

        public PushServicePushNotificationService(IOptions<PushNotificationServiceOptions> optionsAccessor, PushServiceClient pushClient, IVapidTokenCache vapidTokenCache, IPushSubscriptionStoreAccessorProvider subscriptionStoreAccessorProvider, ILogger<PushServicePushNotificationService> logger)
        {
            _options = optionsAccessor.Value;

            _pushClient = pushClient;
            _pushClient.DefaultAuthentication = new VapidAuthentication(_options.PublicKey, _options.PrivateKey)
            {
                Subject = _options.Subject,
                TokenCache = vapidTokenCache
            };

            _subscriptionStoreAccessorProvider = subscriptionStoreAccessorProvider;
            _logger = logger;
        }

        public Task SendNotificationAsync(PushSubscription subscription, PushMessage message)
        {
            return SendNotificationAsync(subscription, message, CancellationToken.None);
        }

        public async Task SendNotificationAsync(PushSubscription subscription, PushMessage message, CancellationToken cancellationToken)
        {
            try
            {
                await _pushClient.RequestPushMessageDeliveryAsync(subscription, message, cancellationToken);
            }
            catch (Exception ex)
            {
                await HandlePushMessageDeliveryExceptionAsync(ex, subscription);
            }
        }

        private async Task HandlePushMessageDeliveryExceptionAsync(Exception exception, PushSubscription subscription)
        {
            PushServiceClientException pushServiceClientException = exception as PushServiceClientException;

            if (pushServiceClientException is null)
            {
                _logger?.LogError(exception, "Failed requesting push message delivery to {0}.", subscription.Endpoint);
            }
            else
            {
                if ((pushServiceClientException.StatusCode == HttpStatusCode.NotFound) || (pushServiceClientException.StatusCode == HttpStatusCode.Gone))
                {
                    using (IPushSubscriptionStoreAccessor subscriptionStoreAccessor = _subscriptionStoreAccessorProvider.GetPushSubscriptionStoreAccessor())
                    {
                        await subscriptionStoreAccessor.PushSubscriptionStore.DiscardSubscriptionAsync(subscription.Endpoint);
                    }
                    _logger?.LogInformation("Subscription has expired or is no longer valid and has been removed.");
                }
            }
        }
    }
}
