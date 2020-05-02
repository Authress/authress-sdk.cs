using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    /// A collect of permissions that the user has to a resource.
    /// </summary>
    [DataContract]
    public class InlineResponse200
    {
        /// <summary>
        /// Gets or Sets Account
        /// </summary>
        [DataMember(Name = "account", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "account")]
        public InlineResponse200Account Account { get; set; }

        /// <summary>
        /// Gets or Sets UserId
        /// </summary>
        [DataMember(Name = "userId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        /// <summary>
        /// A list of the permissions
        /// </summary>
        /// <value>A list of the permissions</value>
        [DataMember(Name = "permissions", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "permissions")]
        public List<PermissionObject> Permissions { get; set; }
    }
}
