using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    /// Request to link an identity provider&#x27;s audience and your app&#x27;s audience with Authress.
    /// </summary>
    [DataContract]
    public class IdentityRequest
    {
        /// <summary>
        /// A valid JWT OIDC compliant token which will still pass authentication requests to the identity provider. Must contain a unique audience and issuer.
        /// </summary>
        /// <value>A valid JWT OIDC compliant token which will still pass authentication requests to the identity provider. Must contain a unique audience and issuer.</value>
        [DataMember(Name = "jwt", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "jwt")]
        public string Jwt { get; set; }

        /// <summary>
        /// If the `jwt` token contains more than one valid audience, then the single audience that should associated with Authress. If more than one audience is preferred, repeat this call with each one.
        /// </summary>
        /// <value>If the `jwt` token contains more than one valid audience, then the single audience that should associated with Authress. If more than one audience is preferred, repeat this call with each one.</value>
        [DataMember(Name = "preferredAudience", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "preferredAudience")]
        public string PreferredAudience { get; set; }
    }
}
