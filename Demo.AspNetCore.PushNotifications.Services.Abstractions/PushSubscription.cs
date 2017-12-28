using System.Collections.Generic;

namespace Demo.AspNetCore.PushNotifications.Services.Abstractions
{
    public class PushSubscription
    {
        public string Endpoint { get; set; }

        public IDictionary<string, string> Keys { get; set; }

        public string GetKey(PushEncryptionKeyName keyName)
        {
            string key = null;

            if (Keys != null)
            {
                string keyNameStringified = StringifyKeyName(keyName);

                if (Keys.ContainsKey(keyNameStringified))
                {
                    key = Keys[keyNameStringified];
                }
            }

            return key;
        }

        public void SetKey(PushEncryptionKeyName keyName, string key)
        {
            if (Keys == null)
            {
                Keys = new Dictionary<string, string>();
            }

            Keys[StringifyKeyName(keyName)] = key;
        }

        private string StringifyKeyName(PushEncryptionKeyName keyName)
        {
            return keyName.ToString().ToLowerInvariant();
        }
    }
}
