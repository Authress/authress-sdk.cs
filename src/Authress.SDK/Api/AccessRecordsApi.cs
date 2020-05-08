using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authress.SDK.Api;
using Authress.SDK.Client;
using Authress.SDK.DTO;

namespace Authress.SDK
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class AuthressClient : IAccessRecordsApi
    {
        /// <summary>
        /// Claim a resource by an allowed user Claim a resource by allowing a user to pick an identifier and recieve admin access to that resource if it hasn&#x27;t already been claimed. This only works for resources specifically marked as &lt;strong&gt;CLAIM&lt;/strong&gt;. The result will be a new access record listing that user as the admin for this resource.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>ClaimResponse</returns>
        public async Task<ClaimResponse> CreateClaim (ClaimRequest body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ArgumentNullException("Missing required parameter 'body'.");

            var path = "/v1/claims";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.PostAsync(path, body.ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<ClaimResponse>();
            }
        }

        /// <summary>
        /// Get all account records. Returns a paginated records list for the account. Only records the user has access to are returned.
        /// </summary>
        /// <returns>AccessRecord</returns>
        public async Task<AccessRecordCollection> GetRecords ()
        {

            var path = "/v1/records";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<AccessRecordCollection>();
            }
        }

        /// <summary>
        /// Create a new access record Specify user roles for specific resources.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>AccessRecord</returns>
        public async Task<AccessRecord> CreateRecord (AccessRecord body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ArgumentNullException("Missing required parameter 'body'.");

            var path = "/v1/records";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.PostAsync(path, body.ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<AccessRecord>();
            }
        }

        /// <summary>
        /// Deletes an access record. Remove an access record, removing associated permissions from all users in record. If a user has a permission from another record, that permission will not be removed.
        /// </summary>
        /// <param name="recordId">The identifier of the access record.</param>
        /// <returns></returns>
        public async Task DeleteRecord (string recordId)
        {
            // verify the required parameter 'recordId' is set
            if (recordId == null) throw new ArgumentNullException("Missing required parameter 'recordId'.");

            var path = $"/v1/records/{recordId}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.DeleteAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
            }
        }

        /// <summary>
        /// Get an access record for the account. Access records contain information assigning permissions to users for resources.
        /// </summary>
        /// <param name="recordId">The identifier of the access record.</param>
        /// <returns>AccessRecord</returns>
        public async Task<AccessRecord> GetRecord (string recordId)
        {
            // verify the required parameter 'recordId' is set
            if (recordId == null) throw new ArgumentNullException("Missing required parameter 'recordId'.");

            var path = $"/v1/records/{recordId}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<AccessRecord>();
            }
        }

        /// <summary>
        /// Update an access record. Updates an access record adding or removing user permissions to resources.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="recordId">The identifier of the access record.</param>
        /// <returns>AccessRecord</returns>
        public async Task<AccessRecord> UpdateRecord (string recordId, AccessRecord body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ArgumentNullException("Missing required parameter 'body'.");
            // verify the required parameter 'recordId' is set
            if (recordId == null) throw new ArgumentNullException("Missing required parameter 'recordId'.");

            var path = $"/v1/records/{recordId}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.PostAsync(path, body.ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<AccessRecord>();
            }
        }
    }
}
