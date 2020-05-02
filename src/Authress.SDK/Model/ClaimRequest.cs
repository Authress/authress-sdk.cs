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
    public class ClaimRequest
    {
        /// <summary>
        /// The parent resource to add a child resource to. The resource must have a resource configuration that add the permission CLAIM for this to work.
        /// </summary>
        /// <value>The parent resource to add a child resource to. The resource must have a resource configuration that add the permission CLAIM for this to work.</value>
        [DataMember(Name = "collectionResource", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "collectionResource")]
        public string CollectionResource { get; set; }

        /// <summary>
        /// The child resource the user is requesting Admin ownership over.
        /// </summary>
        /// <value>The child resource the user is requesting Admin ownership over.</value>
        [DataMember(Name = "resourceId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "resourceId")]
        public string ResourceId { get; set; }
    }
}
