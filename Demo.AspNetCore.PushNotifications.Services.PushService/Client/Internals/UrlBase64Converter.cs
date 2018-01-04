using System;

namespace Demo.AspNetCore.PushNotifications.Services.PushService.Client.Internals
{
    internal static class UrlBase64Converter
    {
        internal static byte[] FromUrlBase64String(string input)
        {
            input = input.Replace('-', '+').Replace('_', '/');

            while (input.Length % 4 != 0)
            {
                input += "=";
            }

            return Convert.FromBase64String(input);
        }

        internal static string ToUrlBase64String(byte[] input)
        {
            return Convert.ToBase64String(input).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }
    }
}
