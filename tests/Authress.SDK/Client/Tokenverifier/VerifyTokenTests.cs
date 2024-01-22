using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authress.SDK.Client;
using Authress.SDK.DTO;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Xunit;

namespace Authress.SDK.UnitTests.TokenVerifier
{
    internal class UnitTestIntentionalException : Exception {}

    public class VerifyTokenTests
    {

        private static string authressCustomDomain = "https://unit-test-customdomain.authress.io";
        // Prefix MCowBQYDK2VwAyEA is inferred by the configuration of the JWK
        private static (string, string) eddsaKeys = ("MC4CAQAwBQYDK2VwBCIEIHWOlqpfN1YdPSAvAZlSxOyZs0v0jnOj3flvG4ezJ8/R", "P1ghjuexanmp5hYgSYRvbFJirquynaCyolH3vHb9JCE=");

        [Fact]
        public async Task ValidateEddsaToken() {
            var testUserId = Guid.NewGuid().ToString();
            var testKeyId = Guid.NewGuid().ToString();
            var authressClientTokenProvider = new AuthressClientTokenProvider($"{testUserId}.{testKeyId}.account.{eddsaKeys.Item1}", authressCustomDomain);
            // setup
            var edDsaJwkResponse = new JwkResponse { Keys = new List<Jwk> { new Jwk { Alg = Alg.EdDSA, kid = testKeyId, x = eddsaKeys.Item2 } } };
            var jwtToken = await authressClientTokenProvider.GetBearerToken();

            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);
            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = edDsaJwkResponse.ToHttpContent() });

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);

            var mockHttpClientProvider = new HttpClientProvider(null, null, mockFactory.Object);
            var tokenVerifier = new SDK.TokenVerifier(authressCustomDomain, mockHttpClientProvider);

            var result = await tokenVerifier.VerifyToken(jwtToken);
            result.Should().BeEquivalentTo(new VerifiedUserIdentity { UserId = testUserId });

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once());
            mockHttpClient.VerifyAll();
        }

        private static (string, string) rsa512Keys = ("-----BEGIN PRIVATE KEY-----MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQDClKx0rcN5hgtLrCrUtpeErBu8wAQIU0Rc3mnb5Kcg4wn6EGvPhyhCTVVpDtBdFyKXmg3hwhI57yON2jeikHsr3pFm6AbgTs4lHlwFJxEIrQYOvD5wxGeiy5boOTCIWl27OtohJOYiRqHvoVzltcOFgESTvTXID0tIDJMnyADfrUyXD2tavHoqaifhGN9eT9ObsnGo7OaIiSspvgm/Sz8zVBumv8DMDM9FMlOPqeXFFhy1ADEQ4LQh6ZuJ7hs78aVBjq7vQmlWYsWu1yIlC4Qufhhnt5/lAsVTHp2YmEqZHWpqrBaus1kXmhG/YZMvzRLfLkv1vi26z/MJoemA3E5tAgMBAAECgf9i5Tk1/QO/0ZWDZlUwAeOAs7Mfe9V5lEz2Q6BnKIQO/puwmigC70v1fh58ZBDrnH9PsQ5RhykNyPegYU581yxOvMqSt0+Bjxe3LqWKylYeAUcYtiupGUEe1Al8INrVidKCokm7VJcummYzsJt1JPKd+SThrCwMcP9X9oCOmm/XPq+o5HjzovxN9MxJLhsZH6oBqcMD/nBf0aLPYPMRRM/6CxvWHQdMYGHhUdfUtRHR977ZwPJEOTe0p8Z+zNM/yZ3oSaLyu8928kpeFWQWkNEOj71DLpFkOtPRMeXIxlaL7wIEAas2oeBTmdkpJuRQG9sjsBLJVaBnoCzBcXqY6vECgYEA/4njaT2SfBs4URqVFyBPTc9a/gCipOegzLFP1+T+Ng5qbmlGxpTmqdqFpPa/N3BmzrLs/PHJcDc5hAD8B6ChsSPozRLtathFr7YT/SJi2GLBikRj/+UwsO5pWNLUkKUqA80G1G2rRaj1PoXhOm9sB4E9bm/pghFUYCwUrFEqhHECgYEAwu6cNZNnBO1enyoBsT9LWnr8MeLP9Seqr2vWGKY5awliEDW/LebvryEfEumTMDjMPa+1nBdsWMJ4O+fdGaLIGet8TKBD8Lt+00N2ZRvJAmS6DLfZkhUOawzi51tQYuJhHdQJavXOzAMOjDytAQr23m5D+ffb/wi5Qm5VdrNod70CgYEA20LMT1vWmgidHPIrJQnDIieekr2m0MoyrhAiS0QgX++koRJR+UiAVxO6gp552i7m98qNEEjCqDeqcTqLBlxtANqoAXaRIpFp0efwZM4hdDvghyxBhat5SQd4ew+D9ozRbSt6BcIIKKBdtgUYCZTbY+Ef/eemu8T02gRYxLZsPfECgYAZHNpdAJdmCBqHkMbVCd1wU6XH23uFDs4reU4EsO12v6e1hOcTR8wbGL5DFnpS3Q5a6BcSe+YGbU9GEHHoipMS28aQrJj0G4OUPf2zuuejekyJtOm/qxzHZ8qXmaj6hEWUrStlbzNsDvzBzlNPbhUtxLDXNDpQWdRcHZC/EQ/lVQKBgQD8aQs5tYZ0Zt6zLAw8J43M4vGsTTEL9YziSRfVdRYMr1yAPMoVQxujjY4+hYm9//BK2Kfq2yxTYpN+ShoJJWY4GGrJt5ZGsbIgJpdBP7UzMbZXg7CvwsKtPnKk7iuNPUxF7B856zOwHwbsPYNAJB3snerTLIy2/00oOXIVGYAJEw==-----END PRIVATE KEY-----", "-----BEGIN PUBLIC KEY-----MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwpSsdK3DeYYLS6wq1LaXhKwbvMAECFNEXN5p2+SnIOMJ+hBrz4coQk1VaQ7QXRcil5oN4cISOe8jjdo3opB7K96RZugG4E7OJR5cBScRCK0GDrw+cMRnosuW6DkwiFpduzraISTmIkah76Fc5bXDhYBEk701yA9LSAyTJ8gA361Mlw9rWrx6Kmon4RjfXk/Tm7JxqOzmiIkrKb4Jv0s/M1Qbpr/AzAzPRTJTj6nlxRYctQAxEOC0Iembie4bO/GlQY6u70JpVmLFrtciJQuELn4YZ7ef5QLFUx6dmJhKmR1qaqwWrrNZF5oRv2GTL80S3y5L9b4tus/zCaHpgNxObQIDAQAB-----END PUBLIC KEY-----");

        [Fact]
        public async Task ValidateRsaToken() {
            var testUserId = Guid.NewGuid().ToString();
            var testKeyId = Guid.NewGuid().ToString();


            var accessKeyAsString = System.Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(JsonConvert.SerializeObject(new AccessKey {
                Algorithm = "RS256",
                PrivateKey = rsa512Keys.Item1,
                ClientId = testUserId,
                KeyId = testKeyId,
                Audience = "account"
            })));
            var authressClientTokenProvider = new AuthressClientTokenProvider(accessKeyAsString, authressCustomDomain);
            // setup

            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(DotNetUtilities.ToRSAParameters((RsaKeyParameters)new PemReader(new StringReader(rsa512Keys.Item2)).ReadObject()));
            var rsaParameters = csp.ExportParameters(false);

            var rsaJwkResponse = new JwkResponse { Keys = new List<Jwk> { new Jwk {
                Alg = Alg.RS256, kid = testKeyId, n = Convert.ToBase64String(rsaParameters.Modulus), e = Convert.ToBase64String(rsaParameters.Exponent)
            } } };
            var jwtToken = await authressClientTokenProvider.GetBearerToken();

            var mockHttpClient = new Mock<HttpClientHandler>(MockBehavior.Strict);
            mockHttpClient.Protected().SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = rsaJwkResponse.ToHttpContent() });

            var mockFactory = new Mock<IHttpClientHandlerFactory>(MockBehavior.Strict);
            mockFactory.Setup(factory => factory.Create()).Returns(mockHttpClient.Object);

            var mockHttpClientProvider = new HttpClientProvider(null, null, mockFactory.Object);
            var tokenVerifier = new SDK.TokenVerifier(authressCustomDomain, mockHttpClientProvider);

            var result = await tokenVerifier.VerifyToken(jwtToken);
            result.Should().BeEquivalentTo(new VerifiedUserIdentity { UserId = testUserId });

            mockFactory.Verify(mockFactory => mockFactory.Create(), Times.Once());
            mockHttpClient.VerifyAll();
        }
    }
}
