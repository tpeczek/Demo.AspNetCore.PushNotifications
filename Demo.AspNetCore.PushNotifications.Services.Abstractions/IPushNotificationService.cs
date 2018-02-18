using System.Threading.Tasks;
using Lib.Net.Http.WebPush;

namespace Demo.AspNetCore.PushNotifications.Services.Abstractions
{
    public interface IPushNotificationService
    {
        string PublicKey { get; }

        Task SendNotificationAsync(PushSubscription subscription, PushMessage message);
    }
}
