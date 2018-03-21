using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Lib.Net.Http.WebPush;
using Demo.AspNetCore.PushNotifications.Services.Abstractions;

namespace Demo.AspNetCore.PushNotifications.Services
{
    internal class PushNotificationsQueue : IPushNotificationsQueue
    {
        private readonly ConcurrentQueue<PushMessage> _messages = new ConcurrentQueue<PushMessage>();
        private readonly SemaphoreSlim _messageEnqueuedSignal = new SemaphoreSlim(0);

        public void Enqueue(PushMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _messages.Enqueue(message);

            _messageEnqueuedSignal.Release();
        }

        public async Task<PushMessage> DequeueAsync(CancellationToken cancellationToken)
        {
            await _messageEnqueuedSignal.WaitAsync(cancellationToken);

            _messages.TryDequeue(out PushMessage message);

            return message;
        }
    }
}
