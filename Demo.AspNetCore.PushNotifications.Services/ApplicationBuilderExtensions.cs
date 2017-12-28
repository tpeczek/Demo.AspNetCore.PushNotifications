using Microsoft.AspNetCore.Builder;
using Demo.AspNetCore.PushNotifications.Services.Sqlite;

namespace Demo.AspNetCore.PushNotifications.Services
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UsePushSubscriptionStore(this IApplicationBuilder app)
        {
            app.UseSqlitePushSubscriptionStore();

            return app;
        }
    }
}
