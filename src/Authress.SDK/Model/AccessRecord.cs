using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    /// The access record which links users to roles.
    /// </summary>
    [DataContract]
    public class AccessRecord
    {
        /// <summary>
        /// Gets or Sets RecordId
        /// </summary>
        [DataMember(Name = "recordId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "recordId")]
        public string RecordId { get; set; }

        /// <summary>
        /// A helpful name for this record
        /// </summary>
        /// <value>A helpful name for this record</value>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Account
        /// </summary>
        [DataMember(Name = "account", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "account")]
        public AccessRecordAccount Account { get; set; }

        /// <summary>
        /// The list of users this record applies to
        /// </summary>
        /// <value>The list of users this record applies to</value>
        [DataMember(Name = "users", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "users")]
        public List<AccessRecordUsers> Users { get; set; }

        /// <summary>
        /// The list of groups this record applies to. Users in these groups will be receive access to the resources listed.
        /// </summary>
        /// <value>The list of groups this record applies to. Users in these groups will be receive access to the resources listed.</value>
        [DataMember(Name = "groups", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "groups")]
        public List<AccessRecordGroup> Groups { get; set; } = new List<AccessRecordGroup>();

        /// <summary>
        /// The list of admin that can edit this record even if they do not have global record edit permissions.
        /// </summary>
        /// <value>The list of admin that can edit this record even if they do not have global record edit permissions.</value>
        [DataMember(Name = "admins", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "admins")]
        public List<AccessRecordUsers> Admins { get; set; }

        /// <summary>
        /// A list of statements which match roles to resources. Users in this record have all statements apply to them
        /// </summary>
        /// <value>A list of statements which match roles to resources. Users in this record have all statements apply to them</value>
        [DataMember(Name = "statements", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "statements")]
        public List<AccessRecordStatements> Statements { get; set; }

        /// <summary>
        /// Gets or Sets Links
        /// </summary>
        [DataMember(Name = "links", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
    }
}
