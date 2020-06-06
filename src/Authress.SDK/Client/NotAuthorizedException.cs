using System;
using System.Net;

namespace Authress.SDK.Client
{
    /// <summary>
    /// User is forbidden from performing this action.
    /// </summary>
    public class NotAuthorizedException : Exception
    {
        /// <summary>
        /// User which is not authorized.
        /// </summary>
        public string User { get; }

        /// <summary>
        /// The relevant resource used in the permissions check.
        /// </summary>
        public string ResourceUri { get; }

        /// <summary>
        /// The missing permission.
        /// </summary>
        public string Permission { get; }

        internal NotAuthorizedException(string user, string resourceUri, string permission) : base("User does not have the required permission to access the resource.")
        {
            User = user;
            ResourceUri = resourceUri;
            Permission = permission;
        }
    }
}
