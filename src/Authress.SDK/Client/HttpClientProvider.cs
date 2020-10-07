using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Authress.SDK.Client
{
    /// <summary>
    /// Customize the base handler used when accessing Authress through the HttpClientProvider. Everytime a new client is generated (which usually only happens once), the base handler factory will be called.
    /// </summary>
    public interface IHttpClientHandlerFactory
    {
        /// <summary>
        /// Creates the base client handler when a new HttpClient is needed.
        /// </summary>
        HttpClientHandler Create();
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
        private readonly HttpClientSettings settings;
        private readonly ITokenProvider tokenProvider;
        private readonly IHttpClientHandlerFactory customHttpClientHandlerFactory;

        public HttpClientProvider(HttpClientSettings settings, ITokenProvider tokenProvider, IHttpClientHandlerFactory customHttpClientHandlerFactory = null)
        {
            this.settings = settings;
            this.tokenProvider = tokenProvider;
            this.customHttpClientHandlerFactory = customHttpClientHandlerFactory;
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
                HttpMessageHandler outermostHandler = customHttpClientHandlerFactory?.Create() ?? new HttpClientHandler { AllowAutoRedirect = true };
                outermostHandler = new RewriteBaseUrlHandler(outermostHandler, settings.ApiBasePath);
                outermostHandler = new AddAuthorizationHeaderHandler(outermostHandler, tokenProvider);
                outermostHandler = new AddUserAgentHeaderHandler(outermostHandler);

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
        private ITokenProvider tokenProvider;
        public AddAuthorizationHeaderHandler(HttpMessageHandler innerHandler, ITokenProvider tokenProvider) : base(innerHandler)
        {
            this.tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var bearerToken = await tokenProvider.GetBearerToken();
            var token = bearerToken.ToLower().Contains("bearer") ? bearerToken : $"Bearer {bearerToken}";
            request.Headers.TryAddWithoutValidation("Authorization", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }

    internal class AddUserAgentHeaderHandler : DelegatingHandler
    {
        public AddUserAgentHeaderHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var version = typeof(HttpClientProvider).Assembly.GetName().Version;
            request.Headers.TryAddWithoutValidation("User-Agent", $"C# AuthressSDK version: {version}");
            return await base.SendAsync(request, cancellationToken);
        }
    }

    internal class RewriteBaseUrlHandler : DelegatingHandler
    {
        private readonly Uri baseUrl;

        public RewriteBaseUrlHandler(HttpMessageHandler innerHandler, string baseUrl) : base(innerHandler)
        {
            this.baseUrl = new Uri(baseUrl.EndsWith("/") ? baseUrl : baseUrl + "/");
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var builder = new UriBuilder(baseUrl.Scheme, baseUrl.Host, baseUrl.Port)
            {
                Path = MergePath(baseUrl.AbsolutePath, request.RequestUri.AbsolutePath),
                Query = request.RequestUri.Query,
                Fragment = request.RequestUri.Fragment
            };

            request.RequestUri = builder.Uri;
            return await base.SendAsync(request, cancellationToken);
        }

        private static string MergePath(string baseUrlPath, string requestPath) =>
            requestPath.StartsWith(baseUrlPath) ? requestPath : baseUrlPath + requestPath.Substring(1);
    }
}
