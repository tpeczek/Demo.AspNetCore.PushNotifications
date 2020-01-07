using System.Collections.Generic;
using Lib.Net.Http.WebPush;

namespace Demo.AspNetCore.PushNotifications.Services.LiteDB
{
    internal interface IPushSubscriptionLiteDatabase
    {
        void Add(PushSubscription subscription);

        void Remove(string endpoint);

        IEnumerable<PushSubscription> GetAll();
    }
}
