using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
                .AddMvc(options =>
                {
                    options.InputFormatters.Add(new TextPlainInputFormatter());
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
                .UseMvc()
                .Run(async (context) =>
                {
                    await context.Response.WriteAsync("-- Demo.AspNetCore.PushNotifications --");
                });
        }
    }
}
