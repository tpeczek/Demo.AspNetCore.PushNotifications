using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Demo.AspNetCore.PushNotifications.Services.PushService.Client.Internals;

namespace Demo.AspNetCore.PushNotifications.Services.PushService.Client.Authentication
{
    internal class VapidAuthentication
    {
        #region Structures
        public readonly struct WebPushSchemeHeadersValues
        {
            public string AuthenticationHeaderValueParameter { get; }

            public string CryptoKeyHeaderValue { get; }

            public WebPushSchemeHeadersValues(string authenticationHeaderValueParameter, string cryptoKeyHeaderValue)
                : this()
            {
                AuthenticationHeaderValueParameter = authenticationHeaderValueParameter;
                CryptoKeyHeaderValue = cryptoKeyHeaderValue;
            }
        }
        #endregion

        #region Fields
        private const string AUDIENCE_CLAIM = "aud";
        private const string EXPIRATION_CLAIM = "exp";
        private const string SUBJECT_CLAIM = "sub";
        private const string P256ECDSA_PREFIX = "p256ecdsa=";
        private const string VAPID_AUTHENTICATION_HEADER_VALUE_PARAMETER_FORMAT = "t={0}, k={1}";
        private const int DEFAULT_EXPIRATION = 43200;
        private const int MAXIMUM_EXPIRATION = 86400;

        private string _subject;
        private string _publicKey;
        private string _privateKey;
        private ECPrivateKeyParameters _privateSigningKey;
        private int _expiration;

        private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
        private static readonly Dictionary<string, string> _jwtHeader = new Dictionary<string, string>
        {
            { "typ", "JWT" },
            { "alg", "ES256" }
        };
        #endregion

        #region Properties
        public string Subject
        {
            get { return _subject; }

            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    if (!value.StartsWith("mailto:"))
                    {
                        if (!Uri.IsWellFormedUriString(value, UriKind.Absolute) || ((new Uri(value)).Scheme != Uri.UriSchemeHttps))
                        {
                            throw new ArgumentException(nameof(Subject), "Subject should include a contact URI for the application server as either a 'mailto: ' (email) or an 'https:' URI");
                        }
                    }

                    _subject = value;
                }
                else
                {
                    _subject = null;
                }
            }
        }

        public string PublicKey
        {
            get { return _publicKey; }

            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(PublicKey));
                }

                byte[] decodedPublicKey = UrlBase64Converter.FromUrlBase64String(value);
                if (decodedPublicKey.Length != 65)
                {
                    throw new ArgumentException(nameof(PublicKey), "VAPID public key must be 65 bytes long");
                }

                _publicKey = value;
            }
        }

        public string PrivateKey
        {
            get { return _privateKey; }

            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(PrivateKey));
                }

                byte[] decodedPrivateKey = UrlBase64Converter.FromUrlBase64String(value);
                if (decodedPrivateKey.Length != 32)
                {
                    throw new ArgumentException(nameof(PrivateKey), "VAPID private key should be 32 bytes long");
                }

                _privateKey = value;
                _privateSigningKey = ECKeyHelper.GetECPrivateKeyParameters(decodedPrivateKey);
            }
        }

        public int Expiration
        {
            get { return _expiration; }

            set
            {
                if ((value <= 0) || (value > MAXIMUM_EXPIRATION))
                {
                    throw new ArgumentOutOfRangeException(nameof(Expiration), "Expiration must be a number of seconds not longer than 24 hours");
                }

                _expiration = value;
            }
        }
        #endregion

        #region Constructor
        public VapidAuthentication(string publicKey, string privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;

            _expiration = DEFAULT_EXPIRATION;
        }
        #endregion

        #region Methods
        public string GetVapidSchemeAuthenticationHeaderValueParameter(string audience)
        {
            return String.Format(VAPID_AUTHENTICATION_HEADER_VALUE_PARAMETER_FORMAT, GetToken(audience), _publicKey);
        }

        public WebPushSchemeHeadersValues GetWebPushSchemeHeadersValues(string audience)
        {
            return new WebPushSchemeHeadersValues(GetToken(audience), P256ECDSA_PREFIX + _publicKey);
        }

        private string GetToken(string audience)
        {
            if (String.IsNullOrWhiteSpace(audience))
            {
                throw new ArgumentNullException(nameof(audience));
            }

            if (!Uri.IsWellFormedUriString(audience, UriKind.Absolute))
            {
                throw new ArgumentException(nameof(audience), "Audience should be an absolute URL");
            }

            Dictionary<string, object> jwtBody = GetJwtBody(audience);

            return GenerateJwtToken(_jwtHeader, jwtBody);
        }

        private Dictionary<string, object> GetJwtBody(string audience)
        {
            Dictionary<string, object> jwtBody = new Dictionary<string, object>
            {
                { AUDIENCE_CLAIM, audience },
                { EXPIRATION_CLAIM, GetAbsoluteExpiration(_expiration) }
            };

            if (_subject != null)
            {
                jwtBody.Add(SUBJECT_CLAIM, _subject);
            }

            return jwtBody;
        }

        private static long GetAbsoluteExpiration(int expirationSeconds)
        {
            TimeSpan unixEpochOffset = DateTime.UtcNow - _unixEpoch;

            return (long)unixEpochOffset.TotalSeconds + expirationSeconds;
        }

        private string GenerateJwtToken(Dictionary<string, string> jwtHeader, Dictionary<string, object> jwtBody)
        {
            string jwtInput = UrlBase64Converter.ToUrlBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jwtHeader)))
                + "."
                + UrlBase64Converter.ToUrlBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jwtBody)));

            byte[] jwtInputHash;
            using (var sha256Hasher = SHA256.Create())
            {
                jwtInputHash = sha256Hasher.ComputeHash(Encoding.UTF8.GetBytes(jwtInput));
            }

            ECDsaSigner jwtSigner = new ECDsaSigner();
            jwtSigner.Init(true, _privateSigningKey);

            BigInteger[] jwtSignature = jwtSigner.GenerateSignature(jwtInputHash);

            byte[] jwtSignatureFirstSegment = jwtSignature[0].ToByteArrayUnsigned();
            byte[] jwtSignatureSecondSegment = jwtSignature[1].ToByteArrayUnsigned();

            int jwtSignatureSegmentLength = Math.Max(jwtSignatureFirstSegment.Length, jwtSignatureSecondSegment.Length);
            byte[] combinedJwtSignature = new byte[2 * jwtSignatureSegmentLength];
            ByteArrayCopyWithPadLeft(jwtSignatureFirstSegment, combinedJwtSignature, 0, jwtSignatureSegmentLength);
            ByteArrayCopyWithPadLeft(jwtSignatureSecondSegment, combinedJwtSignature, jwtSignatureSegmentLength, jwtSignatureSegmentLength);

            return jwtInput + "." + UrlBase64Converter.ToUrlBase64String(combinedJwtSignature);
        }

        private static void ByteArrayCopyWithPadLeft(byte[] sourceArray, byte[] destinationArray, int destinationIndex, int destinationLengthToUse)
        {
            if (sourceArray.Length != destinationLengthToUse)
            {
                destinationIndex += (destinationLengthToUse - sourceArray.Length);
            }

            Array.Copy(sourceArray, 0, destinationArray, destinationIndex, sourceArray.Length);
        }
        #endregion
    }
}
