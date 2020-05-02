using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class IdentityCollection
    {
        /// <summary>
        /// Gets or Sets Identities
        /// </summary>
        [DataMember(Name = "identities", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "identities")]
        public List<Identity> Identities { get; set; }
    }
}
