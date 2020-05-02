using System;

namespace Authress.SDK.Client
{
    /// <summary>
    /// Library specific error for capturing failed API calls
    /// </summary>
    public class NotSuccessHttpResponseException : Exception
    {
        internal NotSuccessHttpResponseException(string message) : base(message) {}
    }
}
