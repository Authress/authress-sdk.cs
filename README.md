
<p align="center">
    <img src="https://github.com/Authress/authress-sdk.cs/assets/5056218/924fb776-9588-4d4a-adf7-33682fa29356" height="300px" alt="Authress Media Banner">
</p>

# Authress SDK for C#

This is the Authress SDK for C#. Authress provides an authorization API for user identity, access control, and api key management as a drop in SaaS.

The Nuget package connects to the [Authress API](https://authress.io/app/#/api). You can use Authress to build authentication and authorization directly into your applications and services. Additionally, Authress can be used locally to develop faster without needing an [Authress Account](https://authress.io)

<p align="center">
    <a href="https://www.nuget.org/packages/Authress.SDK" alt="Authress Nuget C#"><img src="https://badge.fury.io/nu/Authress.Sdk.svg"></a>
    <a href="./LICENSE" alt="Apache-2.0"><img src="https://img.shields.io/badge/License-Apache%202.0-blue.svg"></a>
    <a href="https://authress.io/community" alt="authress community"><img src="https://img.shields.io/badge/Community-Authress-fbaf0b.svg"></a>
</p>

<hr>

## Usage
You can either directly install the Authress SDK directly into your current application or checkout the [Authress C# Starter Kit](https://github.com/Authress/csharp-starter-kit#authress-starter-kit-c--net-asp-mvc).

Installation:

* run: `dotnet add Authress.SDK` (or install via visual tools)

#### Authorize users using user identity token
```csharp
using Authress.SDK;

namespace Microservice
{
    public class Controller
    {
        public static async void Route()
        {
            // automatically populate forward the users token
            // 1. instantiate all the necessary classes (example using ASP.NET or MVC, but any function works)
            //   If using the HttpContextAccessor, register it first inside the application root
            //   services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var tokenProvider = new DynamicTokenProvider(() =>
            {
                // Then get the access token from the incoming API request and return it
                var httpContextAccessor = ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("Bearer", "access_token");
                return accessToken;
            });
            // Get an authress custom domain: https://authress.io/app/#/settings?focus=domain
            var authressSettings = new AuthressSettings { ApiBasePath = "https://CUSTOM_DOMAIN.application.com", };
            var authressClient = new AuthressClient(tokenProvider, authressSettings);

            // 2. At runtime attempt to Authorize the user for the resource
            await authressClient.AuthorizeUser("USERID", "RESOURCE_URI", "PERMISSION");

            // API Route code
            // ...
        }
    }
}
```

#### Authorize using an explicitly set token each time
```csharp
using Authress.SDK;

namespace Microservice
{
    public class Controller
    {
        public static async void Route()
        {
            // automatically populate forward the users token
            // 1. instantiate all the necessary classes
            var tokenProvider = new ManualTokenProvider();
            var authressSettings = new AuthressSettings { ApiBasePath = "https://DOMAIN.api.authress.io", };
            var authressClient = new AuthressClient(tokenProvider, authressSettings);

            // 2. At runtime attempt to Authorize the user for the resource
            tokenProvider.setToken(userJwt);
            await authressClient.AuthorizeUser("USERID", "RESOURCE_URI", "PERMISSION");

            // API Route code
            // ...
        }
    }
}
```

#### Authorize users using client secret
```csharp
using Authress.SDK;

namespace Microservice
{
    public class Controller
    {
        public static async void Route()
        {
            // accessKey is returned from service client creation in Authress UI
            // 1. instantiate all the necessary classes
            var accessKey = 'ACCESS_KEY';
            // Assuming it was encrypted in storage, decrypt it
            var decodedAccessKey = decrypt(accessKey);
            var tokenProvider = new AuthressClientTokenProvider(decodedAccessKey);
            // Get an authress custom domain: https://authress.io/app/#/settings?focus=domain
            var authressSettings = new AuthressSettings { ApiBasePath = "https://CUSTOM_DOMAIN.application.com", };
            var authressClient = new AuthressClient(tokenProvider, authressSettings);

            // Attempt to Authorize the user for the resource
            // 2. At runtime the token provider will automatically pull the token forward
            await authressClient.AuthorizeUser("USERID", "RESOURCE_URI", "PERMISSION");

            // API Route code
            // ...
        }
    }
}
```

### Contribution guidelines for the Authress SDK
[Contribution guidelines](./contributing.md)
