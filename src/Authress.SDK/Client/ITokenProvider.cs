using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

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

        /// <summary>
        /// Provides the token from locally stored access key. Access key can be retrieved when creating a new client in the Authress UI.
        /// </summary>
        public AuthressClientTokenProvider(string accessKeyBase64)
        {
            var buffer = System.Convert.FromBase64String(accessKeyBase64);
            var accessKeyAsString = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
            this.accessKey = JsonConvert.DeserializeObject<AccessKey>(accessKeyAsString);
        }

        /// <summary>
        /// Get the bearer token
        /// </summary>
        public Task<string> GetBearerToken()
        {
            var privateKey = new X509Certificate2(accessKey.PrivateKey);
            var jwtManager = new JwtSecurityTokenHandler();
            var signingOptions = new SecurityTokenDescriptor
            {
                Issuer = $"AuthressServiceClient:{accessKey.ClientId}",
                Audience = accessKey.Audience,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new X509SecurityKey(privateKey, accessKey.KeyId), "RS256"),
                Claims = new Dictionary<string, object>
                {
                    { "sub",  accessKey.ClientId },
                    { "scopes" , "openid" }
                }
            };
            return Task.FromResult(jwtManager.CreateEncodedJwt(signingOptions));
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
    public class ResolverTokenProvider : ITokenProvider
    {
        private Func<Task<string>> resolver;

        /// <summary>
        /// Provides the token from a provider function. Specify a resolver function that when execution will return a bearer token to be used with Authress.
        /// The expectation is that this resolver could grab the current user's JWT from the incoming service call and pass it to the outgoing Authress service call.
        /// <param name="resolver">A function that will be called to resolve a token.</param>
        /// </summary>
        public ResolverTokenProvider(Func<Task<string>> resolver)
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
