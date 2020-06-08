using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    /// The collection of a list of clients
    /// </summary>
    [DataContract]
    public class ClientCollection : IPaginationDto
    {
        /// <summary>
        /// A list of clients
        /// </summary>
        /// <value>A list of clients</value>
        [DataMember(Name = "clients", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "clients")]
        public List<ServiceClient> Clients { get; set; } = new List<ServiceClient>();

        /// <summary>
        /// The Links associated with this collection
        /// </summary>
        [DataMember(Name = "links", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
    }
}
