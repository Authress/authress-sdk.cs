using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Authress.SDK.Client;
using FluentAssertions;
using Xunit;

namespace Authress.SDK.UnitTests
{
    public class RewriteBaseUrlHandlerTests
    {
        [Fact]
        public async Task AppendsRequestPath() =>
            (await GetRewrittenUrl("https://example.com", "/request-path"))
            .Should().Be("https://example.com/request-path");

        [Theory]
        [InlineData("/relative-path")]
        [InlineData("relative-path")]
        public async Task PreservesBaseUrlPath(string relativePath) =>
            (await GetRewrittenUrl("https://example.com/path/", relativePath))
            .Should().Be("https://example.com/path/relative-path");

        [Fact]
        public async Task PassesQuery() =>
            (await GetRewrittenUrl("https://example.com", "/path?paramName=paramValue"))
            .Should().Be("https://example.com/path?paramName=paramValue");

        [Fact]
        public async Task PassesFragment() =>
            (await GetRewrittenUrl("https://example.com", "/path#fragment"))
            .Should().Be("https://example.com/path#fragment");

        private static async Task<string> GetRewrittenUrl(string baseUrl, string relativePath)
        {
            var innerHandler = new TestHandler();
            var handler = new RewriteBaseUrlHandler(innerHandler, baseUrl);
            var client = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };

            await client.GetAsync(relativePath);
            return innerHandler.RequestUri;
        }

        private class TestHandler : DelegatingHandler
        {
            public string RequestUri { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                RequestUri = request.RequestUri.ToString();
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
        }
    }
}
