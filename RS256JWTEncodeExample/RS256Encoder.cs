using JWT.Algorithms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using JWT;
using JWT.Serializers;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace RS256JWTEncodeExample
{
    public static class RS256Encoder
    {
        public static String pathToPemFile = String.Empty;
        static RS256Encoder()
        {
            pathToPemFile = File.ReadAllText(@"path to your .pem file");
        }

        public static String GetSignBodyPayloadForRequest(object payload)
        {
            RSAParameters rsaParams = GetRsaParameters();
            IJwtEncoder encoder = GetRS256JWTEncoder(rsaParams);

            String token = encoder.Encode(null, payload, new byte[0]);
            return token;
        }

        private static RSAParameters GetRsaParameters()
        {
            using (StringReader tr = new StringReader(pathToPemFile))
            {
                PemReader pemReader = new PemReader(tr);

                //becouse bug here -- but it's working for me =)
                while (tr.Peek() != -1)
                {
                    if (pemReader.ReadObject() is RsaPrivateCrtKeyParameters parameter)
                    {
                        return DotNetUtilities.ToRSAParameters(parameter);
                    }
                }

                return new RSAParameters();
            }
        }

        private static IJwtEncoder GetRS256JWTEncoder(RSAParameters rsaParams)
        {
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParams);

            RS256Algorithm algorithm = new RS256Algorithm(csp, csp);
            JsonNetSerializer serializer = new JsonNetSerializer();
            JwtBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            JwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder;
        }
    }
}
