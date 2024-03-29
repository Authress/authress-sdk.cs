using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    /// The collection of a list of access records
    /// </summary>
    [DataContract]
    public class AccessRecordCollection : IPaginationDto
    {
        /// <summary>
        /// A list of clients
        /// </summary>
        /// <value>A list of clients</value>
        [DataMember(Name = "records", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "records")]
        public List<AccessRecord> Records { get; set; } = new List<AccessRecord>{};

        /// <summary>
        /// The Links associated with this collection
        /// </summary>
        [DataMember(Name = "links", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
    }
}
