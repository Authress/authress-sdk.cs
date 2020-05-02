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
    public interface IServiceClientsApi
    {
        /// <summary>
        /// Remove an access key for a client Deletes an access key for a client prevent it from being used to authenticate with Authress.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client.</param>
        /// <param name="keyId">The id of the access key to remove from the client.</param>
        /// <returns></returns>
        void V1ClientsClientIdAccessKeysKeyIdDelete (string clientId, string keyId);
        /// <summary>
        /// Request a new access key Create a new access key for the client so that a service can authenticate with Authress as that client. Using the client will allow delegation of permission checking of users.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client.</param>
        /// <returns>ClientAccessKey</returns>
        ClientAccessKey V1ClientsClientIdAccessKeysPost (string clientId);
        /// <summary>
        /// Delete a client This deletes the service client.
        /// </summary>
        /// <param name="clientId">The unique identifier for the client.</param>
        /// <returns></returns>
        void V1ClientsClientIdDelete (string clientId);
        /// <summary>
        /// Get a client. Returns all information related to client except for the private access keys.
        /// </summary>
        /// <param name="clientId">The unique identifier for the client.</param>
        /// <returns>Client</returns>
        ServiceClient V1ClientsClientIdGet (string clientId);
        /// <summary>
        /// Update a client Updates a client information.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="clientId">The unique identifier for the client.</param>
        /// <returns>Client</returns>
        ServiceClient V1ClientsClientIdPut (ServiceClient body, string clientId);
        /// <summary>
        /// Get clients collection Returns all clients that the user has access to in the account.
        /// </summary>
        /// <returns>ClientCollection</returns>
        ClientCollection V1ClientsGet ();
        /// <summary>
        /// Create a new client. Creates a service client to interact with Authress or any other service on behalf of users. Each client has secret private keys used to authenticate with Authress. To use service clients created through other mechanisms, skip creating a client and create access records with the client identifier.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Client</returns>
        ServiceClient V1ClientsPost (ServiceClient body);
    }
}
