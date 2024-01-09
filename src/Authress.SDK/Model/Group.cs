using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO {

  /// <summary>
  /// A group of users, which can be added to access records.
  /// </summary>
  [DataContract]
  public class Group
  {
        /// <summary>
        /// Unique identifier for the groupId, can be specified on record creation.
        /// </summary>
        /// <value>Unique identifier for the groupId, can be specified on record creation.</value>
        [DataMember(Name="groupId", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "groupId")]
        public string GroupId { get; set; }

        /// <summary>
        /// A helpful name for this record
        /// </summary>
        /// <value>A helpful name for this record</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The expected last time the group was updated
        /// </summary>
        /// <value>The expected last time the group was updated</value>
        [DataMember(Name="lastUpdated", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "lastUpdated")]
        public DateTimeOffset? LastUpdated { get; set; }

        /// <summary>
        /// The list of users in this group
        /// </summary>
        /// <value>The list of users in this group</value>
        [DataMember(Name="users", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "users")]
        public List<AccessRecordUsers> Users { get; set; }

        /// <summary>
        /// The list of admins that can edit this record even if they do not have global record edit permissions.
        /// </summary>
        /// <value>The list of admins that can edit this record even if they do not have global record edit permissions.</value>
        [DataMember(Name="admins", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "admins")]
        public List<AccessRecordUsers> Admins { get; set; }

        /// <summary>
        /// Gets or Sets Links
        /// </summary>
        [DataMember(Name="links", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
    }
}
