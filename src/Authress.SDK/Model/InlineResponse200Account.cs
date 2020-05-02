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
    public class InlineResponse200Account
    {
        /// <summary>
        /// Gets or Sets AccountId
        /// </summary>
        [DataMember(Name = "accountId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "accountId")]
        public string AccountId { get; set; }

    }
}
