using Microsoft.Extensions.DependencyInjection;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;

namespace Demo.AspNetCore.PushNotifications.Services.Sqlite
{
    internal class SqlitePushSubscriptionStoreAccessor : IPushSubscriptionStoreAccessor
    {
        private IServiceScope _serviceScope;

        public IPushSubscriptionStore PushSubscriptionStore { get; private set; }

        public SqlitePushSubscriptionStoreAccessor(IPushSubscriptionStore pushSubscriptionStore)
        {
            PushSubscriptionStore = pushSubscriptionStore;
        }

        public SqlitePushSubscriptionStoreAccessor(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope;
            PushSubscriptionStore = _serviceScope.ServiceProvider.GetService<IPushSubscriptionStore>();
        }

        public void Dispose()
        {
            PushSubscriptionStore = null;

            if (!(_serviceScope is null))
            {
                _serviceScope.Dispose();
                _serviceScope = null;
            }
        }
    }
}
