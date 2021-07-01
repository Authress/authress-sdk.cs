using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class AccessRecordGroup
    {
        /// <summary>
        /// Gets or Sets GroupId
        /// </summary>
        [DataMember(Name = "groupId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "groupId")]
        public string GroupId { get; set; }
    }
}
