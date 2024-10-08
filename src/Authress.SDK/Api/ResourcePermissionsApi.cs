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
    internal partial class AuthressClient : IResourcePermissionsApi
    {
        /// <summary>
        /// List resource configurations Permissions can be set globally at a resource level. Lists any resources with a globally set resource policy
        /// </summary>
        /// <returns>ResourcePermissionCollection</returns>
        public async Task<ResourcePermissionCollection> GetResources ()
        {

            var path = "/v1/resources";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<ResourcePermissionCollection>();
            }
        }

        /// <summary>
        /// Get a resource permissions object. Permissions can be set globally at a resource level. This will apply to all users in an account.
        /// </summary>
        /// <param name="resourceUri">The uri path of a resource to validate, uri segments are allowed.</param>
        /// <returns>ResourcePermission</returns>
        public async Task<ResourcePermission> GetResourcePermissions (string resourceUri)
        {
            // verify the required parameter 'resourceUri' is set
            if (resourceUri == null) throw new ArgumentNullException("Missing required parameter 'resourceUri'.");

            var path = $"/v1/resources/{System.Web.HttpUtility.UrlEncode(resourceUri)}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<ResourcePermission>();
            }
        }

        /// <summary>
        /// Update a resource permissions object. Updates the global permissions on a resource. This applies to all users in an account.
        /// </summary>
        /// <param name="body">The contents of the permission to set on the resource. Overwrites existing data.</param>
        /// <param name="resourceUri">The uri path of a resource to validate, uri segments are allowed.</param>
        /// <returns>Object</returns>
        public async Task SetResourcePermissions (string resourceUri, ResourcePermission body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ArgumentNullException("Missing required parameter 'body'.");
            // verify the required parameter 'resourceUri' is set
            if (resourceUri == null) throw new ArgumentNullException("Missing required parameter 'resourceUri'.");

            var path = $"/v1/resources/{System.Web.HttpUtility.UrlEncode(resourceUri)}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.PutAsync(path, body.ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
            }
        }

    }
}
