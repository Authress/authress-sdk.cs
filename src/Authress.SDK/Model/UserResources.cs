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
    public class UserResources : IPaginationDto
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
        public List<AccessRecordResource> Resources { get; set; } = new List<AccessRecordResource>();

        /// <summary>
        /// The Links associated with this collection
        /// </summary>
        [DataMember(Name = "links", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }

        /// <summary>
        /// If the user has access to all sub-resources, then instead of resources being a list, this property will be populated `true`.
        /// </summary>
        public bool AccessToAllSubResources { get; set; } = false;
    }
}
