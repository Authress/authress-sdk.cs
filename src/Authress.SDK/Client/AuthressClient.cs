using System.Net.Http;
using System.Net.Http.Headers;
using Authress.SDK.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Authress.SDK
{
    public partial class AuthressClient
    {
        private readonly string accessKey;

        private readonly HttpClientProvider authressHttpClientProvider;

        /// <summary>
        /// Get the permissions a user has to a resource. Get a summary of the permissions a user has to a particular resource.
        /// </summary>
        public AuthressClient(string accessKey, HttpClientSettings settings)
        {
            this.accessKey = accessKey;
            authressHttpClientProvider = new HttpClientProvider(settings);
        }

        internal static HttpContent ToHttpContent(object data)
        {
            var standardSerializerSettings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var content = JsonConvert.SerializeObject(data, standardSerializerSettings);
            var httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpContent;
        }

    }
}
