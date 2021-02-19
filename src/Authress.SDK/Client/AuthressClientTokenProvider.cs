using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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

        /// <summary>
        /// Provides the token from locally stored access key. Access key can be retrieved when creating a new client in the Authress UI.
        /// </summary>
        public AuthressClientTokenProvider(string accessKeyBase64)
        {
            try
            {
                var buffer = System.Convert.FromBase64String(accessKeyBase64);
                var accessKeyAsString = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
                this.accessKey = JsonConvert.DeserializeObject<AccessKey>(accessKeyAsString);
            }
            catch (Exception)
            {
                this.accessKey = new AccessKey
                {
                    Algorithm = "EdDSA",
                    ClientId = accessKeyBase64.Split('.')[0], KeyId = accessKeyBase64.Split('.')[1],
                    Audience = $"{accessKeyBase64.Split('.')[2]}.accounts.authress.io", PrivateKey = accessKeyBase64.Split('.')[3]
                };
            }
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
                Issuer = $"https://api.authress.io/v1/clients/{System.Web.HttpUtility.UrlEncode(accessKey.ClientId)}",
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
    }
}
