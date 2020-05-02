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
    public partial class AuthressClient : IServiceClientsApi
    {
        /// <summary>
        /// Remove an access key for a client Deletes an access key for a client prevent it from being used to authenticate with Authress.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client.</param>
        /// <param name="keyId">The id of the access key to remove from the client.</param>
        /// <returns></returns>
        public void V1ClientsClientIdAccessKeysKeyIdDelete (string clientId, string keyId)
        {
            // verify the required parameter 'clientId' is set
            if (clientId == null) throw new ApiException(400, "Missing required parameter 'clientId' when calling V1ClientsClientIdAccessKeysKeyIdDelete");
            // verify the required parameter 'keyId' is set
            if (keyId == null) throw new ApiException(400, "Missing required parameter 'keyId' when calling V1ClientsClientIdAccessKeysKeyIdDelete");

            var path = "/v1/clients/{clientId}/access-keys/{keyId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "clientId" + "}", ApiClient.ParameterTostring(clientId));
path = path.Replace("{" + "keyId" + "}", ApiClient.ParameterTostring(keyId));

            var queryParams = new Dictionary<string, string>();
            var headerParams = new Dictionary<string, string>();
            var formParams = new Dictionary<string, string>();
            var fileParams = new Dictionary<string, FileParameter>();
            string postBody = null;


            // authentication setting, if any
            string[] authSettings = new string[] { "oauth2" };

            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsClientIdAccessKeysKeyIdDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsClientIdAccessKeysKeyIdDelete: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Request a new access key Create a new access key for the client so that a service can authenticate with Authress as that client. Using the client will allow delegation of permission checking of users.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client.</param>
        /// <returns>ClientAccessKey</returns>
        public ClientAccessKey V1ClientsClientIdAccessKeysPost (string clientId)
        {
            // verify the required parameter 'clientId' is set
            if (clientId == null) throw new ApiException(400, "Missing required parameter 'clientId' when calling V1ClientsClientIdAccessKeysPost");

            var path = "/v1/clients/{clientId}/access-keys";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "clientId" + "}", ApiClient.ParameterTostring(clientId));

            var queryParams = new Dictionary<string, string>();
            var headerParams = new Dictionary<string, string>();
            var formParams = new Dictionary<string, string>();
            var fileParams = new Dictionary<string, FileParameter>();
            string postBody = null;


            // authentication setting, if any
            string[] authSettings = new string[] { "oauth2" };

            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsClientIdAccessKeysPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsClientIdAccessKeysPost: " + response.ErrorMessage, response.ErrorMessage);

            return (ClientAccessKey) ApiClient.Deserialize(response.Content, typeof(ClientAccessKey), response.Headers);
        }

        /// <summary>
        /// Delete a client This deletes the service client.
        /// </summary>
        /// <param name="clientId">The unique identifier for the client.</param>
        /// <returns></returns>
        public void V1ClientsClientIdDelete (string clientId)
        {
            // verify the required parameter 'clientId' is set
            if (clientId == null) throw new ApiException(400, "Missing required parameter 'clientId' when calling V1ClientsClientIdDelete");

            var path = "/v1/clients/{clientId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "clientId" + "}", ApiClient.ParameterTostring(clientId));

            var queryParams = new Dictionary<string, string>();
            var headerParams = new Dictionary<string, string>();
            var formParams = new Dictionary<string, string>();
            var fileParams = new Dictionary<string, FileParameter>();
            string postBody = null;


            // authentication setting, if any
            string[] authSettings = new string[] { "oauth2" };

            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsClientIdDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsClientIdDelete: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Get a client. Returns all information related to client except for the private access keys.
        /// </summary>
        /// <param name="clientId">The unique identifier for the client.</param>
        /// <returns>Client</returns>
        public ServiceClient V1ClientsClientIdGet (string clientId)
        {
            // verify the required parameter 'clientId' is set
            if (clientId == null) throw new ApiException(400, "Missing required parameter 'clientId' when calling V1ClientsClientIdGet");

            var path = "/v1/clients/{clientId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "clientId" + "}", ApiClient.ParameterTostring(clientId));

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
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsClientIdGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsClientIdGet: " + response.ErrorMessage, response.ErrorMessage);

            return (ServiceClient) ApiClient.Deserialize(response.Content, typeof(ServiceClient), response.Headers);
        }

        /// <summary>
        /// Update a client Updates a client information.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="clientId">The unique identifier for the client.</param>
        /// <returns>Client</returns>
        public ServiceClient V1ClientsClientIdPut (ServiceClient body, string clientId)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ApiException(400, "Missing required parameter 'body' when calling V1ClientsClientIdPut");
            // verify the required parameter 'clientId' is set
            if (clientId == null) throw new ApiException(400, "Missing required parameter 'clientId' when calling V1ClientsClientIdPut");

            var path = "/v1/clients/{clientId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "clientId" + "}", ApiClient.ParameterTostring(clientId));

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
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsClientIdPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsClientIdPut: " + response.ErrorMessage, response.ErrorMessage);

            return (ServiceClient) ApiClient.Deserialize(response.Content, typeof(ServiceClient), response.Headers);
        }

        /// <summary>
        /// Get clients collection Returns all clients that the user has access to in the account.
        /// </summary>
        /// <returns>ClientCollection</returns>
        public ClientCollection V1ClientsGet ()
        {

            var path = "/v1/clients";
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
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsGet: " + response.ErrorMessage, response.ErrorMessage);

            return (ClientCollection) ApiClient.Deserialize(response.Content, typeof(ClientCollection), response.Headers);
        }

        /// <summary>
        /// Create a new client. Creates a service client to interact with Authress or any other service on behalf of users. Each client has secret private keys used to authenticate with Authress. To use service clients created through other mechanisms, skip creating a client and create access records with the client identifier.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Client</returns>
        public ServiceClient V1ClientsPost (ServiceClient body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ApiException(400, "Missing required parameter 'body' when calling V1ClientsPost");

            var path = "/v1/clients";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<string, string>();
            var headerParams = new Dictionary<string, string>();
            var formParams = new Dictionary<string, string>();
            var fileParams = new Dictionary<string, FileParameter>();
            string postBody = null;

                                                postBody = ApiClient.Serialize(body); // http body (model) parameter

            // authentication setting, if any
            string[] authSettings = new string[] { "oauth2" };

            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1ClientsPost: " + response.ErrorMessage, response.ErrorMessage);

            return (ServiceClient) ApiClient.Deserialize(response.Content, typeof(ServiceClient), response.Headers);
        }

    }
}
