using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    /// A client configuration.
    /// </summary>
    [DataContract]
    public class ClientAccessKey
    {
        /// <summary>
        /// The unique id of the client.
        /// </summary>
        /// <value>The unique id of the client.</value>
        [DataMember(Name = "keyId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "keyId")]
        public string KeyId { get; set; }

        /// <summary>
        /// The unique id of the client.
        /// </summary>
        /// <value>The unique id of the client.</value>
        [DataMember(Name = "clientId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or Sets GenerationDate
        /// </summary>
        [DataMember(Name = "generationDate", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "generationDate")]
        public DateTime? GenerationDate { get; set; }

        /// <summary>
        /// An encoded access key which contains identifying information for client access token creation. For direct use with the Authress SDKs, not meant to be decoded. Must be saved on creted, as it will never be returned from the API ever again. Authress only saved the corresponding public key to verify private access keys.
        /// </summary>
        /// <value>An encoded access key which contains identifying information for client access token creation. For direct use with the Authress SDKs, not meant to be decoded. Must be saved on creted, as it will never be returned from the API ever again. Authress only saved the corresponding public key to verify private access keys.</value>
        [DataMember(Name = "accessKey", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "accessKey")]
        public string AccessKey { get; set; }
    }
}
