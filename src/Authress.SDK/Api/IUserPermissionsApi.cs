using System;
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
        /// Get the permissions a user has to resources that match the collection resource. Will return "*" or other top level resources which imply cascading access to child resources. Will not return sub resources of the resources in a collection.
        /// </summary>
        /// <param name="userId">The user to check permissions on</param>
        /// <param name="resourceCollectionUri">The uri path of a collection resource to fetch permissions for.</param>
        /// <returns>UserResources</returns>
        Task<UserResources> GetUserResources (string userId, string resourceCollectionUri);

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
