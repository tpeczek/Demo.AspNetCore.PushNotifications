using System;

namespace Demo.AspNetCore.PushNotifications.Services.Abstractions
{
    public interface IPushSubscriptionStoreAccessor : IDisposable
    {
        IPushSubscriptionStore PushSubscriptionStore { get; }
    }
}
