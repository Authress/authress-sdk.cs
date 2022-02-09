using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Authress.SDK.DTO
{
    /// <summary>
    /// The access requested by a user.
    /// </summary>
    [DataContract]
    public class AccessRequest
    {
        /// <summary>
        /// Current status of the access request.
        /// </summary>
        /// <value>Current status of the access request.</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum StatusEnum
        {
            /// <summary>
            /// Enum OPEN for value: OPEN
            /// </summary>
            [EnumMember(Value = "OPEN")]
            OPEN = 1,
            /// <summary>
            /// Enum APPROVED for value: APPROVED
            /// </summary>
            [EnumMember(Value = "APPROVED")]
            APPROVED = 2,
            /// <summary>
            /// Enum DENIED for value: DENIED
            /// </summary>
            [EnumMember(Value = "DENIED")]
            DENIED = 3,
            /// <summary>
            /// Enum DELETED for value: DELETED
            /// </summary>
            [EnumMember(Value = "DELETED")]
            DELETED = 4
        }

        /// <summary>
        /// Current status of the access request.
        /// </summary>
        /// <value>Current status of the access request.</value>
        [DataMember(Name="status", EmitDefaultValue=false)]
        public StatusEnum? Status { get; set; }

        /// <summary>
        /// Unique identifier for the request.
        /// </summary>
        /// <value>Unique identifier for the request.</value>
        [DataMember(Name="requestId", EmitDefaultValue=false)]
        public string RequestId { get; private set; }

        /// <summary>
        /// The expected last time the request was updated
        /// </summary>
        /// <value>The expected last time the request was updated</value>
        [DataMember(Name="lastUpdated", EmitDefaultValue=false)]
        public DateTime? LastUpdated { get; private set; }


        /// <summary>
        /// Gets or Sets Access
        /// </summary>
        [DataMember(Name="access", EmitDefaultValue=false)]
        public AccessTemplate Access { get; set; }

        /// <summary>
        /// Gets or Sets Links
        /// </summary>
        [DataMember(Name = "links", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
    }
}
