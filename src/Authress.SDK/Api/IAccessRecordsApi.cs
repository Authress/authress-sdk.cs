using System.Threading.Tasks;
using Authress.SDK.DTO;

namespace Authress.SDK.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IAccessRecordsApi
    {
        /// <summary>
        /// Claim a resource by an allowed user Claim a resource by allowing a user to pick an identifier and receive admin access to that resource if it hasn't already been claimed. This only works for resources specifically marked as "CLAIM". The result will be a new access record listing that user as the admin for this resource.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>ClaimResponse</returns>
        Task<ClaimResponse> CreateClaim (ClaimRequest body);

        #region Access Requests
        /// <summary>
        /// Get all account records. Returns a paginated records list for the account. Only records the user has access to are returned.
        /// </summary>
        /// <param name="limit">Max number of results to return</param>
        /// <param name="cursor">Continuation cursor for paging (will automatically be set)</param>
        /// <param name="filter">Filter to search records by. This is a case insensitive search through every text field.</param>
        /// <param name="status">Filter records by their current status.</param>
        /// <returns>AccessRecordCollection</returns>
        Task<AccessRecordCollection> GetRecords (int? limit = null, string cursor = null, string filter = null, string status = null);
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
        #endregion

        #region Access Requests
        /// <summary>
        /// Create access request
        /// </summary>
        /// <remarks>
        /// Specify a request in the form of an access record that an admin can approve.
        /// </remarks>
        /// <param name="body"></param>
        /// <returns>AccessRequest</returns>
        Task<AccessRequest> CreateRequest (AccessRequest body);

        /// <summary>
        /// Deletes access request
        /// </summary>
        /// <remarks>
        /// Remove an access request.
        /// </remarks>
        /// <param name="requestId">The identifier of the access request.</param>
        /// <returns>Task</returns>
        Task DeleteRequest (string requestId);

        /// <summary>
        /// Retrieve access request
        /// </summary>
        /// <remarks>
        /// Access request contain information requesting permissions for users to resources.
        /// </remarks>

        /// <param name="requestId">The identifier of the access request.</param>
        /// <returns>AccessRequest</returns>
        Task<AccessRequest> GetRequest (string requestId);

        /// <summary>
        /// List access requests
        /// </summary>
        /// <remarks>
        /// Returns a paginated request list. Only requests the user has access to are returned. This query resource is meant for administrative actions only, therefore has a lower rate limit tier than user permissions related resources.
        /// </remarks>
        /// <param name="limit">Max number of results to return (optional, default to 20)</param>
        /// <param name="cursor">Continuation cursor for paging (will automatically be set) (optional)</param>
        /// <param name="status">Filter records by their current status. (optional)</param>
        /// <returns>AccessRequestCollection</returns>
        Task<AccessRequestCollection> GetRequests (int? limit = null, string cursor = null, string status = null);

        /// <summary>
        /// Approve or deny access request
        /// </summary>
        /// <remarks>
        /// Updates an access request, approving it or denying it.
        /// </remarks>
        /// <param name="requestId">The identifier of the access request.</param>
        /// <param name="body"></param>
        /// <returns>AccessRequest</returns>
        Task<AccessRequest> RespondToAccessRequest (string requestId, AccessRequestResponse body);
        #endregion

        #region Invites
        /// <summary>
        /// Create user invite Invites are used to easily assign permissions to users that have not been created in your identity provider yet.
        /// 1. This generates an invite record.
        /// 2. Send the invite ID to the user.
        /// 3. Log the user in.
        /// 4. As the user PATCH the /invite/{inviteId} endpoint
        /// 5. This accepts the invite.
        /// When the user accepts the invite they will automatically receive the permissions assigned in the Invite. Invites automatically expire after 7 days.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Invite</returns>
        Task<Invite> CreateInvite (Invite body);

        /// <summary>
        /// Accept invite
        /// </summary>
        /// <remarks>
        /// Accepts an invite by claiming this invite by this user. The user access token used for this request will gain the permissions associated with the invite.
        /// </remarks>
        /// <param name="inviteId">The identifier of the invite.</param>
        /// <returns>Account</returns>
        Task<Account> RespondToInvite (string inviteId);

        /// <summary>
        /// Delete invite
        /// </summary>
        /// <remarks>
        /// Deletes an invite.
        /// </remarks>
        /// <param name="inviteId">The identifier of the invite.</param>
        /// <returns>Task</returns>
        Task DeleteInvite (string inviteId);
        #endregion
    }
}
