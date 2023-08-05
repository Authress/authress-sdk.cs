using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Authress.SDK.DTO
{

    namespace PermissionedResource {

        /// <summary>
        /// The type of permissions applied to the resource.
        /// </summary>
        /// <value>The type of permissions applied to the resource.</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ActionEnum {
            /// <summary>
            /// Enum CLAIM for value: CLAIM
            /// </summary>
            [EnumMember(Value = "CLAIM")]
            CLAIM = 1,
            /// <summary>
            /// Enum PUBLIC for value: PUBLIC
            /// </summary>
            [EnumMember(Value = "PUBLIC")]
            PUBLIC = 2
        }
    }
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class ResourcePermissionPermissions
    {
        /// <summary>
        /// Gets or Sets Action
        /// </summary>
        [DataMember(Name = "action", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "action")]
        public PermissionedResource.ActionEnum Action { get; set; }

        /// <summary>
        /// Gets or Sets Allow
        /// </summary>
        [DataMember(Name = "allow", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "allow")]
        public bool Allow { get; set; }
    }
}
