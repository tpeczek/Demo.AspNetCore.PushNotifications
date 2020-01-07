using System;
using Microsoft.Extensions.Configuration;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;

namespace Demo.AspNetCore.PushNotifications.Services
{
    internal static class ConfigurationExtensions
    {
        private const string SUBSCRIPTION_STORE_TYPE_CONFIGURATION_KEY = "PushSubscriptionStoreType";
        private const string SUBSCRIPTION_STORE_TYPE_SQLITE = "Sqlite";
        private const string SUBSCRIPTION_STORE_TYPE_LITEDB = "LiteDB";

        public static SubscriptionStoreTypes GetSubscriptionStoreType(this IConfiguration configuration)
        {
            string subscriptionStoreType = configuration.GetValue(SUBSCRIPTION_STORE_TYPE_CONFIGURATION_KEY, SUBSCRIPTION_STORE_TYPE_SQLITE);

            if (subscriptionStoreType.Equals(SUBSCRIPTION_STORE_TYPE_SQLITE, StringComparison.InvariantCultureIgnoreCase))
            {
                return SubscriptionStoreTypes.Sqlite;
            }
            else if (subscriptionStoreType.Equals(SUBSCRIPTION_STORE_TYPE_LITEDB, StringComparison.InvariantCultureIgnoreCase))
            {
                return SubscriptionStoreTypes.LiteDB;
            }
            else
            {
                throw new NotSupportedException($"Not supported {nameof(IPushSubscriptionStore)} type.");
            }
        }
    }
}
