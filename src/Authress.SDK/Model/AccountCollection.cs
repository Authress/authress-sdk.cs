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
    public class AccountCollection
    {
        /// <summary>
        /// Gets or Sets Accounts
        /// </summary>
        [DataMember(Name = "accounts", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "accounts")]
        public Account Accounts { get; set; }
    }
}
