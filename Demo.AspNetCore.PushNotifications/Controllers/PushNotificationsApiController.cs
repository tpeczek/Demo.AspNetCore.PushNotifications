using Microsoft.AspNetCore.Mvc;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;
using System.Threading.Tasks;

namespace Demo.AspNetCore.PushNotifications.Controllers
{
    [Route("push-notifications-api")]
    public class PushNotificationsApiController : Controller
    {
        private readonly IPushSubscriptionStore _subscriptionStore;
        private readonly IPushNotificationService _notificationService;

        public PushNotificationsApiController(IPushSubscriptionStore subscriptionStore, IPushNotificationService notificationService)
        {
            _subscriptionStore = subscriptionStore;
            _notificationService = notificationService;
        }

        // GET push-notifications-api/public-key
        [HttpGet("public-key")]
        public ContentResult GetPublicKey()
        {
            return Content(_notificationService.PublicKey, "text/plain");
        }

        // POST push-notifications-api/subscriptions
        [HttpPost("subscriptions")]
        public async Task<IActionResult> StoreSubscription([FromBody]PushSubscription subscription)
        {
            await _subscriptionStore.StoreSubscriptionAsync(subscription);

            return NoContent();
        }

        // DELETE push-notifications-api/subscriptions?endpoint={endpoint}
        [HttpDelete("subscriptions")]
        public async Task<IActionResult> DiscardSubscription(string endpoint)
        {
            await _subscriptionStore.DiscardSubscriptionAsync(endpoint);

            return NoContent();
        }

        // POST push-notifications-api/notifications
        [HttpPost("notifications")]
        public async Task<IActionResult> SendNotification([FromBody]string payload)
        {
            // TODO: This should be scheduled in background
            await _subscriptionStore.ForEachSubscriptionAsync((PushSubscription subscription) => _notificationService.SendNotification(subscription, payload));

            return NoContent();
        }
    }
}
