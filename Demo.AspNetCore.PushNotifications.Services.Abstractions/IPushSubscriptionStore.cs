using System;
using System.Threading.Tasks;
using Lib.Net.Http.WebPush;

namespace Demo.AspNetCore.PushNotifications.Services.Abstractions
{
    public interface IPushSubscriptionStore
    {
        Task StoreSubscriptionAsync(PushSubscription subscription);

        Task DiscardSubscriptionAsync(string endpoint);

        Task ForEachSubscriptionAsync(Action<PushSubscription> action);
    }
}
