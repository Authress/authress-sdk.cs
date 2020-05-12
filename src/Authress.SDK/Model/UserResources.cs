using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    /// A collect of resource uri that a user has access to.
    /// </summary>
    [DataContract]
    public class UserResources
    {
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
        [DataMember(Name = "resources", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "resources")]
        public List<AccessRecordResource> Resources { get; set; }
    }
}
