using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Authress.SDK;
using Authress.SDK.Api;
using Authress.SDK.Client;
using Authress.SDK.DTO;

namespace Authress.SDK
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class AuthressClient : IUserPermissionsApi
    {
        /// <summary>
        /// Get the permissions a user has to a resource. Get a summary of the permissions a user has to a particular resource.
        /// </summary>
        /// <param name="userId">The user to check permissions on</param>
        /// <param name="resourceUri">The uri path of a resource to validate, must be URL encoded, uri segments are allowed.</param>
        /// <returns>UserPermissions</returns>
        public async Task<UserPermissions> GetUserAuthorizationForResource (string userId, string resourceUri)
        {
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling V1UsersUserIdResourcesResourceUriPermissionsGet");
            // verify the required parameter 'resourceUri' is set
            if (resourceUri == null) throw new ApiException(400, "Missing required parameter 'resourceUri' when calling V1UsersUserIdResourcesResourceUriPermissionsGet");

            var path = $"/v1/users/{userId}/resources/{resourceUri}/permissions";
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
        /// <param name="resourceUri">The uri path of a resource to validate, must be URL encoded, uri segments are allowed, the resource must be a full path, and permissions are not inhereted by sub resources.</param>
        /// <param name="permission">Permission to check, &#x27;*&#x27; and scoped permissions can also be checked here.</param>
        /// <returns>AuthorizationResponse</returns>
        public async Task<AuthorizationResponse> AuthorizeUser (string userId, string resourceUri, string permission)
        {
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling V1UsersUserIdResourcesResourceUriPermissionsPermissionGet");
            // verify the required parameter 'resourceUri' is set
            if (resourceUri == null) throw new ApiException(400, "Missing required parameter 'resourceUri' when calling V1UsersUserIdResourcesResourceUriPermissionsPermissionGet");
            // verify the required parameter 'permission' is set
            if (permission == null) throw new ApiException(400, "Missing required parameter 'permission' when calling V1UsersUserIdResourcesResourceUriPermissionsPermissionGet");

            var path = $"/v1/users/{userId}/resources/{resourceUri}/permissions/{permission}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<AuthorizationResponse>();
            }
        }
    }
}
