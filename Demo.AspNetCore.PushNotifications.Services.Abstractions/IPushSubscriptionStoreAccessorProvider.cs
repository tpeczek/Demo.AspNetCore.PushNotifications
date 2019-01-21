namespace Demo.AspNetCore.PushNotifications.Services.Abstractions
{
    public interface IPushSubscriptionStoreAccessorProvider
    {
        IPushSubscriptionStoreAccessor GetPushSubscriptionStoreAccessor();
    }
}
