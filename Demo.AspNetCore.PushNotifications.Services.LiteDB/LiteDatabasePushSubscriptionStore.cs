using System;
using System.Threading;
using System.Threading.Tasks;
using Lib.Net.Http.WebPush;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;

namespace Demo.AspNetCore.PushNotifications.Services.LiteDB
{
    internal class LiteDatabasePushSubscriptionStore : IPushSubscriptionStore
    {
        private readonly IPushSubscriptionLiteDatabase _database;

        public LiteDatabasePushSubscriptionStore(IPushSubscriptionLiteDatabase database)
        {
            _database = database;
        }

        public Task StoreSubscriptionAsync(PushSubscription subscription)
        {
            _database.Add(subscription);

            return Task.CompletedTask;
        }

        public Task DiscardSubscriptionAsync(string endpoint)
        {
            _database.Remove(endpoint);

            return Task.CompletedTask;
        }

        public Task ForEachSubscriptionAsync(Action<PushSubscription> action)
        {
            return ForEachSubscriptionAsync(action, CancellationToken.None);
        }

        public Task ForEachSubscriptionAsync(Action<PushSubscription> action, CancellationToken cancellationToken)
        {
            foreach (PushSubscription subscription in _database.GetAll())
            {
                action(subscription);
            }

            return Task.CompletedTask;
        }
    }
}
