using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Authress.SDK.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IAccessRecordsApi
    {
        /// <summary>
        /// Claim a resource by an allowed user Claim a resource by allowing a user to pick an identifier and recieve admin access to that resource if it hasn&#x27;t already been claimed. This only works for resources specifically marked as &lt;strong&gt;CLAIM&lt;/strong&gt;. The result will be a new access record listing that user as the admin for this resource.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>ClaimResponse</returns>
        Task<ClaimResponse> CreateClaim (ClaimRequest body);
        /// <summary>
        /// Get all account records. Returns a paginated records list for the account. Only records the user has access to are returned.
        /// </summary>
        /// <returns>AccessRecord</returns>
        Task<AccessRecord> GetRecords ();
        /// <summary>
        /// Create a new access record Specify user roles for specific resources.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>AccessRecord</returns>
        Task<AccessRecord> CreateRecord (AccessRecord body);
        /// <summary>
        /// Deletes an access record. Remove an access record, removing associated permissions from all users in record. If a user has a permission from another record, that permission will not be removed.
        /// </summary>
        /// <param name="recordId">The identifier of the access record.</param>
        /// <returns></returns>
        Task DeleteRecord (string recordId);
        /// <summary>
        /// Get an access record for the account. Access records contain information assigning permissions to users for resources.
        /// </summary>
        /// <param name="recordId">The identifier of the access record.</param>
        /// <returns>AccessRecord</returns>
        Task<AccessRecord> GetRecord (string recordId);
        /// <summary>
        /// Update an access record. Updates an access record adding or removing user permissions to resources.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="recordId">The identifier of the access record.</param>
        /// <returns>AccessRecord</returns>
        Task<AccessRecord> UpdateRecord (string recordId, AccessRecord body);
    }
}
