using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using Authress.SDK;
using Authress.SDK.Client;
using System.Net.Http;
using System.Threading;
using Moq.Protected;
using System;

namespace Authress.SDK.UnitTests
{
    public class AuthorizeUserTestsIntentionalException : Exception {}

    public class AuthorizeUserTests
    {
        private const string userId = "unit-test-user-id";

        public class TestCase
        {
            public string UserId { get; set; }
            public string ResourceId { get; set; }
            public string Permission { get; set; }
        }

        public static TestData<TestCase> TestCases => new TestData<TestCase>
        {
            {
                "Expect cached value to be returned",
                new TestCase{ UserId = "TestUserId", ResourceId = "TestResourceId", Permission = "TestPermission" }
            }
        };

        [Theory, MemberData(nameof(TestCases))]
        public async Task AuthorizeUserCaching(string testName, TestCase testCase)
        {
            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);
            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK })
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var client = new AuthressClient(null, new AuthressSettings{ AdditionalRetries = 0 }, mockFactory.Object);
            await client.AuthorizeUser(testCase.UserId, testCase.ResourceId, testCase.Permission);
            await client.AuthorizeUser(testCase.UserId, testCase.ResourceId, testCase.Permission);

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once(), testName);
            mockHttpClient.VerifyAll();
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task AuthorizeUserFallbackToCacheOn5XX(string testName, TestCase testCase)
        {
            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);

            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK })
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError });

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var client = new AuthressClient(null, new AuthressSettings{ AdditionalRetries = 0 }, mockFactory.Object);
            await client.AuthorizeUser(testCase.UserId, testCase.ResourceId, testCase.Permission);
            await client.AuthorizeUser(testCase.UserId, testCase.ResourceId, testCase.Permission);

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once(), testName);
            mockHttpClient.VerifyAll();
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task AuthorizeUserFallbackToCacheOnConnectionError(string testName, TestCase testCase)
        {
            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);

            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK })
            .ThrowsAsync(new AuthorizeUserTestsIntentionalException());

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var client = new AuthressClient(null, new AuthressSettings{ AdditionalRetries = 0 }, mockFactory.Object);
            await client.AuthorizeUser(testCase.UserId, testCase.ResourceId, testCase.Permission);
            await client.AuthorizeUser(testCase.UserId, testCase.ResourceId, testCase.Permission);

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once(), testName);
            mockHttpClient.VerifyAll();
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task AuthorizeUserFallbackToCachedUnauthorizedOnConnectionError(string testName, TestCase testCase)
        {
            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);

            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound })
            .ThrowsAsync(new AuthorizeUserTestsIntentionalException());

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var client = new AuthressClient(null, new AuthressSettings{ AdditionalRetries = 0 }, mockFactory.Object);
            try {
                await client.AuthorizeUser(testCase.UserId, testCase.ResourceId, testCase.Permission);
            } catch (Exception actualException) {
                actualException.Should().BeOfType<NotAuthorizedException>();
            }
            try {
                await client.AuthorizeUser(testCase.UserId, testCase.ResourceId, testCase.Permission);
            } catch (Exception actualException) {
                actualException.Should().BeOfType<NotAuthorizedException>();
            }

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once(), testName);
            mockHttpClient.VerifyAll();
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task ThrowsErrorOnNoCachedResultAndMissingPermission(string testName, TestCase testCase)
        {
            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);

            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound })
            .ThrowsAsync(new AuthorizeUserTestsIntentionalException());

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var client = new AuthressClient(null, new AuthressSettings{ AdditionalRetries = 0 }, mockFactory.Object);
            try {
                await client.AuthorizeUser(testCase.UserId, testCase.ResourceId, $"{new Random().Next()}");
            } catch (Exception actualException) {
                actualException.Should().BeOfType<NotAuthorizedException>();
            }

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once(), testName);
            mockHttpClient.VerifyAll();
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task CacheEvictionsHappenAtMaxSize(string testName, TestCase testCase)
        {
            var iterationsLength = 10000;
            var expectedCacheSize = 2000;

            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);
            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var client = new AuthressClient(null, new AuthressSettings{ AdditionalRetries = 0 }, mockFactory.Object);

            var callSequence = mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            for (var iteration = 0; iteration < iterationsLength; iteration++) {
                callSequence.ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK });
            }

            // Then force the next 30 to all fail forcing retrieving values from the cache
            for (var iteration = 0; iteration < iterationsLength; iteration++) {
                callSequence.ThrowsAsync(new AuthorizeUserTestsIntentionalException());
            }

            // Put them all in the cache (only the last 2000 should be allowed to be kept in the cache)
            for (var iteration = 0; iteration < iterationsLength; iteration++) {
                await client.AuthorizeUser(testCase.UserId, testCase.ResourceId, $"{iteration}");
            }

            // Because caching isn't necessary thread safe we can't really validate that these are in the cache, but we can validate that next once are definitely not
            // for (var expectedValueInTheCache = iterationsLength - 1; expectedValueInTheCache >= iterationsLength - expectedCacheSize; expectedValueInTheCache--) {
            //     await client.AuthorizeUser(testCase.UserId, testCase.ResourceId, $"{expectedValueInTheCache}");
            // }

            for (var iteration = 0; iteration < iterationsLength - expectedCacheSize; iteration++) {
                var expectedValueToNotBeInTheCache = iteration;
                try {
                    await client.AuthorizeUser(testCase.UserId, testCase.ResourceId, $"{expectedValueToNotBeInTheCache}");
                } catch (Exception actualException) {
                    actualException.Should().BeOfType<AuthorizeUserTestsIntentionalException>();
                }
            }

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once(), testName);
            mockHttpClient.VerifyAll();
        }
    }
}
