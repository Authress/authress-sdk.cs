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

        /// <summary>
        /// Get the permissions a user has to a resource. Get a summary of the permissions a user has to a particular resource.
        /// </summary>
        public AuthressClient(ITokenProvider tokenProvider, HttpClientSettings settings, IHttpClientHandlerFactory customHttpClientHandlerFactory = null)
        {
            authressHttpClientProvider = new HttpClientProvider(settings, tokenProvider, customHttpClientHandlerFactory);
        }
    }

    internal class AccessKey
    {
        public String Audience { get; set; }
        public String ClientId { get; set; }
        public String KeyId { get; set; }
        public String PrivateKey { get; set; }
    }
}
