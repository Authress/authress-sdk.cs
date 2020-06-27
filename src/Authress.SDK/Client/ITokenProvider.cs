using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Authress.SDK
{
    /// <summary>
    /// Provides the token necessary for Authress authentication
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// Get the bearer token
        /// </summary>
        Task<string> GetBearerToken();
    }

    /// <summary>
    /// Provides the token from locally stored access key. Access key can be retrieved when creating a new client in the Authress UI.
    /// </summary>
    public class AuthressClientTokenProvider : ITokenProvider
    {
        private readonly AccessKey accessKey;
        private string token { get; set; }
        private DateTime tokenExpiryDate { get; set; }

        /// <summary>
        /// Provides the token from locally stored access key. Access key can be retrieved when creating a new client in the Authress UI.
        /// </summary>
        public AuthressClientTokenProvider(string accessKeyBase64)
        {
            var buffer = System.Convert.FromBase64String(accessKeyBase64);
            var accessKeyAsString = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
            this.accessKey = JsonConvert.DeserializeObject<AccessKey>(accessKeyAsString);
        }

        private static RSAParameters ImportPrivateKey(string pem) {
            var pr = new PemReader(new StringReader(pem));
            var rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)pr.ReadObject());
            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParams);
            return csp.ExportParameters(true);
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

            var rsaParameters = ImportPrivateKey(accessKey.PrivateKey);
            var jwtManager = new JwtSecurityTokenHandler();
            var signingOptions = new SecurityTokenDescriptor
            {
                Issuer = $"https://api.authress.io/v1/clients/{System.Web.HttpUtility.UrlEncode(accessKey.ClientId)}",
                Audience = accessKey.Audience,
                NotBefore = DateTime.UtcNow,
                Expires = expiryDate,
                SigningCredentials = new SigningCredentials(new RsaSecurityKey(rsaParameters) { KeyId = accessKey.KeyId }, "RS256"),
                AdditionalHeaderClaims = new Dictionary<string, object> { { "kid",  accessKey.KeyId } },
                Subject = new System.Security.Claims.ClaimsIdentity(new [] {
                    new Claim("sub", accessKey.ClientId),
                    new Claim("scopes", "openid")
                })

            };
            token = jwtManager.CreateEncodedJwt(signingOptions);
            tokenExpiryDate = expiryDate;

            return Task.FromResult(token);
        }
    }

    /// <summary>
    /// Provides the token from locally stored access key.
    /// </summary>
    public class ManualTokenProvider : ITokenProvider
    {
        private string token;

        /// <summary>
        /// Set the bearer token
        /// </summary>
        public void SetToken(string token)
        {
            this.token = token;
        }

        /// <summary>
        /// Get the bearer token
        /// </summary>
        public Task<string> GetBearerToken()
        {
            return Task.FromResult(token);
        }
    }

    /// <summary>
    /// Provides the token from a provider function.
    /// </summary>
    public class DynamicTokenProvider : ITokenProvider
    {
        private Func<Task<string>> resolver;

        /// <summary>
        /// Provides the token from a provider function. Specify a resolver function that when execution will return a bearer token to be used with Authress.
        /// The expectation is that this resolver could grab the current user's JWT from the incoming service call and pass it to the outgoing Authress service call.
        /// <param name="resolver">A function that will be called to resolve a token.</param>
        /// </summary>
        public DynamicTokenProvider(Func<Task<string>> resolver)
        {
            this.resolver = resolver;
        }

        /// <summary>
        /// Get the bearer token
        /// </summary>
        public async Task<string> GetBearerToken()
        {
            var token = await resolver();
            return token;
        }
    }
}
