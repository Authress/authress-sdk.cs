using System;
using System.Net;

namespace Authress.SDK.Client
{
    /// <summary>
    /// Library specific error for capturing failed API calls
    /// </summary>
    public class TooManyRequestsException : Exception
    {
        internal TooManyRequestsException() : base("The client authorized user has exceeded the current limit of requests as defined by the account. Configure exponentional backoff or increase allowed requests.") {}
    }
}
