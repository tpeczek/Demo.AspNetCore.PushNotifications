using System;

namespace Demo.AspNetCore.PushNotifications.Services.PushService.Client
{
    internal class PushMessage
    {
        #region Fields
        private int? _timeToLive;
        #endregion

        #region Properties
        public string Content { get; set; }

        public int? TimeToLive
        {
            get { return _timeToLive; }

            set
            {
                if (value.HasValue && (value.Value < 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(TimeToLive), "The TTL must be a non-negative integer");
                }

                _timeToLive = value;
            }
        }
        #endregion

        #region Constructors
        public PushMessage(string content)
        {
            Content = content;
        }
        #endregion
    }
}
