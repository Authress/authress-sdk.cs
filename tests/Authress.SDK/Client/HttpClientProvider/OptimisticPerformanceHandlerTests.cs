using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Authress.SDK.Client;
using Authress.SDK.DTO;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Xunit;

namespace Authress.SDK.UnitTests
{
    internal class UnitTestIntentionalException : Exception {}

    public class OptimisticPerformanceHandlerTests
    {
        [Fact]
        public async Task GetCachedUserResourcesOnSubsequentFailure()
        {
            // setup

            var userId = Guid.NewGuid().ToString();
            var resourceId = Guid.NewGuid().ToString();
            var permission = Guid.NewGuid().ToString();

            var expectedResult = new UserResources { UserId = Guid.NewGuid().ToString() };

            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);
            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = expectedResult.ToHttpContent() })
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = expectedResult.ToHttpContent() })
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError })
            .ThrowsAsync(new UnitTestIntentionalException());

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var client = new AuthressClient(null, new AuthressSettings{ AdditionalRetries = 0 }, mockFactory.Object);

            var responseWhenSuccess = await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            var responseWhenSuccess2 = await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            var responseWhenError = await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            var responseWhenOtherError = await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);

            responseWhenSuccess.Should().BeEquivalentTo(expectedResult);
            responseWhenSuccess2.Should().BeEquivalentTo(expectedResult);
            responseWhenError.Should().BeEquivalentTo(responseWhenSuccess);
            responseWhenOtherError.Should().BeEquivalentTo(responseWhenSuccess);

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once());
            mockHttpClient.VerifyAll();
        }

        [Fact]
        public async Task SkipCacheWhenUsingABadToken()
        {
            // setup

            var groupId = Guid.NewGuid().ToString();

            var expectedResult = new Group { GroupId = groupId };

            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);
            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = expectedResult.ToHttpContent() })
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = expectedResult.ToHttpContent() })
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError })
            .ThrowsAsync(new UnitTestIntentionalException());

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var manualTokenProvider = new ManualTokenProvider();
            manualTokenProvider.SetToken("Bad Token");
            var client = new AuthressClient(manualTokenProvider, new AuthressSettings{ AdditionalRetries = 0 }, mockFactory.Object);

            var responseWhenSuccess = await client.GetGroup(groupId);
            var responseWhenSuccess2 = await client.GetGroup(groupId);

            responseWhenSuccess.Should().BeEquivalentTo(expectedResult);
            responseWhenSuccess2.Should().BeEquivalentTo(expectedResult);

            try {
                await client.GetGroup(groupId);
            } catch (Exception e) {
                e.Should().BeOfType<NotSuccessHttpResponseException>();
            }try {
                await client.GetGroup(groupId);
            } catch (Exception e) {
                e.Should().BeOfType<UnitTestIntentionalException>();
            }

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once());
            mockHttpClient.VerifyAll();
        }

        [Fact]
        public async Task Skip_the_cache_when_no_token_available()
        {
            // setup

            var groupId = Guid.NewGuid().ToString();

            var expectedResult = new Group { GroupId = groupId };

            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);
            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = expectedResult.ToHttpContent() })
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError })
            .ThrowsAsync(new UnitTestIntentionalException());

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var client = new AuthressClient(null, new AuthressSettings{ AdditionalRetries = 0 }, mockFactory.Object);

            var responseWhenSuccess = await client.GetGroup(groupId);

            try {
                await client.GetGroup(groupId);
            } catch (Exception e) {
                e.Should().BeOfType<NotSuccessHttpResponseException>();
            }try {
                await client.GetGroup(groupId);
            } catch (Exception e) {
                e.Should().BeOfType<UnitTestIntentionalException>();
            }


            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once());
            mockHttpClient.VerifyAll();
        }

        [Fact]
        public async Task Use_the_cache_tied_to_the_correct_user()
        {
            // setup

            var groupId = Guid.NewGuid().ToString();

            var expectedResult = new Group { GroupId = groupId };

            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);
            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = expectedResult.ToHttpContent() })
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError })
            .ThrowsAsync(new UnitTestIntentionalException());

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var manualTokenProvider = new ManualTokenProvider();
            manualTokenProvider.SetToken("HEADER.eyJzdWIiOiIxMjM0NTY3ODkwMCIsImlhdCI6MTUxNjIzOTAyMn0.SIGNATURE");
            var client = new AuthressClient(manualTokenProvider, new AuthressSettings{ AdditionalRetries = 0 }, mockFactory.Object);

            var responseWhenSuccess = await client.GetGroup(groupId);

            manualTokenProvider.SetToken("HEADER.eyJzdWIiOiJvdGhlci11c2VyIiwiaWF0IjoxNTE2MjM5MDIyfQ.SIGNATURE");

            try {
                await client.GetGroup(groupId);
            } catch (Exception e) {
                e.Should().BeOfType<NotSuccessHttpResponseException>();
            }try {
                await client.GetGroup(groupId);
            } catch (Exception e) {
                e.Should().BeOfType<UnitTestIntentionalException>();
            }


            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once());
            mockHttpClient.VerifyAll();
        }

        [Fact(DisplayName = "Authress.SDK.UnitTests.OptimisticPerformanceHandlerTests.TimeoutDuringGet: Timeout during GET. If this test times out, it means the cache is broken, because the timeouts are skipped because the cache comes into play.", Timeout = 1000)]
        public async Task TimeoutDuringGet()
        {

            var userId = Guid.NewGuid().ToString();
            var resourceId = Guid.NewGuid().ToString();
            var permission = Guid.NewGuid().ToString();

            var expectedResult1 = new UserResources { UserId = "User 1" };
            var expectedResult2 = new UserResources { UserId = "User 2" };
            var expectedResult3 = new UserResources { UserId = "User 3" };
            var expectedResult4 = new UserResources { UserId = "User 4" };

            var taskSource1 = new TaskCompletionSource<bool>();
            var manualTaskToComplete1 = taskSource1.Task;

            var taskSource2 = new TaskCompletionSource<bool>();
            var manualTaskToComplete2 = taskSource2.Task;

            var taskSource3 = new TaskCompletionSource<bool>();
            var manualTaskToComplete3 = taskSource3.Task;

            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);
            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = expectedResult1.ToHttpContent() })
            .Returns(async () => {
                await manualTaskToComplete1;
                return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = expectedResult2.ToHttpContent() };
            })
            .Returns(async () => {
                await manualTaskToComplete2;
                return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = expectedResult3.ToHttpContent() };
            })
            .Returns(async () => {
                await manualTaskToComplete3;
                return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = expectedResult4.ToHttpContent() };
            })
            .Returns(async () => {
                await new TaskCompletionSource<bool>().Task;
                return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = expectedResult4.ToHttpContent() };
            });

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var client = new AuthressClient(null, new AuthressSettings{ AdditionalRetries = 0, CacheFallbackTimeout = TimeSpan.FromMilliseconds(1) }, mockFactory.Object);

            var responseWhenSuccess = await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            var responseWhenSuccess2 = await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            var responseWhenTimeout = await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            var responseWhenTimeout2 = await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);

            responseWhenSuccess.Should().BeEquivalentTo(expectedResult1);
            responseWhenSuccess2.Should().BeEquivalentTo(expectedResult1);
            responseWhenTimeout.Should().BeEquivalentTo(expectedResult1);
            responseWhenTimeout2.Should().BeEquivalentTo(expectedResult1);

            // Check after timeouts are done.
            taskSource1.SetResult(true);
            // Since this is in the background we have delay this thread until the background update is completed
            await Task.Delay(10);
            taskSource2.SetResult(true);
            // Since this is in the background we have delay this thread until the background update is completed
            await Task.Delay(10);
            taskSource3.SetResult(true);
            // Since this is in the background we have delay this thread until the background update is completed
            await Task.Delay(10);

            var responseWhenSuccessAfterTimeout = await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            responseWhenSuccessAfterTimeout.Should().BeEquivalentTo(expectedResult4);

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once());
            mockHttpClient.VerifyAll();
        }

        [Fact]
        public async Task NoCacheOnlyRequestExceptions()
        {
            // setup

            var userId = Guid.NewGuid().ToString();
            var resourceId = Guid.NewGuid().ToString();
            var permission = Guid.NewGuid().ToString();

            var expectedResult = new UserResources { UserId = Guid.NewGuid().ToString() };

            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);
            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new UnitTestIntentionalException())
            .ThrowsAsync(new UnitTestIntentionalException());

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var client = new AuthressClient(null, new AuthressSettings{ AdditionalRetries = 0 }, mockFactory.Object);

            try {
                await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            } catch (Exception e) {
                e.Should().BeOfType<UnitTestIntentionalException>();
            }try {
                await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            } catch (Exception e) {
                e.Should().BeOfType<UnitTestIntentionalException>();
            }

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once());
            mockHttpClient.VerifyAll();
        }

        [Fact]
        public async Task NoCacheOnly500s()
        {
            // setup

            var userId = Guid.NewGuid().ToString();
            var resourceId = Guid.NewGuid().ToString();
            var permission = Guid.NewGuid().ToString();

            var expectedResult = new UserResources { UserId = Guid.NewGuid().ToString() };

            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);
            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError })
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError });

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var client = new AuthressClient(null, new AuthressSettings{ AdditionalRetries = 0 }, mockFactory.Object);

            try {
                await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            } catch (Exception e) {
                e.Should().BeOfType<NotSuccessHttpResponseException>();
            }try {
                await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            } catch (Exception e) {
                e.Should().BeOfType<NotSuccessHttpResponseException>();
            }

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once());
            mockHttpClient.VerifyAll();
        }

        [Fact]
        public async Task GetCachedError()
        {

            var userId = Guid.NewGuid().ToString();
            var resourceId = Guid.NewGuid().ToString();
            var permission = Guid.NewGuid().ToString();

            var expectedResult = new UserResources { UserId = Guid.NewGuid().ToString() };

            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);
            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = expectedResult.ToHttpContent() })
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = expectedResult.ToHttpContent() })
            .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError });

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);
            var client = new AuthressClient(null, new AuthressSettings{ AdditionalRetries = 0 }, mockFactory.Object);

            try {
                var responseWhenSuccess = await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            } catch (Exception e) {
                e.Should().BeOfType<NotSuccessHttpResponseException>();
            }
            try {
                var responseWhenSuccess2 = await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            } catch (Exception e) {
                e.Should().BeOfType<NotSuccessHttpResponseException>();
            }
            try {
                var responseWhenError = await client.GetUserResources(userId, resourceId, permission, CollectionConfigurationEnum.INCLUDE_NESTED);
            } catch (Exception e) {
                e.Should().BeOfType<NotSuccessHttpResponseException>();
            }

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once());
            mockHttpClient.VerifyAll();
        }

        [Fact]
        public async Task CacheOnlyStandardHeaders() {
            var responseData = new ResourcePermission { Permissions = new List<ResourcePermissionPermissions>{
                new ResourcePermissionPermissions {Action = DTO.PermissionedResource.ActionEnum.CLAIM, Allow = true }
            }};

            var httpResponseMessage = new HttpResponseMessage {
                Content = responseData.ToHttpContent(),
                StatusCode = HttpStatusCode.Continue
            };
            httpResponseMessage.Headers.Add("Header", new string[] {"Value", "Value"});
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("Type/Type");
            var expectedResult = new HttpCacheableData(
                await responseData.ToHttpContent().ReadAsByteArrayAsync(),
                new HttpResponseMessage{ ReasonPhrase = httpResponseMessage.ReasonPhrase, StatusCode = httpResponseMessage.StatusCode, Version = httpResponseMessage.Version },
                new Dictionary<string, IEnumerable<string>>{{ "Header", new string[]{"Value", "Value" }}},
                new Dictionary<string, IEnumerable<string>>{{ "Content-Type", new string[]{"Type/Type"}}, { "Content-Length", new string[1] {"49"} }}
            );
            var cacheableResult = await httpResponseMessage.ConvertHttpResponseToCacheEntry();
            cacheableResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
