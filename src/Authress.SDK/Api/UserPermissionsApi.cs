using System;
using System.Collections.Generic;
using System.Net.Http;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Authress.SDK.Api
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
        /// <returns>InlineResponse200</returns>
        public InlineResponse200 V1UsersUserIdResourcesResourceUriPermissionsGet (string userId, string resourceUri)
        {
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling V1UsersUserIdResourcesResourceUriPermissionsGet");
            // verify the required parameter 'resourceUri' is set
            if (resourceUri == null) throw new ApiException(400, "Missing required parameter 'resourceUri' when calling V1UsersUserIdResourcesResourceUriPermissionsGet");

            var path = "/v1/users/{userId}/resources/{resourceUri}/permissions";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userId" + "}", ApiClient.ParameterTostring(userId));
path = path.Replace("{" + "resourceUri" + "}", ApiClient.ParameterTostring(resourceUri));

            var queryParams = new Dictionary<string, string>();
            var headerParams = new Dictionary<string, string>();
            var formParams = new Dictionary<string, string>();
            var fileParams = new Dictionary<string, FileParameter>();
            string postBody = null;


            // authentication setting, if any
            string[] authSettings = new string[] { "oauth2" };

            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling V1UsersUserIdResourcesResourceUriPermissionsGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1UsersUserIdResourcesResourceUriPermissionsGet: " + response.ErrorMessage, response.ErrorMessage);

            return (InlineResponse200) ApiClient.Deserialize(response.Content, typeof(InlineResponse200), response.Headers);
        }

        /// <summary>
        /// Check to see if a user has permissions to a resource. Does the user have the specified permissions to the resource?
        /// </summary>
        /// <param name="userId">The user to check permissions on</param>
        /// <param name="resourceUri">The uri path of a resource to validate, must be URL encoded, uri segments are allowed, the resource must be a full path, and permissions are not inhereted by sub resources.</param>
        /// <param name="permission">Permission to check, &#x27;*&#x27; and scoped permissions can also be checked here.</param>
        /// <returns>Object</returns>
        public Object V1UsersUserIdResourcesResourceUriPermissionsPermissionGet (string userId, string resourceUri, string permission)
        {
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling V1UsersUserIdResourcesResourceUriPermissionsPermissionGet");
            // verify the required parameter 'resourceUri' is set
            if (resourceUri == null) throw new ApiException(400, "Missing required parameter 'resourceUri' when calling V1UsersUserIdResourcesResourceUriPermissionsPermissionGet");
            // verify the required parameter 'permission' is set
            if (permission == null) throw new ApiException(400, "Missing required parameter 'permission' when calling V1UsersUserIdResourcesResourceUriPermissionsPermissionGet");

            var path = "/v1/users/{userId}/resources/{resourceUri}/permissions/{permission}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userId" + "}", ApiClient.ParameterTostring(userId));
path = path.Replace("{" + "resourceUri" + "}", ApiClient.ParameterTostring(resourceUri));
path = path.Replace("{" + "permission" + "}", ApiClient.ParameterTostring(permission));

            var queryParams = new Dictionary<string, string>();
            var headerParams = new Dictionary<string, string>();
            var formParams = new Dictionary<string, string>();
            var fileParams = new Dictionary<string, FileParameter>();
            string postBody = null;


            // authentication setting, if any
            string[] authSettings = new string[] { "oauth2" };

            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling V1UsersUserIdResourcesResourceUriPermissionsPermissionGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1UsersUserIdResourcesResourceUriPermissionsPermissionGet: " + response.ErrorMessage, response.ErrorMessage);

            return (Object) ApiClient.Deserialize(response.Content, typeof(Object), response.Headers);
        }

    }
}
