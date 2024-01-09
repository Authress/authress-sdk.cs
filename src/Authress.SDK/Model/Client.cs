using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    /// A client configuration.
    /// </summary>
    [DataContract]
    public class ServiceClient
    {
        /// <summary>
        /// The unique id of the client.
        /// </summary>
        /// <value>The unique id of the client.</value>
        [DataMember(Name = "clientId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or Sets CreatedTime
        /// </summary>
        [DataMember(Name = "createdTime", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "createdTime")]
        public DateTimeOffset? CreatedTime { get; set; }

        /// <summary>
        /// The name of the client
        /// </summary>
        /// <value>The name of the client</value>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
