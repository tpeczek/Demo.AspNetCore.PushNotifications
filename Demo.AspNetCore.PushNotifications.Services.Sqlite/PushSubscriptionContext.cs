using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.AspNetCore.PushNotifications.Services.Sqlite
{
    internal class PushSubscriptionContext : DbContext
    {
        public class PushSubscription : Abstractions.PushSubscription
        {
            public string P256DH
            {
                get { return GetKey(Abstractions.PushEncryptionKeyName.P256DH); }

                set { SetKey(Abstractions.PushEncryptionKeyName.P256DH, value); }
            }

            public string Auth
            {
                get { return GetKey(Abstractions.PushEncryptionKeyName.Auth); }

                set { SetKey(Abstractions.PushEncryptionKeyName.Auth, value); }
            }

            public PushSubscription()
            { }

            public PushSubscription(Abstractions.PushSubscription subscription)
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
