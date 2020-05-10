using System;
using System.Net;

namespace Authress.SDK.Client
{
    /// <summary>
    /// User token is invalid.
    /// </summary>
    public class InvalidTokenException : Exception
    {
        internal InvalidTokenException() : base("Token is invalid, it have expired is not allowed to be used to access Authress with the current configuration") {}
    }
}
