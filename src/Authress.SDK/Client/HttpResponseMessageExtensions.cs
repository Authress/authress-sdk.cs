using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Authress.SDK.Client
{
    internal static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Throws an exception when the status code indicates an unsuccessful response.
        /// </summary>
        /// <param name="message">Extension on a HttpResponseMessage.</param>
        /// <returns>Returns an awaitable Task.</returns>
        /// <exception cref="NotSuccessHttpResponseException">Thrown when not success status code</exception>
        public static async Task ThrowIfNotSuccessStatusCode(this HttpResponseMessage message)
        {
            if (!message.IsSuccessStatusCode)
            {
                if (message.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new InvalidTokenException();
                }

                if (message.StatusCode == HttpStatusCode.PaymentRequired)
                {
                    throw new PaymentRequiredException();
                }

                if ((int)message.StatusCode == 429)
                {
                    throw new TooManyRequestsException();
                }

                var formattedMsg = await FormatErrorMessage(message);
                throw new NotSuccessHttpResponseException(message.StatusCode, formattedMsg);
            }
        }

        public static async Task<string> FormatErrorMessage(this HttpResponseMessage message)
        {
            var msg = await message.Content.ReadAsStringAsync();
            var formattedMsg = $"Error processing request. Status code was {message.StatusCode} when calling '{message.RequestMessage?.RequestUri?.ToString()}', message was '{msg}'";
            return formattedMsg;
        }
    }
}
