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
        [DataMember(Name = "records", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "records")]
        public List<AccessRecord> Records { get; set; }
    }
}
