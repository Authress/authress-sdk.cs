using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class AuthressClient : IGroupsApi
    {
        /// <summary>
        /// Create group Specify users to be included in a new group. (Groups have a maximum size of ~100KB)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Group</returns>
        public async Task<Group> CreateGroup (Group body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ArgumentNullException("Missing required parameter 'body'.");

            var path = "/v1/groups";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.PostAsync(path, body.ToHttpContent()))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<Group>();
            }
        }

        /// <summary>
        /// Deletes group Remove a group, users will lose any role that was assigned through membership of this group. This action cannot be undone.
        /// </summary>
        /// <param name="groupId">The identifier of the group.</param>
        /// <returns></returns>
        public async Task DeleteGroup (string groupId)
        {
            // verify the required parameter 'groupId' is set
            if (groupId == null) throw new ArgumentNullException("Missing required parameter 'groupId'.");

            var path = $"/v1/groups/{System.Web.HttpUtility.UrlEncode(groupId)}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.DeleteAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
            }
        }

        /// <summary>
        /// Retrieve group A group contains multiple users which can be added to an access record, and should be assigned the same roles at the same time.
        /// </summary>
        /// <param name="groupId">The identifier of the group.</param>
        /// <returns>Group</returns>
        async public Task<Group> GetGroup (string groupId)
        {
            // verify the required parameter 'groupId' is set
            if (groupId == null) throw new ArgumentNullException("Missing required parameter 'groupId'.");

            var path = $"/v1/groups/{groupId}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<Group>();
            }
        }

        /// <summary>
        /// List groups Returns a paginated groups list for the account. Only groups the user has access to are returned. This query resource is meant for administrative actions only, therefore has a lower rate limit tier than user permissions related resources.
        /// </summary>
        /// <param name="limit">Max number of results to return</param>
        /// <param name="filter">Filter to search groups by. This is a case insensitive search through every text field.</param>
        /// <returns>Group</returns>
        async public Task<GroupCollection> GetGroups (int? limit = null, string filter = null)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "limit", limit?.ToString() },
                { "filter", filter }
            };

            var queryString = queryParams.Where(pair => !string.IsNullOrEmpty(pair.Value))
                .Select(pair => $"{pair.Key}={System.Web.HttpUtility.UrlEncode(pair.Value)}").Aggregate((next, total) => $"{total}&{next}");
            var path = $"/v1/groups?{queryString}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<GroupCollection>();
            }
        }

        /// <summary>
        /// Update a group Updates a group adding or removing user. Change a group updates the permissions and roles the users have access to. (Groups have a maximum size of ~100KB)
        /// </summary>
        /// <param name="groupId">The identifier of the group.</param>
        /// <param name="group">The Group data</param>
        /// <param name="expectedLastModifiedTime">The expected last time the group was updated</param>
        /// <returns>Group</returns>
        public async Task<Group> UpdateGroup (string groupId, Group group, DateTimeOffset? expectedLastModifiedTime = null)
        {
            if (group == null) throw new ArgumentNullException("Missing required parameter 'group'.");
            if (groupId == null) throw new ArgumentNullException("Missing required parameter 'groupId'.");

            var path = $"/v1/groups/{System.Web.HttpUtility.UrlEncode(groupId)}";
            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var request = new HttpRequestMessage(HttpMethod.Post, path))
            {
                request.Content = group.ToHttpContent();
                if (expectedLastModifiedTime.HasValue) {
                    request.Headers.Add("If-Unmodified-Since", expectedLastModifiedTime.Value.ToString("o", CultureInfo.InvariantCulture));
                }
                using (var response = await client.SendAsync(request))
                {
                    await response.ThrowIfNotSuccessStatusCode();
                    return await response.Content.ReadAsAsync<Group>();
                }
            }
        }

    }
}
