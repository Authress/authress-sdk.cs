using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{
    /// <summary>
    /// A collection of groups
    /// </summary>
    [DataContract]
    public class GroupCollection : IPaginationDto
    {
        /// <summary>
        /// A list of groups
        /// </summary>
        /// <value>A list of groups</value>
        [DataMember(Name="groups", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "groups")]
        public List<Group> Groups { get; set; }

        /// <summary>
        /// The Links associated with this collection
        /// </summary>
        [DataMember(Name = "links", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
    }
}
