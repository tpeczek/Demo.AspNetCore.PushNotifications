using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebPush = Lib.Net.Http.WebPush;

namespace Demo.AspNetCore.PushNotifications.Services.Sqlite
{
    internal class PushSubscriptionContext : DbContext
    {
        public class PushSubscription : WebPush.PushSubscription
        {
            public string P256DH
            {
                get { return GetKey(WebPush.PushEncryptionKeyName.P256DH); }

                set { SetKey(WebPush.PushEncryptionKeyName.P256DH, value); }
            }

            public string Auth
            {
                get { return GetKey(WebPush.PushEncryptionKeyName.Auth); }

                set { SetKey(WebPush.PushEncryptionKeyName.Auth, value); }
            }

            public PushSubscription()
            { }

            public PushSubscription(WebPush.PushSubscription subscription)
            {
                Endpoint = subscription.Endpoint;
                Keys = subscription.Keys;
            }
        }

        public DbSet<PushSubscription> Subscriptions { get; set; }

        public PushSubscriptionContext(DbContextOptions<PushSubscriptionContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<PushSubscription> pushSubscriptionEntityTypeBuilder = modelBuilder.Entity<PushSubscription>();
            pushSubscriptionEntityTypeBuilder.HasKey(e => e.Endpoint);
            pushSubscriptionEntityTypeBuilder.Ignore(p => p.Keys);
        }
    }
}
