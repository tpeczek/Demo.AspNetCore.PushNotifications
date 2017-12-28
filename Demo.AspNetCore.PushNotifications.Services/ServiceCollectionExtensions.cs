using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;
using Demo.AspNetCore.PushNotifications.Services.Sqlite;
using Demo.AspNetCore.PushNotifications.Services.WebPush;

namespace Demo.AspNetCore.PushNotifications.Services
{
    public static class ServiceCollectionExtensions
    {
        private const string PUSH_NOTIFICATION_SERVICE_CONFIGURATION_SECTION = "PushNotificationService";

        public static IServiceCollection AddPushSubscriptionStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSqlitePushSubscriptionStore(configuration);
            
            return services;
        }

        public static IServiceCollection AddPushNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<PushNotificationServiceOptions>(configuration.GetSection(PUSH_NOTIFICATION_SERVICE_CONFIGURATION_SECTION));

            services.AddWebPushPushNotificationService();

            return services;
        }
    }
}
