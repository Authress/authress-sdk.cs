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
    public interface IAccountsApi
    {
        /// <summary>
        /// Get account information. Includes the original configuration information.
        /// </summary>
        /// <param name="accountId">The unique identifier for the account</param>
        /// <returns>Account</returns>
        Account V1AccountsAccountIdGet (string accountId);
        /// <summary>
        /// Get all accounts user has access to Returns a list of accounts that the user has access to.
        /// </summary>
        /// <returns>AccountCollection</returns>
        AccountCollection V1AccountsGet ();
        /// <summary>
        /// Get all linked identities for this account. Returns a list of identities linked for this account.
        /// </summary>
        /// <returns>IdentityCollection</returns>
        IdentityCollection V1IdentitiesGet ();
        /// <summary>
        /// Link a new account identity. An identity is a JWT subscriber *sub* and issuer *iss*. Only one account my be linked to a particular JWT combination. Allows calling the API with a federated token directly instead of using a client access key.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Object</returns>
        Object V1IdentitiesPost (IdentityRequest body);
    }
}
