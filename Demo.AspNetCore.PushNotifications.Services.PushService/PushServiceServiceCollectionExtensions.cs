using Microsoft.Extensions.DependencyInjection;
using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;

namespace Demo.AspNetCore.PushNotifications.Services.PushService
{
    public static class PushServiceServiceCollectionExtensions
    {
        public static IServiceCollection AddPushServicePushNotificationService(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IVapidTokenCache, MemoryVapidTokenCache>();
            services.AddHttpClient<PushServiceClient>();
            services.AddTransient<IPushNotificationService, PushServicePushNotificationService>();

            return services;
        }
    }
}
