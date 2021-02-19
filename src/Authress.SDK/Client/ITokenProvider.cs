using System.Threading.Tasks;

namespace Authress.SDK
{
    /// <summary>
    /// Provides the token necessary for Authress authentication
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// Get the bearer token
        /// </summary>
        Task<string> GetBearerToken();
    }
}
