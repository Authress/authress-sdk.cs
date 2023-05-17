using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NSec.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Authress.SDK
{
    /// <summary>
    /// Provides the token from locally stored access key. Access key can be retrieved when creating a new client in the Authress UI.
    /// </summary>
    public class AuthressClientTokenProvider : ITokenProvider
    {
        private readonly AccessKey accessKey;
        private string token { get; set; }
        private DateTime tokenExpiryDate { get; set; }
        private readonly string authressCustomDomain;
        private readonly string resolvedAuthressCustomDomain;

        /// <summary>
        /// Provides the token from locally stored access key. Access key can be retrieved when creating a new client in the Authress UI.
        /// </summary>
        public AuthressClientTokenProvider(string accessKeyBase64, string authressCustomDomain = null)
        {
            this.authressCustomDomain = authressCustomDomain;
            try
            {
                var buffer = System.Convert.FromBase64String(accessKeyBase64);
                var accessKeyAsString = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
                this.accessKey = JsonConvert.DeserializeObject<AccessKey>(accessKeyAsString);
                var accountId = this.accessKey.Audience.Split('.')[0];
                this.resolvedAuthressCustomDomain = (authressCustomDomain ?? $"{accountId}.api.authress.io").Replace("https://", "");
            }
            catch (Exception)
            {
                var accountId = accessKeyBase64.Split('.')[2];
                this.accessKey = new AccessKey
                {
                    Algorithm = "EdDSA",
                    ClientId = accessKeyBase64.Split('.')[0], KeyId = accessKeyBase64.Split('.')[1],
                    Audience = $"{accountId}.accounts.authress.io", PrivateKey = accessKeyBase64.Split('.')[3]
                };

                this.resolvedAuthressCustomDomain = (authressCustomDomain ?? $"{accountId}.api.authress.io").Replace("https://", "");
            }
        }

        private string GetIssuer(string authressCustomDomainFallback = null)
        {
            var rawDomain = (this.authressCustomDomain ?? authressCustomDomainFallback ?? resolvedAuthressCustomDomain).Replace("https://", "");
            return $"https://{rawDomain}/v1/clients/{System.Web.HttpUtility.UrlEncode(this.accessKey.ClientId)}";
        }

        private static SigningCredentials GetSigningCredentials(string pem, string keyId)
        {
            var pr = new PemReader(new StringReader(pem));
            var rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)pr.ReadObject());
            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParams);
            return new SigningCredentials(new RsaSecurityKey(csp.ExportParameters(true)) { KeyId = keyId }, SecurityAlgorithms.RsaSha256);
        }

        private static string SignEdDsaToken(IDictionary<string, object> header, IDictionary<string, object> payload, string privateKey)
        {
            var headerString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header))).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            var payloadString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload))).TrimEnd('=').Replace('+', '-').Replace('/', '_');

            var encodedRawJwt = $"{headerString}.{payloadString}";
            var ed25519alg = SignatureAlgorithm.Ed25519;
            var key = Key.Import(ed25519alg, Convert.FromBase64String(privateKey), KeyBlobFormat.PkixPrivateKey);
            var signatureBytes = ed25519alg.Sign(key, Encoding.UTF8.GetBytes(encodedRawJwt));
            var signature = Convert.ToBase64String(signatureBytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            return $"{encodedRawJwt}.{signature}";
        }

        /// <summary>
        /// Get the bearer token
        /// </summary>
        public Task<string> GetBearerToken(string authressCustomDomainFallback = null)
        {
            if (token != null && tokenExpiryDate > DateTime.UtcNow.AddHours(1))
            {
                return Task.FromResult(token);
            }

            var now = DateTime.UtcNow;
            var expiryDate = now.AddDays(1);
            tokenExpiryDate = expiryDate;
            if (accessKey == null || accessKey.PrivateKey == null)
            {
                throw new ArgumentNullException("Invalid access key provided");
            }

            if (accessKey.Algorithm == "RS256")
            {
                var signingOptions = new SecurityTokenDescriptor
                {
                    Issuer = GetIssuer(authressCustomDomainFallback),
                    TokenType = "at+jwt",
                    Audience = accessKey.Audience,
                    NotBefore = DateTime.UtcNow,
                    Expires = expiryDate,
                    SigningCredentials = GetSigningCredentials(accessKey.PrivateKey, accessKey.KeyId),
                    AdditionalHeaderClaims = new Dictionary<string, object> { { "kid",  accessKey.KeyId } },
                    Subject = new System.Security.Claims.ClaimsIdentity(new [] {
                        new Claim("sub", accessKey.ClientId),
                        new Claim("scopes", "openid")
                    })

                };
                var jwtManager = new JwtSecurityTokenHandler();
                token = jwtManager.CreateEncodedJwt(signingOptions);
                return Task.FromResult(token);
            }

            var header = new Dictionary<string, object>
            {
                { "alg", "EdDSA" },
                { "typ", "at+jwt" },
                { "kid", accessKey.KeyId }
            };
            var payload = new Dictionary<string, object>
            {
                { "iss", GetIssuer(authressCustomDomainFallback) },
                { "sub", accessKey.ClientId },
                { "exp", expiryDate.Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds },
                { "iat", now.Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds },
                { "nbf", now.Subtract(TimeSpan.FromMinutes(10)).Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds },
                { "aud", accessKey.Audience },
                { "scopes", "openid" }
            };

            token = SignEdDsaToken(header, payload, accessKey.PrivateKey);
            return Task.FromResult(token);
        }

        /// <summary>
        /// Generate the url to redirect the user back to your application from your authentication server after their credentials have been successfully verified. All these parameters should be found passed through from the user's login attempt along with their credentials. The authentication server receives a request from the user to login, with these values. Then these are constructed and sent back to Authress to verify the generated login data.
        /// </summary>
        /// <param name="authressCustomDomainLoginUrl">The url sent with the request for the user to login, this should match the Authress custom domain: https://authress.io/app/#/setup?focus=domain and end in /login for example https://login.domain.com/login. This value is sent as the `redirect_uri` query string parameter in the request for simplicity.</param>
        /// <param name="state">The state parameter sent to the authentication server.</param>
        /// <param name="userId">The user to request a JWT for.</param>
        /// <returns>Returns a Url. Redirect the Url to this url to complete login.</returns>
        public Task<string> GenerateUserLoginUrl(string authressCustomDomainLoginUrl, string state, string userId)
        {
            if (string.IsNullOrEmpty(authressCustomDomainLoginUrl))
            {
                throw new ArgumentNullException("The authressCustomDomainLoginUrl is specified in the incoming login request, this should match the configured Authress custom domain.");
            }

            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentNullException("The state should match value to generate a authorization code redirect for is required.");
            }

            if (string.IsNullOrEmpty(accessKey.ClientId))
            {
                throw new ArgumentNullException("The clientId specifying the origin of the authentication request. This should match the service client ID");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("The user to generate a authorization code redirect for is required.");
            }

            var now = DateTime.UtcNow;
            var expiryDate = now.AddSeconds(60);
            var issuer = GetIssuer(authressCustomDomainLoginUrl);

            var header = new Dictionary<string, object>
            {
                { "alg", "EdDSA" },
                { "typ", "oauth-authz-req+jwt" },
                { "kid", accessKey.KeyId }
            };
            var payload = new Dictionary<string, object>
            {
                { "iss", issuer },
                { "sub", accessKey.ClientId },
                { "exp", expiryDate.Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds },
                { "iat", now.Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds },
                { "nbf", now.Subtract(TimeSpan.FromMinutes(10)).Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds },
                { "aud", accessKey.Audience },
                { "scopes", "openid" },
                { "max_age", 60 },
                { "client_id", accessKey.ClientId }
            };

            var code = SignEdDsaToken(header, payload, accessKey.PrivateKey);

            var url = new Uri(authressCustomDomainLoginUrl);
            var qs = HttpUtility.ParseQueryString(url.Query);
            qs.Set("code", code);
            qs.Set("iss", issuer);
            qs.Set("state", state);

            var uriBuilder = new UriBuilder(url);
            uriBuilder.Query = qs.ToString();
            return Task.FromResult(uriBuilder.Uri.ToString());
        }
    }
}
