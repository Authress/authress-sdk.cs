# authress-sdk.cs
The Authress SDK for C#

[![NuGet version](https://badge.fury.io/nu/Authress.Sdk.svg)](https://badge.fury.io/nu/Authress.Sdk) [![Build Status](https://travis-ci.com/authress/authress-sdk.cs.svg?branch=master)](https://travis-ci.com/authress/authress-sdk.cs)


### Usage

#### Package Management
* [Authress.SDK Nuget Package](https://www.nuget.org/packages/Authress.SDK)

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
            var authressSettings = new HttpClientSettings { ApiBasePath = "https://DOMAIN.api.authress.io", };
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
            var authressSettings = new HttpClientSettings { ApiBasePath = "https://DOMAIN.api.authress.io", };
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
            var authressSettings = new HttpClientSettings { ApiBasePath = "https://DOMAIN.api.authress.io", };
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
