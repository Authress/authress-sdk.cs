using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using Authress.SDK;
using Authress.SDK.Client;

namespace Authress.SDK.UnitTests
{
    public class AuthressClientTokenProviderTests
    {
        private const string userId = "unit-test-user-id";

        public class TestCase
        {
            public string AccessKey { get; set; }
        }

        public static TestData<TestCase> TestCases = new TestData<TestCase>
        {
            {
                "EdDSA",
                new TestCase{ AccessKey = "KEY" }
            }
        };

        [Theory, MemberData(nameof(TestCases))]
        public async Task GetBearerToken(string testName, TestCase testCase)
        {
            var tokenProvider = new AuthressClientTokenProvider(testCase.AccessKey);
            var token = await tokenProvider.GetBearerToken();
            System.Console.WriteLine(token);

            tokenProvider.Should().NotBeNull(testName, testCase);

            var authressSettings = new AuthressSettings { AuthressApiUrl = "DOMAIN", };
            var authressClient = new AuthressClient(tokenProvider, authressSettings);
            var result = await authressClient.GetUserResources("user_001", "resources/*/sub", "edit", DTO.CollectionConfigurationEnum.INCLUDE_NESTED);
            await authressClient.GetUserResources("user_001", "resources/*/sub", "edit", DTO.CollectionConfigurationEnum.INCLUDE_NESTED);
            await authressClient.GetUserResources("user_001", "resources/*/sub", "edit", DTO.CollectionConfigurationEnum.INCLUDE_NESTED);
            await authressClient.GetUserResources("user_001", "resources/*/sub", "edit", DTO.CollectionConfigurationEnum.INCLUDE_NESTED);
            await authressClient.GetUserResources("user_001", "resources/*/sub", "edit", DTO.CollectionConfigurationEnum.INCLUDE_NESTED);
            await authressClient.GetUserResources("user_001", "resources/*/sub", "edit", DTO.CollectionConfigurationEnum.INCLUDE_NESTED);
            System.Console.WriteLine(string.Join(", ", result.Resources.Select(s => s.ResourceUri)));

            var token2 = await tokenProvider.GetBearerToken();
            System.Console.WriteLine(token2);
        }
    }
}
