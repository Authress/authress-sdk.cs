using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Authress.SDK.Client
{
    internal interface IHttpClientProvider
    {
        HttpClient Create(HttpMessageHandler messageHandler);
    }

    /// <summary>
    /// Authress Domain Host: https://DOMAIN.api.authress.io
    /// </summary>
    public class HttpClientSettings
    {
        /// <summary>
        /// Authress Domain Host: https://DOMAIN.api.authress.io
        /// </summary>
        public string ApiBasePath { get; set; } = "https://api.authress.io";

        /// <summary>
        /// Timeout for requests to Authress. Default is unset.
        /// </summary>
        public TimeSpan? RequestTimeout { get; set; } = null;
    }

    internal class HttpClientProvider
    {
        private readonly SemaphoreSlim syncObj = new SemaphoreSlim(1);
        private HttpClient clientProxy;
        private HttpClientSettings settings;

        public HttpClientProvider(HttpClientSettings settings)
        {
            this.settings = settings;
        }


        public async Task<HttpClient> GetHttpClientAsync()
        {
            if (clientProxy != null)
            {
                return clientProxy;
            }

            try
            {
                // ensure only one thread is creating the client
                await syncObj.WaitAsync();

                // in case another thread has created a client in the meanwhile
                if (clientProxy != null)
                {
                    return clientProxy;
                }

                // create the inner handlers for the cache handler, the outermost handler is called first. (https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/http-message-handlers)
                HttpMessageHandler outermostHandler = new HttpClientHandler { AllowAutoRedirect = true };
                outermostHandler = new AddAuthorizationHeaderHandler(outermostHandler);

                // create the client and assign it to the member variable for future access, create a tmp client so that the client is fully initialized before setting "client" property.
                clientProxy = new HttpClient(outermostHandler);
                if (settings.RequestTimeout != null)
                {
                    clientProxy.Timeout = settings.RequestTimeout.Value;
                }

                clientProxy.BaseAddress = new Uri(settings.ApiBasePath);
                return clientProxy;
            }
            finally
            {
                syncObj.Release();
            }
        }
    }

    internal class AddAuthorizationHeaderHandler : DelegatingHandler
    {
        public AddAuthorizationHeaderHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.TryAddWithoutValidation("Authorization", "");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
