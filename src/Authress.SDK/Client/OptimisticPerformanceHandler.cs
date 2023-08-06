using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Authress.SDK.Client
{
    internal class JWT {
        internal string Sub { get; set; }
    }

    internal class OptimisticPerformanceHandler : DelegatingHandler
    {
        private readonly TimeSpan cacheFallbackNormTimeout;

        private readonly TimeSpan cacheDuration = TimeSpan.FromHours(1);
        private readonly IMemoryCache responseCache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 2000 });

        public OptimisticPerformanceHandler(HttpMessageHandler innerHandler, TimeSpan cacheFallbackTimeout) : base(innerHandler) {
            this.cacheFallbackNormTimeout = cacheFallbackTimeout;
        }

        /// <summary>
        /// Tries to get the value from the cache, and only calls the delegating handler on cache misses.
        /// </summary>
        /// <returns>The HttpResponseMessage from cache, or a newly invoked one.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // only handle Safe and idempotent calls the rest of them should skip using this handler
            if (request.Method != HttpMethod.Get && request.Method != HttpMethod.Head && request.Method != HttpMethod.Options && request.Method.Method != "Query")
            {
                return await base.SendAsync(request, cancellationToken);
            }

            // When a userId is not present in the request uri then that means we are returning results based on the user in the authorization.
            // * That means either the authorization token userId has to be part of the requestUri OR the userId from the authorization token has to be.
            // * We assume here that the later rarely happens, so caching those results doesn't offer additional benefits
            var requestUriString = request.RequestUri.ToString();
            var key = request.Method + "-" + requestUriString;
            if (!Regex.Match(requestUriString, "/users/[^/]+", RegexOptions.IgnoreCase).Success) {
                var token = request.Headers.Authorization?.Parameter ?? request.Headers.Authorization?.Scheme ?? string.Empty;
                if (token.Split('.').Length < 3) {
                    return await base.SendAsync(request, cancellationToken);
                }
                var jwtPayload =  token.Split('.')[1];

                JWT jwt = JsonConvert.DeserializeObject<JWT>(Base64UrlEncoder.Decode(jwtPayload));
                var jwtSubject = jwt.Sub;
                if (string.IsNullOrEmpty(jwtSubject)) {
                    return await base.SendAsync(request, cancellationToken);
                }
                key = $"{jwtSubject}-${key}";
            }

            var cachedResponse = GetCachedResponse(request, key);

            TimeSpan modifiedTimeoutCalculator()
            {
                var maxWaitTime = 10000;
                if (cachedResponse == null) {
                    return TimeSpan.FromMilliseconds(maxWaitTime);
                }

                var difference = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - cachedResponse.CachedAtTimestamp;
                // Otherwise use a scaled value for how long is reasonable to wait. The longer the data is stale the longer we should wait before accepting it
                /* Wait times based on cache age
                { cache age seconds, timeout milliseconds}
                    { 0, 0}
                    { 1, 10}
                    { 10, 30}
                    { 30, 100}
                    { 60, 200}
                    { 5 * 60, 1000}
                    { 10 * 60, 2000}
                */
                var expectedTimeoutMultiplier = difference <= 0 ? 0 : (int)Math.Floor(2 * difference * Math.Log(difference, 10));
                var actualTimeoutMultiplier = expectedTimeoutMultiplier >= maxWaitTime ? maxWaitTime : (expectedTimeoutMultiplier <= 0 ? 0 : expectedTimeoutMultiplier);
                return actualTimeoutMultiplier / 100 * cacheFallbackNormTimeout;
            }

            var httpSendTask = base.SendAsync(request, cancellationToken);
            var timeoutTask = Task.Delay(modifiedTimeoutCalculator(), cancellationToken);

            var firstCompletedTask = await Task.WhenAny(httpSendTask, timeoutTask);

            // Timeout: The http send task did not complete first. Wait for the cache to respond and return that data
            // * The cache does not respond, has no data, or times out then fallback to the http request
            if (firstCompletedTask == timeoutTask)
            {
                if (cachedResponse != null)
                {
                    // update the cache after the http task eventually completes, without awaiting it
                    // Do not await saving the value to the cache because we timed out
                    // Instead we'll attempt to wait for the response in the background and then save it to the cache if successful
                    _ = httpSendTask.ContinueWith(async t => {
                        if (200 <= (int)t.Result.StatusCode && (int)t.Result.StatusCode < 500) { await SaveResponseToCache(t.Result, key); }
                    }, TaskContinuationOptions.OnlyOnRanToCompletion);

                    return cachedResponse.HttpResponseMessage;
                }
            }

            // Either the HTTP task finished quickly or there is no data from the cache.
            // * Save the result to the cache if successful and return it.
            HttpResponseMessage response = null;
            Exception requestException = null;
            try {
                response = await httpSendTask;

                // when successful, return that value, we don't store unsuccessful responses in the cache
                if (200 <= (int)response.StatusCode && (int)response.StatusCode < 500)
                {
                    // This consumes the response, so we need to use the returned entry to respond with
                    // * If we failed to store the value to the cache then we are stuck, so fallback to the previous version in the cache if available.
                    var entry = await SaveResponseToCache(response, key);
                    if (entry != null) {
                        return request.ConvertCachedEntryToHttpResponse(entry).HttpResponseMessage;
                    }
                }
            }
            catch (Exception ex) {
                /* It doesn't matter what the exception was we can't use it, either try the cache or throw */
                requestException = ex;
            }

            // when unsuccessful, try to get it from the cache, which means the last successful invocation
            if (cachedResponse != null)
            {
                return cachedResponse.HttpResponseMessage;
            }

            if (requestException != null) {
                throw requestException;
            }

            return response;
        }

        private HttpResponseMessageWithCacheData GetCachedResponse(HttpRequestMessage request, string key)
        {
            // get from cache
            if (responseCache.TryGetValue(key, out byte[] binaryData))
            {
                try
                {
                    var chars = new char[binaryData.Length / sizeof(char)];
                    Buffer.BlockCopy(binaryData, 0, chars, 0, binaryData.Length);
                    string json = new(chars);
                    var data = JsonConvert.DeserializeObject<HttpCacheableData>(json);
                    if (data != null)
                    {
                        // get the data from the cache
                        var cachedResponse = request.ConvertCachedEntryToHttpResponse(data);
                        return cachedResponse;
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        private async Task<HttpCacheableData> SaveResponseToCache(HttpResponseMessage response, string key)
        {
            if (TimeSpan.Zero != cacheDuration)
            {
                var entry = await response.ConvertHttpResponseToCacheEntry();
                try
                {
                    var json = JsonConvert.SerializeObject(entry);
                    var bytes = new byte[json.Length * sizeof(char)];
                    Buffer.BlockCopy(json.ToCharArray(), 0, bytes, 0, bytes.Length);
                    responseCache.Set(key, bytes, new MemoryCacheEntryOptions {Size = 1, SlidingExpiration = cacheDuration});
                }
                catch { /* ignore all exceptions */ }

                return entry;
            }

            return null;
        }
    }
}

internal class HttpResponseMessageWithCacheData {
    public HttpResponseMessage HttpResponseMessage { get; }
    public long CachedAtTimestamp { get; }

    public HttpResponseMessageWithCacheData(long cachedAtDateTime, HttpResponseMessage httpResponseMessage) {
        HttpResponseMessage = httpResponseMessage;
        CachedAtTimestamp = cachedAtDateTime;
    }
}
/// <summary>
/// The data object that is used to put into the cache.
/// </summary>
internal class HttpCacheableData
{
    /// <summary>
    /// Construct an object to cache
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cacheableResponse"></param>
    /// <param name="headers"></param>
    /// <param name="contentHeaders"></param>
    public HttpCacheableData(byte[] data, HttpResponseMessage cacheableResponse, Dictionary<string, IEnumerable<string>> headers, Dictionary<string, IEnumerable<string>> contentHeaders)
    {
        Data = data;
        CacheableResponse = cacheableResponse;
        Headers = headers;
        ContentHeaders = contentHeaders;
        CachedAtDateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public long CachedAtDateTime { get; }

    /// <summary>
    /// The cacheable part of a previously retrieved response (excludes the content and request).
    /// </summary>
    public HttpResponseMessage CacheableResponse { get; }

    /// <summary>
    /// The content of the response.
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// the headers of the response.
    /// </summary>
    public Dictionary<string, IEnumerable<string>> Headers { get; }

    /// <summary>
    /// The content headers of the response.
    /// </summary>
    public Dictionary<string, IEnumerable<string>> ContentHeaders { get; }
}

/// <summary>
/// Extension methods of the HttpResponseMessage that are related to the caching functionality.
/// </summary>
internal static class HttpResponseMessageExtensions
{
    public static async Task<HttpCacheableData> ConvertHttpResponseToCacheEntry(this HttpResponseMessage response)
    {
        var ignoredHeaders = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) {
            "x-amzn-requestid", "access-control-allow-origin", "strict-transport-security", "x-region", "x-amz-apigw-id", "access-control-expose-headers", "x-amzn-trace-id", "access-control-allow-credentials", "vary", "x-cache", "via", "x-amz-cf-pop", "x-amz-cf-id"
        };

        var data = await response.Content.ReadAsByteArrayAsync();
        var copy = new HttpResponseMessage{ ReasonPhrase = response.ReasonPhrase, StatusCode = response.StatusCode, Version = response.Version };
        var headers = response.Headers.Where(h => h.Key != null && !ignoredHeaders.Contains(h.Key) && h.Value != null && h.Value.Any())
            .ToDictionary(h => h.Key, h => h.Value);
        var contentHeaders = response.Content.Headers.Where(h => h.Value != null && h.Value.Any()).ToDictionary(h => h.Key, h => h.Value);
        var entry = new HttpCacheableData(data, copy, headers, contentHeaders);
        return entry;
    }

    /// <summary>
    /// Prepares the cached entry to be consumed by the caller, notably by setting the content.
    /// </summary>
    /// <param name="request">The request that invoked retrieving this response and need to be attached to the response.</param>
    /// <param name="cachedData">The deserialized data from the cache.</param>
    /// <returns>A valid HttpResponseMessage that can be consumed by the caller of this message handler.</returns>
    public static HttpResponseMessageWithCacheData ConvertCachedEntryToHttpResponse(this HttpRequestMessage request, HttpCacheableData cachedData)
    {
        var response = cachedData.CacheableResponse;
        if (cachedData.Headers != null)
        {
            foreach (var kvp in cachedData.Headers)
            {
                response.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value);
            }
        }

        response.Content = new ByteArrayContent(cachedData.Data);
        if (cachedData.ContentHeaders != null)
        {
            foreach (var kvp in cachedData.ContentHeaders)
            {
                response.Content.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value);
            }
        }
        response.RequestMessage = request;
        return new HttpResponseMessageWithCacheData(cachedData.CachedAtDateTime, response);
    }
}
