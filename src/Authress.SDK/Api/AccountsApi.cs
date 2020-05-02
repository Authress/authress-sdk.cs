using System;
using System.Collections.Generic;
using System.Net.Http;
using Authress.SDK.DTO;

namespace Authress.SDK.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class AuthressClient : IAccountsApi
    {
        /// <summary>
        /// Get account information. Includes the original configuration information.
        /// </summary>
        /// <param name="accountId">The unique identifier for the account</param>
        /// <returns>Account</returns>
        public Account V1AccountsAccountIdGet (string accountId)
        {
            // verify the required parameter 'accountId' is set
            if (accountId == null) throw new ApiException(400, "Missing required parameter 'accountId' when calling V1AccountsAccountIdGet");

            var path = "/v1/accounts/{accountId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "accountId" + "}", ApiClient.ParameterTostring(accountId));

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
                throw new ApiException ((int)response.StatusCode, "Error calling V1AccountsAccountIdGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1AccountsAccountIdGet: " + response.ErrorMessage, response.ErrorMessage);

            return (Account) ApiClient.Deserialize(response.Content, typeof(Account), response.Headers);
        }

        /// <summary>
        /// Get all accounts user has access to Returns a list of accounts that the user has access to.
        /// </summary>
        /// <returns>AccountCollection</returns>
        public AccountCollection V1AccountsGet ()
        {

            var path = "/v1/accounts";
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
                throw new ApiException ((int)response.StatusCode, "Error calling V1AccountsGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1AccountsGet: " + response.ErrorMessage, response.ErrorMessage);

            return (AccountCollection) ApiClient.Deserialize(response.Content, typeof(AccountCollection), response.Headers);
        }

        /// <summary>
        /// Get all linked identities for this account. Returns a list of identities linked for this account.
        /// </summary>
        /// <returns>IdentityCollection</returns>
        public IdentityCollection V1IdentitiesGet ()
        {

            var path = "/v1/identities";
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
                throw new ApiException ((int)response.StatusCode, "Error calling V1IdentitiesGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1IdentitiesGet: " + response.ErrorMessage, response.ErrorMessage);

            return (IdentityCollection) ApiClient.Deserialize(response.Content, typeof(IdentityCollection), response.Headers);
        }

        /// <summary>
        /// Link a new account identity. An identity is a JWT subscriber *sub* and issuer *iss*. Only one account my be linked to a particular JWT combination. Allows calling the API with a federated token directly instead of using a client access key.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Object</returns>
        public Object V1IdentitiesPost (IdentityRequest body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ApiException(400, "Missing required parameter 'body' when calling V1IdentitiesPost");

            var path = "/v1/identities";
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
                throw new ApiException ((int)response.StatusCode, "Error calling V1IdentitiesPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling V1IdentitiesPost: " + response.ErrorMessage, response.ErrorMessage);

            return (Object) ApiClient.Deserialize(response.Content, typeof(Object), response.Headers);
        }

    }
}
