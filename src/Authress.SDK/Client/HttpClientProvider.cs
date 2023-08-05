using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Authress.SDK.Client
{
    /// <summary>
    /// Customize the base handler used when accessing Authress through the HttpClientProvider. Every time a new client is generated (which usually only happens once), the base handler factory will be called.
    /// </summary>
    public interface IHttpClientHandlerFactory
    {
        /// <summary>
        /// Creates the base client handler when a new HttpClient is needed.
        /// </summary>
        HttpClientHandler Create();
    }

    /// <summary>
    /// Authress Domain Host: https://CUSTOM_DOMAIN.application.com (Get an authress custom domain: https://authress.io/app/#/settings?focus=domain)
    /// </summary>
    public class HttpClientSettings
    {
        /// <summary>
        /// Authress Domain Host: https://CUSTOM_DOMAIN.application.com (Get an authress custom domain: https://authress.io/app/#/settings?focus=domain)
        /// </summary>
        public string ApiBasePath { get; set; } = "https://api.authress.io";

        /// <summary>
        /// Timeout for requests to Authress. Default is unset.
        /// </summary>
        public TimeSpan? RequestTimeout { get; set; } = null;
    }

    /// <summary>
    /// Authress Domain Host: https://CUSTOM_DOMAIN.application.com (Get an authress custom domain: https://authress.io/app/#/settings?focus=domain)
    /// </summary>
    public class AuthressSettings
    {
        /// <summary>
        /// Authress Domain Host: https://CUSTOM_DOMAIN.application.com (Get an authress custom domain: https://authress.io/app/#/settings?focus=domain)
        /// </summary>
        public string ApiBasePath { get; set; } = "https://api.authress.io";

        /// <summary>
        /// Timeout for requests to Authress. Default is unset.
        /// </summary>
        public TimeSpan? RequestTimeout { get; set; } = null;

        /// <summary>
        /// In the even that the request fails with a retryable error, such as a 503 status code, how many additional attempts to should the SDK make to complete request.
        /// </summary>
        public uint AdditionalRetries { get; set; } = 5;

        /// <summary>
        /// Set the max wait time for requests sent to Authress. If Authress does not respond in this time, immediately go to cache to resolve the result. If the cache is not populated this value of this setting is not taken into account and the request will wait for the full duration. The default is 30 milliseconds.
        /// </summary>
        public TimeSpan CacheFallbackTimeout { get; set; } = TimeSpan.FromMilliseconds(30);
    }

    internal class HttpClientProvider
    {
        private readonly SemaphoreSlim syncObj = new SemaphoreSlim(1);
        private HttpClient clientProxy;
        private readonly AuthressSettings settings;
        private readonly ITokenProvider tokenProvider;
        private readonly IHttpClientHandlerFactory customHttpClientHandlerFactory;

        public HttpClientProvider(AuthressSettings settings, ITokenProvider tokenProvider, IHttpClientHandlerFactory customHttpClientHandlerFactory = null)
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
                // * The retry handler is listed first, then everything else will be executed once, and the retries will run in an inner loop
                // * If the retry handler is listed last, then the whole stack is retried going all the way in and all the way back out to the retry handler.
                // * If you want something to be recalculated on every retried call put it before the retry handler, if a handler is invariant across multiple calls, put it after the retry handler.
                /**** ⌄ Called Last ⌄ ******/
                HttpMessageHandler outermostHandler = customHttpClientHandlerFactory?.Create() ?? new HttpClientHandler { AllowAutoRedirect = true };
                // List of Handlers that need to be recalculated on every call

                // -
                // -

                /********************/
                // The retry handler
                outermostHandler = new RetryHandler(outermostHandler, settings.AdditionalRetries);
                /********************/

                // List of Handlers that never need to be retried
                outermostHandler = new OptimisticPerformanceHandler(outermostHandler, settings.CacheFallbackTimeout);
                outermostHandler = new RewriteBaseUrlHandler(outermostHandler, settings.ApiBasePath);
                outermostHandler = new AddAuthorizationHeaderHandler(outermostHandler, tokenProvider, settings.ApiBasePath);
                outermostHandler = new AddUserAgentHeaderHandler(outermostHandler);
                /**** ⌃ Called First ⌃ ******/

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
        private readonly ITokenProvider tokenProvider;
        private readonly string apiBasePath;

        public AddAuthorizationHeaderHandler(HttpMessageHandler innerHandler, ITokenProvider tokenProvider, string apiBasePath) : base(innerHandler)
        {
            this.tokenProvider = tokenProvider;
            this.apiBasePath = apiBasePath;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (tokenProvider != null) {
                var bearerToken = await tokenProvider.GetBearerToken(apiBasePath);
                var token = bearerToken.ToLower().Contains("bearer") ? bearerToken : $"Bearer {bearerToken}";
                request.Headers.TryAddWithoutValidation("Authorization", token);
            }
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

    internal class RetryHandler : DelegatingHandler
    {
        private readonly uint additionalRetries = 5;
        private const int retryDelayMilliseconds = 20;

        public RetryHandler(HttpMessageHandler innerHandler, uint additionalRetries = 5) : base(innerHandler) {
            this.additionalRetries = additionalRetries;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            Exception lastException = null;
            var totalRequests = 1 + additionalRetries;
            for (int i = 0; i < totalRequests; i++)
            {
                try
                {
                    response = await base.SendAsync(request, cancellationToken);

                    if ((int) response.StatusCode <= 499)
                    {
                        return response;
                    }
                }
                catch (Exception requestException)
                {
                    lastException = requestException;
                }

                if (i == additionalRetries)
                {
                    break;
                }

                await Task.Delay((int) (retryDelayMilliseconds * Math.Pow(2, i)), cancellationToken);
            }

            if (response != null)
            {
                return response;
            }

            throw lastException;
        }
    }
}
