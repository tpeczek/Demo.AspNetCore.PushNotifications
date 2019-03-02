using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;
using Demo.AspNetCore.PushNotifications.Services.Sqlite;
using Demo.AspNetCore.PushNotifications.Services.PushService;

namespace Demo.AspNetCore.PushNotifications.Services
{
    public static class ServiceCollectionExtensions
    {
        

        public static IServiceCollection AddPushSubscriptionStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSqlitePushSubscriptionStore(configuration);
            
            return services;
        }

        public static IServiceCollection AddPushNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.AddPushServicePushNotificationService(configuration);

            return services;
        }

        public static IServiceCollection AddPushNotificationsQueue(this IServiceCollection services)
        {
            services.AddSingleton<IPushNotificationsQueue, PushNotificationsQueue>();
            services.AddSingleton<IHostedService, PushNotificationsDequeuer>();

            return services;
        }
    }
}
