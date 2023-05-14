using System;
using System.Net;

namespace Authress.SDK.Client
{
    /// <summary>
    /// User token is invalid.
    /// </summary>
    public class InvalidTokenException : Exception
    {
        internal InvalidTokenException() : base("Token is invalid, it may have expired and is not allowed to be used to access Authress with the current configuration. If this is a User JWT Access tokens, return a 401 back to the caller. For service client access tokens, generate a new one at: https://authress.io/app/#/settings?focus=clients.") {}
    }
}
