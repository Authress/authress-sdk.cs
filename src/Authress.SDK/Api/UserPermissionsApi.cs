using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Authress.SDK;
using Authress.SDK.Api;
using Authress.SDK.Client;
using Authress.SDK.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace Authress.SDK
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class AuthressClient : IUserPermissionsApi
    {
        // There is a two level cache in place because the OptimisticPerformanceHandler might have evicted the value from the cache already, and here we can keep ones either for long and also more specific to this endpoint. The difference is we only cache for 5 minutes
        private static readonly IMemoryCache authorizationCache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 5000 });

        private static void SetCacheKey ((string, string, string) key, bool value) {
            authorizationCache.Set(key, value, new MemoryCacheEntryOptions {
                SlidingExpiration = TimeSpan.FromMinutes(5),
                Size = 1
            });
        }

        /// <summary>
        /// Get the permissions a user has to a resource. Get a summary of the permissions a user has to a particular resource.
        /// </summary>
        /// <param name="userId">The user to check permissions on</param>
        /// <param name="resourceUri">The uri path of a resource to validate, uri segments are allowed.</param>
        /// <returns>UserPermissions</returns>
        public async Task<UserPermissions> GetUserAuthorizationForResource (string userId, string resourceUri)
        {
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ArgumentNullException("Missing required parameter 'userId'.");
            // verify the required parameter 'resourceUri' is set
            if (resourceUri == null) throw new ArgumentNullException("Missing required parameter 'resourceUri'.");

            var path = $"/v1/users/{System.Web.HttpUtility.UrlEncode(userId)}/resources/{System.Web.HttpUtility.UrlEncode(resourceUri)}/permissions";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<UserPermissions>();
            }
        }

        /// <summary>
        /// Check to see if a user has permissions to a resource. Does the user have the specified permissions to the resource?
        /// </summary>
        /// <param name="userId">The user to check permissions on</param>
        /// <param name="resourceUri">The uri path of a resource to validate, uri segments are allowed.</param>
        /// <param name="permission">Permission to check, &#x27;*&#x27; and scoped permissions can also be checked here.</param>
        /// <returns>AuthorizationResponse</returns>
        public async Task AuthorizeUser (string userId, string resourceUri, string permission)
        {
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ArgumentNullException("Missing required parameter 'userId'.");
            // verify the required parameter 'resourceUri' is set
            if (resourceUri == null) throw new ArgumentNullException("Missing required parameter 'resourceUri'.");
            // verify the required parameter 'permission' is set
            if (permission == null) throw new ArgumentNullException("Missing required parameter 'permission'.");

            var path = $"/v1/users/{System.Web.HttpUtility.UrlEncode(userId)}/resources/{System.Web.HttpUtility.UrlEncode(resourceUri)}/permissions/{System.Web.HttpUtility.UrlEncode(permission)}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            HttpStatusCode responseStatusCode;
            string formattedMsg = null;
            try {
                using (var response = await client.GetAsync(path))
                {
                    responseStatusCode = response.StatusCode;

                    // 200s are handled here
                    // 300s have automated redirect handling so they are also dynamically handled
                    if ((int)responseStatusCode >= 200 && (int)responseStatusCode <= 299) {
                        SetCacheKey((userId, resourceUri, permission), true);
                        return;
                    }

                    // 404 missing permission
                    if (responseStatusCode == HttpStatusCode.NotFound)
                    {
                        SetCacheKey((userId, resourceUri, permission), false);
                        throw new NotAuthorizedException(userId, resourceUri, permission);
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    formattedMsg = $"Status code was {responseStatusCode} when calling '{response.RequestMessage?.RequestUri?.ToString()}', message was '{content}'";
                }
            }
            catch
            {
                if (authorizationCache.TryGetValue((userId, resourceUri, permission), out bool cachedValueOnConnectionError))
                {
                    if (cachedValueOnConnectionError)
                    {
                        return;
                    }

                    throw new NotAuthorizedException(userId, resourceUri, permission);
                }

                // Throw on connection issues where there wasn't anything in the cache
                // * At this point we already retried as much as possible there is nothing else we can do other than throw
                throw;
            }

            // All other 400s are handled here
            if (responseStatusCode == HttpStatusCode.Unauthorized) { throw new InvalidTokenException(); }
            if (responseStatusCode == HttpStatusCode.PaymentRequired) { throw new PaymentRequiredException(); }
            if ((int)responseStatusCode == 429) { throw new TooManyRequestsException(); }

            if ((int)responseStatusCode >= 400 && (int)responseStatusCode <= 499)
            {
                throw new NotSuccessHttpResponseException(responseStatusCode, formattedMsg);
            }

            // Handle 500s
            if (authorizationCache.TryGetValue((userId, resourceUri, permission), out bool cachedValue))
            {
                if (cachedValue)
                {
                    return;
                }

                throw new NotAuthorizedException(userId, resourceUri, permission);
            }

            throw new NotSuccessHttpResponseException(responseStatusCode, formattedMsg);
        }

        /// <summary>
        /// Get the users resources. Get the users resources. This result is a list of resource uris that a user has an explicit permission to, a user with * access to all sub resources will return an empty list and {AccessToAllSubResources} will be populated. The list will be paginated.
        /// </summary>
        /// <param name="userId">The user to check permissions on</param>
        /// <param name="resourceCollectionUri">The uri path of a collection resource to fetch permissions for.</param>
        /// <param name="permission">A required ALLOW action to check for. Resources a user does not have this permission will not be returned.</param>
        /// <param name="collectionConfiguration">Configure the collection result to contain only the top level matches or additionally nested resources.</param>
        /// <returns>UserResources</returns>
        /// <returns>AuthorizationResponse</returns>
        public async Task<UserResources> GetUserResources(string userId, string resourceCollectionUri, string permission,
            CollectionConfigurationEnum collectionConfiguration = CollectionConfigurationEnum.TOP_LEVEL_ONLY)
        {
            // verify the required parameter 'userId' is set
            if (userId == null)
            {
                throw new ArgumentNullException("Missing required parameter 'userId'.");
            }

            if (collectionConfiguration == CollectionConfigurationEnum.INCLUDE_NESTED)
            {
                var queryParams = new Dictionary<string, string>
                {
                    { "resourceUri", resourceCollectionUri },
                    { "permissions", permission },
                    { "collectionConfiguration", collectionConfiguration.ToString() }
                };

                var queryString = queryParams.Where(pair => !string.IsNullOrEmpty(pair.Value))
                    .Select(pair => $"{pair.Key}={System.Web.HttpUtility.UrlEncode(pair.Value)}").Aggregate((next, total) => $"{total}&{next}");
                var path = $"/v1/users/{System.Web.HttpUtility.UrlEncode(userId)}/resources?{queryString}";

                var client = await authressHttpClientProvider.GetHttpClientAsync();
                using (var response = await client.GetAsync(path))
                {
                    await response.ThrowIfNotSuccessStatusCode();
                    return await response.Content.ReadAsAsync<UserResources>();
                }
            }
            else
            {

                var queryParams = new Dictionary<string, string>
                {
                    { "resourceUri", resourceCollectionUri },
                    { "permissions", permission }
                };

                var queryString = queryParams.Where(pair => !string.IsNullOrEmpty(pair.Value))
                    .Select(pair => $"{pair.Key}={System.Web.HttpUtility.UrlEncode(pair.Value)}").Aggregate((next, total) => $"{total}&{next}");
                var path = $"/v1/users/{System.Web.HttpUtility.UrlEncode(userId)}/resources?{queryString}";

                var client = await authressHttpClientProvider.GetHttpClientAsync();

                var authorizeUserAsync = AuthorizeUser(userId, resourceCollectionUri, permission);
                using (var response = await client.GetAsync(path))
                {
                    try
                    {
                        await authorizeUserAsync;
                        return new UserResources { UserId = userId, AccessToAllSubResources = true, Resources = null };
                    }
                    catch (Exception)
                    {
                        /* Ignore if the user doesn't have permission or if there is a problem, instead fallback to looking up explicit resources by permission */
                    }
                    await response.ThrowIfNotSuccessStatusCode();
                    return await response.Content.ReadAsAsync<UserResources>();
                }
            }
        }
    }
}
