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
    public class AccessRecordCollection
    {
        /// <summary>
        /// A list of clients
        /// </summary>
        /// <value>A list of clients</value>
        [DataMember(Name = "clients", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "clients")]
        public List<ServiceClient> Clients { get; set; }
    }
}
