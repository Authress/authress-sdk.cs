using System;
using System.Threading.Tasks;
using Authress.SDK.DTO;

namespace Authress.SDK

{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IGroupsApi
    {
        /// <summary>
        /// Create group Specify users to be included in a new group. (Groups have a maximum size of ~100KB)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Group</returns>
        Task<Group> CreateGroup (Group body);
        /// <summary>
        /// Deletes group Remove a group, users will lose any role that was assigned through membership of this group. This action cannot be undone.
        /// </summary>
        /// <param name="groupId">The identifier of the group.</param>
        /// <returns></returns>
        Task DeleteGroup (string groupId);
        /// <summary>
        /// Retrieve group A group contains multiple users which can be added to an access record, and should be assigned the same roles at the same time.
        /// </summary>
        /// <param name="groupId">The identifier of the group.</param>
        /// <returns>Group</returns>
        Task<Group> GetGroup (string groupId);
        /// <summary>
        /// List groups Returns a paginated groups list for the account. Only groups the user has access to are returned. This query resource is meant for administrative actions only, therefore has a lower rate limit tier than user permissions related resources.
        /// </summary>
        /// <param name="limit">Max number of results to return</param>
        /// <param name="filter">Filter to search groups by. This is a case insensitive search through every text field.</param>
        /// <returns>Group</returns>
        Task<GroupCollection> GetGroups (int? limit = null, string filter = null);
        /// <summary>
        /// Update a group Updates a group adding or removing user. Change a group updates the permissions and roles the users have access to. (Groups have a maximum size of ~100KB)
        /// </summary>
        /// <param name="groupId">The identifier of the group.</param>
        /// <param name="group"></param>
        /// <param name="expectedLastModifiedTime">The expected last time the group was updated</param>
        /// <returns>Group</returns>
        Task<Group> UpdateGroup (string groupId, Group group, DateTimeOffset? expectedLastModifiedTime = null);
    }
}
