using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authress.SDK.DTO;

namespace Authress.SDK.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IUserPermissionsApi
    {
        /// <summary>
        /// Get the users resources. This result is a list of resource uris that a user has an permission to. By default only the top level matching resources are returned. To get a user's list of deeply nested resources, set the collectionConfiguration to be INCLUDE_NESTED. This collection is paginated.
        /// </summary>
        /// <param name="userId">The user to check permissions on</param>
        /// <param name="resourceCollectionUri">The uri path of a collection resource to fetch permissions for.</param>
        /// <param name="permission">A required ALLOW action to check for. Resources a user does not have this permission will not be returned.</param>
        /// <param name="collectionConfiguration">Configure the collection result to contain only the top level matches or additionally nested resources.</param>
        /// <returns>UserResources</returns>
        Task<UserResources> GetUserResources (string userId, string resourceCollectionUri, string permission,
            CollectionConfigurationEnum collectionConfiguration = CollectionConfigurationEnum.TOP_LEVEL_ONLY);

        /// <summary>
        /// Get the permissions a user has to a resource. Get a summary of the permissions a user has to a particular resource.
        /// </summary>
        /// <param name="userId">The user to check permissions on</param>
        /// <param name="resourceUri">The uri path of a resource to validate, uri segments are allowed.</param>
        /// <returns>UserPermissions</returns>
        Task<UserPermissions> GetUserAuthorizationForResource (string userId, string resourceUri);

        /// <summary>
        /// Check to see if a user has permissions to a resource. Does the user have the specified permissions to the resource?
        /// </summary>
        /// <param name="userId">The user to check permissions on</param>
        /// <param name="resourceUri">The uri path of a resource to validate, uri segments are allowed.</param>
        /// <param name="permission">Permission to check, &#x27;*&#x27; and scoped permissions can also be checked here.</param>
        /// <returns>Object</returns>
        Task AuthorizeUser (string userId, string resourceUri, string permission);
    }
}
