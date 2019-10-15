using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Demo.AspNetCore.PushNotifications.Services;
using Demo.AspNetCore.PushNotifications.Formatters;

namespace Demo.AspNetCore.PushNotifications
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPushSubscriptionStore(Configuration)
                .AddPushNotificationService(Configuration)
                .AddPushNotificationsQueue();

            services.AddControllersWithViews(options =>
            {
                options.InputFormatters.Add(new TextPlainInputFormatter());
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("push-notifications.html");

            app.UseDefaultFiles(defaultFilesOptions)
                .UseStaticFiles()
                .UsePushSubscriptionStore()
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
