using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;
using Demo.AspNetCore.PushNotifications.Services.Sqlite;
using Demo.AspNetCore.PushNotifications.Services.WebPush;
using Demo.AspNetCore.PushNotifications.Services.PushService;

namespace Demo.AspNetCore.PushNotifications.Services
{
    public static class ServiceCollectionExtensions
    {
        private const string PUSH_NOTIFICATION_SERVICE_CONFIGURATION_SECTION = "PushNotificationService";
        private const string PUSH_NOTIFICATION_SERVICE_TYPE_CONFIGURATION_KEY = "PushNotificationServiceType";
        private const string PUSH_NOTIFICATION_SERVICE_TYPE_WEBPUSH = "WebPush";
        private const string PUSH_NOTIFICATION_SERVICE_TYPE_PUSHSERVICE = "PushService";

        public static IServiceCollection AddPushSubscriptionStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSqlitePushSubscriptionStore(configuration);
            
            return services;
        }

        public static IServiceCollection AddPushNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<PushNotificationServiceOptions>(configuration.GetSection(PUSH_NOTIFICATION_SERVICE_CONFIGURATION_SECTION));

            string pushNotificationServiceType = configuration.GetValue(PUSH_NOTIFICATION_SERVICE_TYPE_CONFIGURATION_KEY, PUSH_NOTIFICATION_SERVICE_TYPE_WEBPUSH);

            if (PUSH_NOTIFICATION_SERVICE_TYPE_WEBPUSH.Equals(pushNotificationServiceType, StringComparison.InvariantCultureIgnoreCase))
            {
                services.AddWebPushPushNotificationService();
            }
            else if (PUSH_NOTIFICATION_SERVICE_TYPE_PUSHSERVICE.Equals(pushNotificationServiceType, StringComparison.InvariantCultureIgnoreCase))
            {
                services.AddPushServicePushNotificationService();
            }
            else
            {
                throw new NotSupportedException($"Not supported {nameof(IPushNotificationService)} type: '{pushNotificationServiceType}'");
            }

            return services;
        }
    }
}
