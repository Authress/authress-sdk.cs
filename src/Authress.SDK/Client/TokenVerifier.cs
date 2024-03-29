using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Authress.SDK.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Authress.SDK.Utilities;
using System.Text.RegularExpressions;
using NSec.Cryptography;
using System.Text;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.OpenSsl;
using System.IO;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;

namespace Authress.SDK
{

    /// <summary>
    /// The verified User identity that can be trusted because it was generated by a verified access token.
    /// </summary>
    public class VerifiedUserIdentity {
        /// <summary>
        ///
        /// </summary>
        public string UserId { get; set; }
    }

    /// <summary>
    /// There was an issue with token verification and user is should be treated as Unauthorized.
    /// </summary>
    public class TokenVerificationException : Exception {

        internal TokenVerificationException(String message) : base(message) {}
    }


    [JsonConverter(typeof(StringEnumConverter))]
    internal enum Alg {
        [EnumMember(Value = "EdDSA")] EdDSA = 1,
        [EnumMember(Value = "RS256")] RS256 = 2,
        [EnumMember(Value = "RS512")] RS512 = 3
    }

    [DataContract]
    internal class Jwk {
        [DataMember]
        public string kid { get; set; }

        [DataMember(Name="alg", EmitDefaultValue=false)]
        public Alg Alg { get; set; }

        [DataMember]
        public string kty { get; set; }

        [DataMember]
        public string crv { get; set; }

        // EdDSA
        [DataMember]
        public string x { get; set; }

        // RSA
        [DataMember]
        public string n { get; set; }
        [DataMember]
        public string e { get; set; }
    }

    [DataContract]
    internal class JwkResponse {
        [DataMember(Name="keys", EmitDefaultValue=false)]
        public IList<Jwk> Keys;
    }

    internal class TokenVerifier {
        private static readonly IMemoryCache jwkCache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 5000 });
        private readonly string authressCustomDomain;
        private readonly HttpClientProvider authressHttpClientProvider;

        internal TokenVerifier(string authressCustomDomain, HttpClientProvider authressHttpClientProvider) {
            this.authressCustomDomain = authressCustomDomain;
            this.authressHttpClientProvider = authressHttpClientProvider;
        }

        private async Task<Jwk> getPublicKey(string issuer, string keyId) {
            var cacheKey = $"{issuer}|{keyId}";

            if (jwkCache.TryGetValue(cacheKey, out Jwk cachedJwk))
            {
                if (cachedJwk != null)
                {
                    return cachedJwk;
                }
            }

            JwkResponse jwkResponse;

            var path = $"{issuer}/.well-known/openid-configuration/jwks";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                jwkResponse = await response.Content.ReadAsAsync<JwkResponse>();
            }

            var foundKey = jwkResponse.Keys.FirstOrDefault(k => k.kid == keyId);
            if (foundKey == null) {
                throw new TokenVerificationException("No matching public key found for token");
            }

            jwkCache.Set(cacheKey, foundKey, new MemoryCacheEntryOptions {
                SlidingExpiration = TimeSpan.FromDays(30),
                Size = 1
            });

            return foundKey;
        }

        public async Task<VerifiedUserIdentity> VerifyToken(string authorizationHeaderValue) {
            if (string.IsNullOrEmpty(authorizationHeaderValue)) {
                throw new ArgumentNullException("Unauthorized: Token not specified");
            }

            var jwtToken = authorizationHeaderValue.Trim().Replace("Bearer ", string.Empty, StringComparison.InvariantCultureIgnoreCase);

            var jwtHeader = JsonConvert.DeserializeObject<Client.JWT.JwtHeader>(Base64UrlEncoder.Decode(jwtToken.Split('.')[0]));
            var unverifiedJwtPayload = JsonConvert.DeserializeObject<Client.JWT.JwtPayload>(Base64UrlEncoder.Decode(jwtToken.Split('.')[1]));

            if (string.IsNullOrEmpty(jwtHeader.KeyId)) {
                throw new TokenVerificationException("Unauthorized: No KID in token");
            }

            if (string.IsNullOrEmpty(unverifiedJwtPayload.Issuer)) {
                throw new TokenVerificationException("Unauthorized: No Issuer found");
            }

            if (unverifiedJwtPayload.Expires < DateTime.UtcNow) {
                throw new TokenVerificationException("Unauthorized: Token is expired");
            }

            if (string.IsNullOrEmpty(authressCustomDomain)) {
                throw new ArgumentNullException("The authress custom domain must be specified in the AuthressSettings.");
            }

            var completeIssuerUrl = new Uri(Sanitizers.SanitizeIssuerUrl(authressCustomDomain));
            var altIssuerUrl = new Uri(Sanitizers.SanitizeUrl(authressCustomDomain));
            try {
                if (new Uri(unverifiedJwtPayload.Issuer).GetLeftPart(UriPartial.Authority) != completeIssuerUrl.GetLeftPart(UriPartial.Authority)
                    && new Uri(unverifiedJwtPayload.Issuer).GetLeftPart(UriPartial.Authority) != altIssuerUrl.GetLeftPart(UriPartial.Authority)) {
                    throw new TokenVerificationException($"Unauthorized: Invalid Issuer: {unverifiedJwtPayload.Issuer}");
                }
            } catch (Exception) {
                throw new TokenVerificationException($"Unauthorized: Invalid Issuer: {unverifiedJwtPayload.Issuer}");
            }

            // Handle service client checking
            var clientIdMatcher = Regex.Match(new Uri(unverifiedJwtPayload.Issuer).AbsolutePath, @"^/v\d/clients/(?<ServiceClientId>[^/]+)$");
            if (clientIdMatcher.Success && clientIdMatcher.Groups["ServiceClientId"].Value != unverifiedJwtPayload.Subject) {
                throw new TokenVerificationException($"Unauthorized: Invalid Sub found for service client token: {unverifiedJwtPayload.Subject}");
            }

            var key = await getPublicKey(unverifiedJwtPayload.Issuer, jwtHeader.KeyId);

            var verifiedUserIdentity = VerifySignature(jwtToken, key);
            return verifiedUserIdentity;
        }

        private byte[] ConvertFromBase64Url(string base64String) {
            var result = base64String.Replace('_', '/').Replace('-', '+');
            switch(result.Length % 4) {
                case 2: result += "=="; break;
                case 3: result += "="; break;
            }

            return Convert.FromBase64String(result);
        }

        private VerifiedUserIdentity VerifySignature(string jwtToken, Jwk key) {

            var unverifiedJwtPayload = JsonConvert.DeserializeObject<Client.JWT.JwtPayload>(Base64UrlEncoder.Decode(jwtToken.Split('.')[1]));

            if (key.Alg == Alg.EdDSA) {
                var ed25519alg = SignatureAlgorithm.Ed25519;

                var data = Encoding.UTF8.GetBytes($"{jwtToken.Split('.')[0]}.{jwtToken.Split('.')[1]}");

                var edDsaPublicKey = NSec.Cryptography.PublicKey.Import(ed25519alg, ConvertFromBase64Url("MCowBQYDK2VwAyEA" + key.x), KeyBlobFormat.PkixPublicKey);
                var signatureData = ConvertFromBase64Url(jwtToken.Split('.')[2]);
                if (!SignatureAlgorithm.Ed25519.Verify(edDsaPublicKey, data, signatureData)) {
                    throw new TokenVerificationException($"Unauthorized: Token Signature is not valid.");
                }

                return new VerifiedUserIdentity {
                    UserId = unverifiedJwtPayload.Subject
                };
            }

            // ELSE assume RS256 or RS512
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(new RSAParameters()
                {
                    Modulus = ConvertFromBase64Url(key.n),
                    Exponent = ConvertFromBase64Url(key.e)
                });

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = unverifiedJwtPayload.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa)
            };

            SecurityToken validatedTokenIdentity;
            IPrincipal principal;
            try
            {
                principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out validatedTokenIdentity);
            }
            catch (Exception exception)
            {
                throw new TokenVerificationException($"Unauthorized: {exception.Message}");
            }

            if (principal == null) {
                throw new TokenVerificationException($"Unauthorized: Invalid token");
            }

            var verifiedJwtPayload = validatedTokenIdentity as JwtSecurityToken;

            return new VerifiedUserIdentity {
                UserId = verifiedJwtPayload.Subject
            };
        }
    }
}
