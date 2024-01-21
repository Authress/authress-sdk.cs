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

namespace Authress.SDK
{
    public partial class AuthressClient
    {
        private readonly HttpClientProvider authressHttpClientProvider;
        private readonly TokenVerifier tokenVerifier;

        /// <summary>
        /// Get the permissions a user has to a resource. Get a summary of the permissions a user has to a particular resource.
        /// </summary>
        public AuthressClient(ITokenProvider tokenProvider, AuthressSettings settings, IHttpClientHandlerFactory customHttpClientHandlerFactory = null)
        {
            if (settings == null) {
                throw new ArgumentNullException("Missing required parameter AuthressSettings");
            }
            authressHttpClientProvider = new HttpClientProvider(settings, tokenProvider, customHttpClientHandlerFactory);
            tokenVerifier = new TokenVerifier(settings.ApiBasePath, authressHttpClientProvider);
        }

        /// <summary>
        /// Deprecated Constructor
        /// </summary>
        public AuthressClient(ITokenProvider tokenProvider, HttpClientSettings settings, IHttpClientHandlerFactory customHttpClientHandlerFactory = null)
        {
            if (settings == null) {
                throw new ArgumentNullException("Missing required parameter HttpClientSettings");
            }
            authressHttpClientProvider = new HttpClientProvider(
                new AuthressSettings { ApiBasePath = settings?.ApiBasePath, RequestTimeout = settings?.RequestTimeout },
                tokenProvider, customHttpClientHandlerFactory);
            tokenVerifier = new TokenVerifier(settings.ApiBasePath, authressHttpClientProvider);
        }

        /// <summary>
        /// Verify a JWT token from anywhere. If it is valid a VerifiedUserIdentity will be returned. If it is invalid an exception will be thrown.
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns>A verified user identity that contains the user's ID</returns>
        /// <exception cref="TokenVerificationException">Token is invalid in some way</exception>
        /// <exception cref="ArgumentNullException">One of the required parameters for this function was not specified.</exception>
        public async Task<VerifiedUserIdentity> VerifyToken(string jwtToken) {
            return await tokenVerifier.VerifyToken(jwtToken);
        }

    }
}
