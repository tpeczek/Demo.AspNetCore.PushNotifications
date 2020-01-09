using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.AspNetCore.PushNotifications.Services.Cosmos
{
    public static class CosmosDbApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCosmosDbPushSubscriptionStore(this IApplicationBuilder app)
        {
            IPushSubscriptionCosmosDbClient pushSubscriptionCosmosDbClient = app.ApplicationServices.GetRequiredService<IPushSubscriptionCosmosDbClient>();

            pushSubscriptionCosmosDbClient.EnsureCreatedAsync().GetAwaiter().GetResult();

            return app;
        }
    }
}
