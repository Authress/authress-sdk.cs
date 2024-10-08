using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class AccessRecordStatement
    {
        /// <summary>
        /// Gets or Sets Roles
        /// </summary>
        [DataMember(Name = "roles", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "roles")]
        public List<string> Roles { get; set; }

        /// <summary>
        /// Gets or Sets Resources
        /// </summary>
        [DataMember(Name = "resources", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "resources")]
        public List<AccessRecordResource> Resources { get; set; }

        /// <summary>
        /// The list of users this record statement applies to
        /// </summary>
        /// <value>The list of users this record statement applies to</value>
        [DataMember(Name = "users", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "users")]
        public List<AccessRecordUser> Users { get; set; }

        /// <summary>
        /// The list of groups this record statement applies to. Users in these groups will be receive access to the resources listed.
        /// </summary>
        /// <value>The list of groups this record statement applies to. Users in these groups will be receive access to the resources listed.</value>
        [DataMember(Name = "groups", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "groups")]
        public List<AccessRecordGroup> Groups { get; set; } = new List<AccessRecordGroup>();
    }
}
