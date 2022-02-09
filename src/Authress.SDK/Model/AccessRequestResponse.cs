using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Authress.SDK.DTO
{
    /// <summary>
    /// The response to an access request.
    /// </summary>
    [DataContract]
    public class AccessRequestResponse
    {
        /// <summary>
        /// New result, either approve or deny the request
        /// </summary>
        /// <value>New result, either approve or deny the request</value>
        [JsonConverter(typeof(StringEnumConverter))]
                public enum StatusEnum
        {
            /// <summary>
            /// Enum APPROVED for value: APPROVED
            /// </summary>
            [EnumMember(Value = "APPROVED")]
            APPROVED = 1,
            /// <summary>
            /// Enum DENIED for value: DENIED
            /// </summary>
            [EnumMember(Value = "DENIED")]
            DENIED = 2
        }

        /// <summary>
        /// New result, either approve or deny the request
        /// </summary>
        /// <value>New result, either approve or deny the request</value>
        [DataMember(Name="status", EmitDefaultValue=false)]
        public StatusEnum Status { get; set; }
    }
}
