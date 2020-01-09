using Demo.AspNetCore.PushNotifications.Services.Abstractions;

namespace Demo.AspNetCore.PushNotifications.Services.Cosmos
{
    internal class CosmosDbPushSubscriptionStoreAccessorProvider : IPushSubscriptionStoreAccessorProvider
    {
        private readonly IPushSubscriptionStore _pushSubscriptionStore;

        public CosmosDbPushSubscriptionStoreAccessorProvider(IPushSubscriptionStore pushSubscriptionStore)
        {
            _pushSubscriptionStore = pushSubscriptionStore;
        }

        public IPushSubscriptionStoreAccessor GetPushSubscriptionStoreAccessor()
        {
            return new CosmosDbPushSubscriptionStoreAccessor(_pushSubscriptionStore);
        }
    }
}
