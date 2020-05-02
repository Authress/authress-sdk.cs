using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    /// A collection of resource permissions that have been defined.
    /// </summary>
    [DataContract]
    public class ResourcePermissionCollection
    {
        /// <summary>
        /// Gets or Sets Resources
        /// </summary>
        [DataMember(Name = "resources", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "resources")]
        public ResourcePermission Resources { get; set; }
    }
}
