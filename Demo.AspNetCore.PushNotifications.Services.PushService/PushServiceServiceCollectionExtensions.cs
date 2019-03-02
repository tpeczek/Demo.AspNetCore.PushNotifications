using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lib.Net.Http.WebPush;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;

namespace Demo.AspNetCore.PushNotifications.Services.PushService
{
    public static class PushServiceServiceCollectionExtensions
    {
        public static IServiceCollection AddPushServicePushNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddMemoryVapidTokenCache();
            services.AddPushServiceClient(options =>
            {
                IConfigurationSection pushNotificationServiceConfigurationSection = configuration.GetSection(nameof(PushServiceClient));

                options.Subject = pushNotificationServiceConfigurationSection.GetValue<string>(nameof(options.Subject));
                options.PublicKey = pushNotificationServiceConfigurationSection.GetValue<string>(nameof(options.PublicKey));
                options.PrivateKey = pushNotificationServiceConfigurationSection.GetValue<string>(nameof(options.PrivateKey));
            });
            services.AddTransient<IPushNotificationService, PushServicePushNotificationService>();

            return services;
        }
    }
}
