using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Authress.SDK.Api;
using Authress.SDK.Client;
using Authress.SDK.DTO;

namespace Authress.SDK
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
        public async Task<Account> GetAccount (string accountId)
        {
            // verify the required parameter 'accountId' is set
            if (accountId == null) throw new ArgumentNullException("Missing required parameter 'accountId'.");

            var path = $"/v1/accounts/{accountId}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<Account>();
            }
        }

        /// <summary>
        /// Get all accounts user has access to Returns a list of accounts that the user has access to.
        /// </summary>
        /// <returns>AccountCollection</returns>
        public async Task<AccountCollection> GetAccounts ()
        {

            var path = "/v1/accounts";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<AccountCollection>();
            }
        }

        /// <summary>
        /// Get all linked identities for this account. Returns a list of identities linked for this account.
        /// </summary>
        /// <returns>IdentityCollection</returns>
        public async Task<IdentityCollection> GetAccountIdentities ()
        {

            var path = "/v1/identities";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<IdentityCollection>();
            }
        }

        /// <summary>
        /// Link a new account identity. An identity is a JWT subscriber *sub* and issuer *iss*. Only one account my be linked to a particular JWT combination. Allows calling the API with a federated token directly instead of using a client access key.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Object</returns>
        public async Task CreateIdentity (IdentityRequest body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ArgumentNullException("Missing required parameter 'body'.");

            var path = "/v1/identities";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.PostAsync(path, body.ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
            }
        }

    }
}
