using System;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;

namespace Demo.AspNetCore.PushNotifications.Services.LiteDB
{
    internal class LiteDatabasePushSubscriptionStoreAccessorProvider : IPushSubscriptionStoreAccessorProvider
    {
        private readonly IPushSubscriptionStore _pushSubscriptionStore;

        public LiteDatabasePushSubscriptionStoreAccessorProvider(IPushSubscriptionStore pushSubscriptionStore)
        {
            _pushSubscriptionStore = pushSubscriptionStore;
        }

        public IPushSubscriptionStoreAccessor GetPushSubscriptionStoreAccessor()
        {
            return new LiteDatabasePushSubscriptionStoreAccessor(_pushSubscriptionStore);
        }
    }
}
