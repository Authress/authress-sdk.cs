using System;
using System.Net;

namespace Authress.SDK.Client
{
    /// <summary>
    /// Library specific error for capturing failed API calls
    /// </summary>
    public class NotSuccessHttpResponseException : Exception
    {
        /// <summary>
        /// Status code of response from server that was >= 400.
        /// </summary>
        public HttpStatusCode ResponseStatusCode { get; private set; }

        internal NotSuccessHttpResponseException(HttpStatusCode responseStatusCode, string message) : base(message)
        {
            ResponseStatusCode = responseStatusCode;
        }
    }
}
