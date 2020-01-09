using Microsoft.Extensions.DependencyInjection;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;

namespace Demo.AspNetCore.PushNotifications.Services.Cosmos
{
    public static class CosmosDbServiceCollectionExtensions
    {
        public static IServiceCollection AddCosmosDbPushSubscriptionStore(this IServiceCollection services)
        {
            services.AddSingleton<IPushSubscriptionCosmosDbClient, PushSubscriptionCosmosDbClient>();

            services.AddSingleton<IPushSubscriptionStore, CosmosDbPushSubscriptionStore>();
            services.AddSingleton<IPushSubscriptionStoreAccessorProvider, CosmosDbPushSubscriptionStoreAccessorProvider>();

            return services;
        }
    }
}
