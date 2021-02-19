using System.Threading.Tasks;

namespace Authress.SDK
{
    /// <summary>
    /// Provides the token from locally stored access key.
    /// </summary>
    public class ManualTokenProvider : ITokenProvider
    {
        private string token;

        /// <summary>
        /// Set the bearer token
        /// </summary>
        public void SetToken(string token)
        {
            this.token = token;
        }

        /// <summary>
        /// Get the bearer token
        /// </summary>
        public Task<string> GetBearerToken()
        {
            return Task.FromResult(token);
        }
    }
}
