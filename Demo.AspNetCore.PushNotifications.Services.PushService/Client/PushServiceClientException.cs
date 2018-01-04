using System;
using System.Net;

namespace Demo.AspNetCore.PushNotifications.Services.PushService.Client
{
    public class PushServiceClientException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public PushServiceClientException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
