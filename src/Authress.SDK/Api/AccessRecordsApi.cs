using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class AuthressClient : IAccessRecordsApi
    {
        /// <summary>
        /// Claim a resource by an allowed user Claim a resource by allowing a user to pick an identifier and receive admin access to that resource if it hasn't already been claimed. This only works for resources specifically marked as "CLAIM". The result will be a new access record listing that user as the admin for this resource.
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
        /// List account records Returns a paginated records list for the account. Only records the user has access to are returned. This query resource is meant for administrative actions only, therefore has a lower rate limit tier than user permissions related resources.
        /// </summary>
        /// <param name="limit">Max number of results to return</param>
        /// <param name="cursor">Continuation cursor for paging (will automatically be set)</param>
        /// <param name="filter">Filter to search records by. This is a case insensitive search through every text field.</param>
        /// <param name="status">Filter records by their current status.</param>
        /// <returns>AccessRecordCollection</returns>
        public async Task<AccessRecordCollection> GetRecords (int? limit = null, string cursor = null, string filter = null, string status = null)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "limit", limit == null ? string.Empty : limit.ToString() },
                { "status", status },
                { "cursor", cursor },
                { "filter", filter }
            };

            var queryString = queryParams.Where(pair => !string.IsNullOrEmpty(pair.Value))
                .Select(pair => $"{pair.Key}={System.Web.HttpUtility.UrlEncode(pair.Value)}").Aggregate((next, total) => $"{total}&{next}");
            var path = $"/v1/records?{queryString}";
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

            var path = $"/v1/records/{System.Web.HttpUtility.UrlEncode(recordId)}";
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

            var path = $"/v1/records/{System.Web.HttpUtility.UrlEncode(recordId)}";
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

            var path = $"/v1/records/{System.Web.HttpUtility.UrlEncode(recordId)}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.PostAsync(path, body.ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<AccessRecord>();
            }
        }

        /// <summary>
        /// Create access request Specify a request in the form of an access record that an admin can approve.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>AccessRequest</returns>
        public async Task<AccessRequest> CreateRequest(AccessRequest body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ArgumentNullException("Missing required parameter 'body'.");

            var path = "/v1/requests";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.PostAsync(path, body.ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<AccessRequest>();
            }
        }

        /// <summary>
        /// Deletes access request Remove an access request.
        /// </summary>
        /// <param name="requestId">The identifier of the access request.</param>
        /// <returns></returns>
        public async Task DeleteRequest(string requestId)
        {
            // verify the required parameter 'recordId' is set
            if (requestId == null) throw new ArgumentNullException("Missing required parameter 'requestId'.");

            var path = $"/v1/requests/{System.Web.HttpUtility.UrlEncode(requestId)}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.DeleteAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
            }
        }

        /// <summary>
        /// Retrieve access request Access request contain information requesting permissions for users to resources.
        /// </summary>
        /// <param name="requestId">The identifier of the access request.</param>
        /// <returns>AccessRequest</returns>
        public async Task<AccessRequest> GetRequest(string requestId)
        {
            if (requestId == null) throw new ArgumentNullException("Missing required parameter 'requestId'.");

            var path = $"/v1/requests/{System.Web.HttpUtility.UrlEncode(requestId)}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<AccessRequest>();
            }
        }

        /// <summary>
        /// List access requests Returns a paginated request list. Only requests the user has access to are returned. This query resource is meant for administrative actions only, therefore has a lower rate limit tier than user permissions related resources.
        /// </summary>
        /// <param name="limit">Max number of results to return (optional, default to 20)</param>
        /// <param name="cursor">Continuation cursor for paging (will automatically be set) (optional)</param>
        /// <param name="status">Filter requests by their current status. (optional)</param>
        /// <returns>AccessRequestCollection</returns>
        public async Task<AccessRequestCollection> GetRequests(int? limit = null, string cursor = null, string status = null)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "limit", limit == null ? string.Empty : limit.ToString() },
                { "status", status },
                { "cursor", cursor }
            };

            var queryString = queryParams.Where(pair => !string.IsNullOrEmpty(pair.Value))
                .Select(pair => $"{pair.Key}={System.Web.HttpUtility.UrlEncode(pair.Value)}").Aggregate((next, total) => $"{total}&{next}");
            var path = $"/v1/requests?{queryString}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<AccessRequestCollection>();
            }
        }

        /// <summary>
        /// Approve or deny access request Updates an access request, approving it or denying it.
        /// </summary>
        /// <param name="requestId">The identifier of the access request.</param>
        /// <param name="body"></param>
        /// <returns>AccessRequest</returns>
        public async Task<AccessRequest> RespondToAccessRequest(string requestId, AccessRequestResponse body)
        {
            if (requestId == null) throw new ArgumentNullException("Missing required parameter 'requestId'.");
            if (body == null) throw new ArgumentNullException("Missing required parameter 'body'.");

            var path = $"/v1/requests/{System.Web.HttpUtility.UrlEncode(requestId)}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.PatchAsync(path, body.ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<AccessRequest>();
            }
        }

        /// <summary>
        /// Create user invite Invites are used to easily assign permissions to users that have not been created in your identity provider yet. 1. This generates an invite record. 2. Send the invite ID to the user. 3. Log the user in. 4. As the user PATCH the /invite/{inviteId} endpoint 5. This accepts the invite.         When the user accepts the invite they will automatically receive the permissions assigned in the Invite. Invites automatically expire after 7 days.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Invite</returns>
        public async Task<Invite> CreateInvite(Invite body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ArgumentNullException("Missing required parameter 'body'.");

            var path = "/v1/invites";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.PostAsync(path, body.ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<Invite>();
            }
        }

        /// <summary>
        /// Accept invite Accepts an invite by claiming this invite by this user. The user access token used for this request will gain the permissions associated with the invite.
        /// </summary>
        /// <param name="inviteId">The identifier of the invite.</param>
        /// <returns>Account</returns>
        public async Task<Account> RespondToInvite(string inviteId)
        {
            if (inviteId == null) throw new ArgumentNullException("Missing required parameter 'inviteId'.");

            var path = $"/v1/invites/{System.Web.HttpUtility.UrlEncode(inviteId)}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.PatchAsync(path, new Invite().ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<Account>();
            }
        }

        /// <summary>
        /// Delete invite Deletes an invite.
        /// </summary>
        /// <param name="inviteId">The identifier of the invite.</param>
        /// <returns></returns>
        public async Task DeleteInvite(string inviteId)
        {
            if (inviteId == null) throw new ArgumentNullException("Missing required parameter 'inviteId'.");

            var path = $"/v1/invites/{System.Web.HttpUtility.UrlEncode(inviteId)}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.DeleteAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
            }
        }
    }
}
