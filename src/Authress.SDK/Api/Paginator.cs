using System;
using System.Threading.Tasks;
using Authress.SDK.Api;
using Authress.SDK.Client;
using Authress.SDK.DTO;

namespace Authress.SDK
{
    /// <summary>
    /// Helper to fetch next pages
    /// </summary>
    public partial class AuthressClient : IPaginator
    {
        /// <summary>
        /// Get next page
        /// </summary>
        /// <param name="collectionResource">A collection resource object, which should contain a next link property</param>
        /// <returns></returns>
        public async Task<T> GetNextPage<T> (T collectionResource) where T : IPaginationDto, new()
        {
            if (collectionResource == null) throw new ArgumentNullException("CollectionResource is required.");

            var path = collectionResource?.Links?.Next?.Href;
            if (string.IsNullOrEmpty(path)) {
                return new T();
            }

            var client = await authressHttpClientProvider.GetHttpClientAsync();
            using (var response = await client.GetAsync(path))
            {
                await response.ThrowIfNotSuccessStatusCode();
                return await response.Content.ReadAsAsync<T>();
            }
        }
    }
}
