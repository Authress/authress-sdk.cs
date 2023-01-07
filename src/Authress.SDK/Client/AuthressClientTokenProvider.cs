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
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using ScottBrady.IdentityModel.Crypto;
using ScottBrady.IdentityModel.Tokens;

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

        /// <summary>
        /// Provides the token from locally stored access key. Access key can be retrieved when creating a new client in the Authress UI.
        /// </summary>
        public AuthressClientTokenProvider(string accessKeyBase64, string authressCustomDomain = null)
        {
            try
            {
                var buffer = System.Convert.FromBase64String(accessKeyBase64);
                var accessKeyAsString = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
                this.accessKey = JsonConvert.DeserializeObject<AccessKey>(accessKeyAsString);
                var accountId = this.accessKey.Audience.Split('.')[0];
                this.authressCustomDomain = (authressCustomDomain ?? $"{accountId}.api.authress.io").Replace("https://", "");
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

                this.authressCustomDomain = (authressCustomDomain ?? $"{accountId}.api.authress.io").Replace("https://", "");
            }
        }

        private string GetIssuer()
        {
            return $"https://{this.authressCustomDomain}/v1/clients/{System.Web.HttpUtility.UrlEncode(this.accessKey.ClientId)}";
        }

        private static SigningCredentials GetSigningCredentials(string pem, string keyId)
        {
            var pr = new PemReader(new StringReader(pem));
            var rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)pr.ReadObject());
            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParams);
            return new SigningCredentials(new RsaSecurityKey(csp.ExportParameters(true)) { KeyId = keyId }, SecurityAlgorithms.RsaSha256);
        }

        private static SigningCredentials GetSigningCredentialsEdDSA(string pem, string keyId)
        {
            return new SigningCredentials(new EdDsaSecurityKey(new Ed25519PrivateKeyParameters(Encoding.UTF8.GetBytes(pem), 0)) { KeyId = keyId }, ExtendedSecurityAlgorithms.EdDsa);
        }

        /// <summary>
        /// Get the bearer token
        /// </summary>
        public Task<string> GetBearerToken()
        {
            if (token != null && tokenExpiryDate > DateTime.UtcNow.AddHours(1))
            {
                return Task.FromResult(token);
            }

            var expiryDate = DateTime.UtcNow.AddDays(1);
            if (accessKey == null || accessKey.PrivateKey == null)
            {
                throw new ArgumentNullException("Invalid access key provided");
            }

            var signingCredentials = accessKey.Algorithm == "RS256" ? GetSigningCredentials(accessKey.PrivateKey, accessKey.KeyId) : GetSigningCredentialsEdDSA(accessKey.PrivateKey, accessKey.KeyId);
            var signingOptions = new SecurityTokenDescriptor
            {
                Issuer = GetIssuer(),
                Audience = accessKey.Audience,
                NotBefore = DateTime.UtcNow,
                Expires = expiryDate,
                SigningCredentials = signingCredentials,
                AdditionalHeaderClaims = new Dictionary<string, object> { { "kid",  accessKey.KeyId } },
                Subject = new System.Security.Claims.ClaimsIdentity(new [] {
                    new Claim("sub", accessKey.ClientId),
                    new Claim("scopes", "openid")
                })

            };
            var jwtManager = new JwtSecurityTokenHandler();
            token = jwtManager.CreateEncodedJwt(signingOptions);
            tokenExpiryDate = expiryDate;

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

            if (string.IsNullOrEmpty(this.accessKey.ClientId))
            {
                throw new ArgumentNullException("The clientId specifying the origin of the authentication request. This should match the service client ID");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("The user to generate a authorization code redirect for is required.");
            }

            var expiryDate = DateTime.UtcNow.AddSeconds(60);
            var signingCredentials = GetSigningCredentialsEdDSA(accessKey.PrivateKey, accessKey.KeyId);
            var issuer = GetIssuer();
            var signingOptions = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = accessKey.Audience,
                NotBefore = DateTime.UtcNow,
                Expires = expiryDate,
                SigningCredentials = signingCredentials,
                TokenType = "oauth-authz-req+jwt",
                AdditionalHeaderClaims = new Dictionary<string, object> { { "kid",  accessKey.KeyId } },
                Subject = new System.Security.Claims.ClaimsIdentity(new [] {
                    new Claim("sub", userId),
                    new Claim("scopes", "openid"),
                    new Claim("max_age", "60", ClaimValueTypes.Integer),
                    new Claim("client_id", this.accessKey.ClientId)
                })

            };
            var jwtManager = new JwtSecurityTokenHandler();
            var code = jwtManager.CreateEncodedJwt(signingOptions);

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
