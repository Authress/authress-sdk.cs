using System;
using System.Net;

namespace Authress.SDK.Client
{
    /// <summary>
    /// Library specific error for capturing failed API calls
    /// </summary>
    public class PaymentRequiredException : Exception
    {
        internal PaymentRequiredException() : base("The account has exceeded the allowed API call limit for the current plan. Configuration or upgrade is required before more calls can be made.") {}
    }
}
