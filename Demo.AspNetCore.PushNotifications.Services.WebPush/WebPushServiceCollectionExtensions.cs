using Microsoft.Extensions.DependencyInjection;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;

namespace Demo.AspNetCore.PushNotifications.Services.WebPush
{
    public static class WebPushServiceCollectionExtensions
    {
        public static IServiceCollection AddWebPushPushNotificationService(this IServiceCollection services)
        {
            services.AddSingleton<IPushNotificationService, WebPushPushNotificationService>();

            return services;
        }
    }
}
