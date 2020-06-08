using System.Threading.Tasks;
using Authress.SDK.DTO;

namespace Authress.SDK.Api
{
    /// <summary>
    /// Helper to fetch next pages
    /// </summary>
    public interface IPaginator
    {
        /// <summary>
        /// Get next page
        /// </summary>
        /// <param name="collectionResource">A collection resource object, which should contain a next link property</param>
        /// <returns></returns>
        Task<T> GetNextPage<T> (T collectionResource) where T : IPaginationDto, new();
    }
}
