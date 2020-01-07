using Demo.AspNetCore.PushNotifications.Services.Abstractions;

namespace Demo.AspNetCore.PushNotifications.Services.LiteDB
{
    internal class LiteDatabasePushSubscriptionStoreAccessor : IPushSubscriptionStoreAccessor
    {
        public IPushSubscriptionStore PushSubscriptionStore { get; private set; }

        public LiteDatabasePushSubscriptionStoreAccessor(IPushSubscriptionStore pushSubscriptionStore)
        {
            PushSubscriptionStore = pushSubscriptionStore;
        }

        public void Dispose()
        { }
    }
}
