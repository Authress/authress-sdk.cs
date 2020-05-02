using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

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
        public string Action { get; set; }

        /// <summary>
        /// Gets or Sets Allow
        /// </summary>
        [DataMember(Name = "allow", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "allow")]
        public string Allow { get; set; }
    }
}
