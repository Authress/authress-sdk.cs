using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    /// The identifying information which uniquely links a JWT OIDC token to an identity provider
    /// </summary>
    [DataContract]
    public class Identity
    {
        /// <summary>
        /// The issuer of the JWT token. This can be any OIDC compliant provider.
        /// </summary>
        /// <value>The issuer of the JWT token. This can be any OIDC compliant provider.</value>
        [DataMember(Name = "issuer", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// The target of the JWT tokens. This must be a sub target of the account app or the whole app
        /// </summary>
        /// <value>The target of the JWT tokens. This must be a sub target of the account app or the whole app</value>
        [DataMember(Name = "audience", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "audience")]
        public string Audience { get; set; }
    }
}
