using System;
using System.Threading.Tasks;

namespace Authress.SDK
{
    /// <summary>
    /// Provides the token from a provider function.
    /// </summary>
    public class DynamicTokenProvider : ITokenProvider
    {
        private Func<Task<string>> resolver;

        /// <summary>
        /// Provides the token from a provider function. Specify a resolver function that when execution will return a bearer token to be used with Authress.
        /// The expectation is that this resolver could grab the current user's JWT from the incoming service call and pass it to the outgoing Authress service call.
        /// <param name="resolver">A function that will be called to resolve a token.</param>
        /// </summary>
        public DynamicTokenProvider(Func<Task<string>> resolver)
        {
            this.resolver = resolver;
        }

        /// <summary>
        /// Get the bearer token
        /// </summary>
        public async Task<string> GetBearerToken(string authressCustomDomainFallback = null)
        {
            var token = await resolver();
            return token;
        }
    }
}
