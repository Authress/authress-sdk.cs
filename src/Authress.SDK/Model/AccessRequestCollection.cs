using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    /// A collection of access requests
    /// </summary>
    [DataContract]
    public class AccessRequestCollection : IPaginationDto
    {
        /// <summary>
        /// A list of clients
        /// </summary>
        /// <value>A list of clients</value>
        [DataMember(Name = "records", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "records")]
        public List<AccessRequest> Records { get; set; } = new List<AccessRequest>{};

        /// <summary>
        /// The Links associated with this collection
        /// </summary>
        [DataMember(Name = "links", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
    }
}
