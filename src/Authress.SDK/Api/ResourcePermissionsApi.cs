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
    public partial class AuthressClient : IResourcePermissionsApi
    {
        /// <summary>
        /// List resource configurations Permissions can be set globally at a resource level. Lists any resources with a globally set resource policy
        /// </summary>
        /// <returns>ResourcePermissionCollection</returns>
        public ResourcePermissionCollection V1ResourcesGet ()
        {

            var path = "/v1/resources";
            path = path.Replace("{format}", "json");

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
                throw new ApiException ((int)response.StatusCode, "Error calling V1ResourcesGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ResourcesGet: " + response.ErrorMessage, response.ErrorMessage);

            return (ResourcePermissionCollection) ApiClient.Deserialize(response.Content, typeof(ResourcePermissionCollection), response.Headers);
        }

        /// <summary>
        /// Get a resource permissions object. Permissions can be set globally at a resource level. This will apply to all users in an account.
        /// </summary>
        /// <param name="resourceUri">The uri path of a resource to validate, must be URL encoded, uri segments are allowed.</param>
        /// <returns>ResourcePermission</returns>
        public ResourcePermission V1ResourcesResourceUriGet (string resourceUri)
        {
            // verify the required parameter 'resourceUri' is set
            if (resourceUri == null) throw new ApiException(400, "Missing required parameter 'resourceUri' when calling V1ResourcesResourceUriGet");

            var path = "/v1/resources/{resourceUri}";
            path = path.Replace("{format}", "json");
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
                throw new ApiException ((int)response.StatusCode, "Error calling V1ResourcesResourceUriGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ResourcesResourceUriGet: " + response.ErrorMessage, response.ErrorMessage);

            return (ResourcePermission) ApiClient.Deserialize(response.Content, typeof(ResourcePermission), response.Headers);
        }

        /// <summary>
        /// Update a resource permissions object. Updates the global permissions on a resource. This applies to all users in an account.
        /// </summary>
        /// <param name="body">The contents of the permission to set on the resource. Overwrites existing data.</param>
        /// <param name="resourceUri">The uri path of a resource to validate, must be URL encoded, uri segments are allowed.</param>
        /// <returns>Object</returns>
        public Object V1ResourcesResourceUriPut (ResourcePermission body, string resourceUri)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ApiException(400, "Missing required parameter 'body' when calling V1ResourcesResourceUriPut");
            // verify the required parameter 'resourceUri' is set
            if (resourceUri == null) throw new ApiException(400, "Missing required parameter 'resourceUri' when calling V1ResourcesResourceUriPut");

            var path = "/v1/resources/{resourceUri}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "resourceUri" + "}", ApiClient.ParameterTostring(resourceUri));

            var queryParams = new Dictionary<string, string>();
            var headerParams = new Dictionary<string, string>();
            var formParams = new Dictionary<string, string>();
            var fileParams = new Dictionary<string, FileParameter>();
            string postBody = null;

                                                postBody = ApiClient.Serialize(body); // http body (model) parameter

            // authentication setting, if any
            string[] authSettings = new string[] { "oauth2" };

            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ResourcesResourceUriPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ResourcesResourceUriPut: " + response.ErrorMessage, response.ErrorMessage);

            return (Object) ApiClient.Deserialize(response.Content, typeof(Object), response.Headers);
        }

    }
}
