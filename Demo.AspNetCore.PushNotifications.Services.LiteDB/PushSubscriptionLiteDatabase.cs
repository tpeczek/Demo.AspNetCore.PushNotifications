using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using LiteDB;
using WebPush = Lib.Net.Http.WebPush;

namespace Demo.AspNetCore.PushNotifications.Services.LiteDB
{
    internal class PushSubscriptionLiteDatabase : IPushSubscriptionLiteDatabase, IDisposable
    {
        private class PushSubscription : WebPush.PushSubscription
        {
            public int Id { get; set; }

            public PushSubscription()
            { }

            public PushSubscription(WebPush.PushSubscription subscription)
            {
                Endpoint = subscription.Endpoint;
                Keys = subscription.Keys;
            }
        }

        private const string LITEDB_CONNECTION_STRING_NAME = "PushSubscriptionLiteDBDatabase";
        private const string SUBSCRIPTIONS_COLLECTION_NAME = "subscriptions";

        private readonly LiteDatabase _liteDatabase;
        private readonly LiteCollection<PushSubscription> _subscriptions;

        public PushSubscriptionLiteDatabase(IConfiguration configuration)
        {
            _liteDatabase = new LiteDatabase(configuration.GetConnectionString(LITEDB_CONNECTION_STRING_NAME));
            _subscriptions = _liteDatabase.GetCollection<PushSubscription>(SUBSCRIPTIONS_COLLECTION_NAME);
        }

        public void Add(WebPush.PushSubscription subscription)
        {
            _subscriptions.Insert(new PushSubscription(subscription));
        }

        public void Remove(string endpoint)
        {
            _subscriptions.Delete(subscription => subscription.Endpoint == endpoint);
        }

        public IEnumerable<WebPush.PushSubscription> GetAll()
        {
            return _subscriptions.FindAll();
        }

        public void Dispose()
        {
            _liteDatabase.Dispose();
        }
    }
}
