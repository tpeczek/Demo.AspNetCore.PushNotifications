using Lib.Net.Http.WebPush;

namespace Demo.AspNetCore.PushNotifications.Services.Abstractions
{
    public interface IPushNotificationService
    {
        string PublicKey { get; }

        void SendNotification(PushSubscription subscription, PushMessage message);
    }
}
