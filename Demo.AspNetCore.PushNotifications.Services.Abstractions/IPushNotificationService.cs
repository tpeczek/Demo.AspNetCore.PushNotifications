namespace Demo.AspNetCore.PushNotifications.Services.Abstractions
{
    public interface IPushNotificationService
    {
        string PublicKey { get; }

        void SendNotification(PushSubscription subscription, string payload);
    }
}
