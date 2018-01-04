using System;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Demo.AspNetCore.PushNotifications.Services.PushService.Client.Internals
{
    internal static class ECKeyHelper
    {
        internal static ECPrivateKeyParameters GetECPrivateKeyParameters(byte[] privateKey)
        {
            Asn1Object derSequence = new DerSequence(
                new DerInteger(1),
                new DerOctetString(privateKey),
                new DerTaggedObject(0, new DerObjectIdentifier("1.2.840.10045.3.1.7"))
            );

            string pemKey = "-----BEGIN EC PRIVATE KEY-----\n"
                + Convert.ToBase64String(derSequence.GetDerEncoded())
                + "\n-----END EC PRIVATE KEY----";

            PemReader pemKeyReader = new PemReader(new StringReader(pemKey));
            AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair)pemKeyReader.ReadObject();

            return (ECPrivateKeyParameters)keyPair.Private;
        }

        internal static ECPublicKeyParameters GetECPublicKeyParameters(byte[] publicKey)
        {
            Asn1Object derSequence = new DerSequence(
                new DerSequence(new DerObjectIdentifier(@"1.2.840.10045.2.1"), new DerObjectIdentifier(@"1.2.840.10045.3.1.7")),
                new DerBitString(publicKey)
            );

            string pemKey = "-----BEGIN PUBLIC KEY-----\n"
                + Convert.ToBase64String(derSequence.GetDerEncoded())
                + "\n-----END PUBLIC KEY-----";

            PemReader pemKeyReader = new PemReader(new StringReader(pemKey));
            return (ECPublicKeyParameters)pemKeyReader.ReadObject();
        }

        internal static AsymmetricCipherKeyPair GenerateAsymmetricCipherKeyPair()
        {
            X9ECParameters ecParameters = NistNamedCurves.GetByName("P-256");
            ECDomainParameters ecDomainParameters = new ECDomainParameters(ecParameters.Curve, ecParameters.G, ecParameters.N, ecParameters.H, ecParameters.GetSeed());

            IAsymmetricCipherKeyPairGenerator keyPairGenerator = GeneratorUtilities.GetKeyPairGenerator("ECDH");
            keyPairGenerator.Init(new ECKeyGenerationParameters(ecDomainParameters, new SecureRandom()));

            return keyPairGenerator.GenerateKeyPair();
        }
    }
}
