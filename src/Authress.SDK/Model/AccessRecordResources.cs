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
    public class AccessRecordResources
    {
        /// <summary>
        /// A resource path which can be top level, fully qualified, or end with a *. Parent resources imply permissions to sub resources.
        /// </summary>
        /// <value>A resource path which can be top level, fully qualified, or end with a *. Parent resources imply permissions to sub resources.</value>
        [DataMember(Name = "resourceUri", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "resourceUri")]
        public string ResourceUri { get; set; }
    }
}
