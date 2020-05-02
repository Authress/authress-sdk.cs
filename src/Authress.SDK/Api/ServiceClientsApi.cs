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
    public partial class AuthressClient : IServiceClientsApi
    {
        /// <summary>
        /// Remove an access key for a client Deletes an access key for a client prevent it from being used to authenticate with Authress.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client.</param>
        /// <param name="keyId">The id of the access key to remove from the client.</param>
        /// <returns></returns>
        public async Task DeleteAccessKey (string clientId, string keyId)
        {
            // verify the required parameter 'clientId' is set
            if (clientId == null) throw new ApiException(400, "Missing required parameter 'clientId' when calling V1ClientsClientIdAccessKeysKeyIdDelete");
            // verify the required parameter 'keyId' is set
            if (keyId == null) throw new ApiException(400, "Missing required parameter 'keyId' when calling V1ClientsClientIdAccessKeysKeyIdDelete");

            var path = $"/v1/clients/{clientId}/access-keys/{keyId}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.DeleteAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
            }
        }

        /// <summary>
        /// Request a new access key Create a new access key for the client so that a service can authenticate with Authress as that client. Using the client will allow delegation of permission checking of users.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client.</param>
        /// <returns>ClientAccessKey</returns>
        public async Task<ClientAccessKey> RequestAccessKey (string clientId)
        {
            // verify the required parameter 'clientId' is set
            if (clientId == null) throw new ApiException(400, "Missing required parameter 'clientId' when calling V1ClientsClientIdAccessKeysPost");

            var path = $"/v1/clients/{clientId}/access-keys";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            var body = new AccessKeyRequest();
            using (var response = await client.PostAsync(path, body.ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<ClientAccessKey>();
            }
        }

        /// <summary>
        /// Delete a client This deletes the service client.
        /// </summary>
        /// <param name="clientId">The unique identifier for the client.</param>
        /// <returns></returns>
        public async Task DeleteClient (string clientId)
        {
            // verify the required parameter 'clientId' is set
            if (clientId == null) throw new ApiException(400, "Missing required parameter 'clientId' when calling V1ClientsClientIdDelete");

            var path = $"/v1/clients/{clientId}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.DeleteAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
            }
        }

        /// <summary>
        /// Get a client. Returns all information related to client except for the private access keys.
        /// </summary>
        /// <param name="clientId">The unique identifier for the client.</param>
        /// <returns>Client</returns>
        public async Task<ServiceClient> GetClient (string clientId)
        {
            // verify the required parameter 'clientId' is set
            if (clientId == null) throw new ApiException(400, "Missing required parameter 'clientId' when calling V1ClientsClientIdGet");

            var path = $"/v1/clients/{clientId}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<ServiceClient>();
            }
        }

        /// <summary>
        /// Update a client Updates a client information.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="clientId">The unique identifier for the client.</param>
        /// <returns>Client</returns>
        public async Task<ServiceClient> UpdateClient (string clientId, ServiceClient body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ApiException(400, "Missing required parameter 'body' when calling V1ClientsClientIdPut");
            // verify the required parameter 'clientId' is set
            if (clientId == null) throw new ApiException(400, "Missing required parameter 'clientId' when calling V1ClientsClientIdPut");

            var path = $"/v1/clients/{clientId}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.PutAsync(path, body.ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<ServiceClient>();
            }
        }

        /// <summary>
        /// Get clients collection Returns all clients that the user has access to in the account.
        /// </summary>
        /// <returns>ClientCollection</returns>
        public async Task<ClientCollection> GetClients ()
        {

            var path = "/v1/clients";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<ClientCollection>();
            }
        }

        /// <summary>
        /// Create a new client. Creates a service client to interact with Authress or any other service on behalf of users. Each client has secret private keys used to authenticate with Authress. To use service clients created through other mechanisms, skip creating a client and create access records with the client identifier.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Client</returns>
        public async Task<ServiceClient> CreateClient (ServiceClient body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ApiException(400, "Missing required parameter 'body' when calling V1ClientsPost");

            var path = "/v1/clients";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.PostAsync(path, body.ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<ServiceClient>();
            }
        }

    }
}
