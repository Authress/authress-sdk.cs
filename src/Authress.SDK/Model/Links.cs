using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{
    /// <summary>
    /// The Link collection of an application/links+json response object
    /// </summary>
    [DataContract]
    public class Links
    {
        /// <summary>
        /// The self link with references with the requested resource
        /// </summary>
        [DataMember(Name = "self", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "self")]
        public Link Self { get; set; }

        /// <summary>
        /// The next link for a paginated response which contains an absolute url to the next page in the collection.
        /// </summary>
        [DataMember(Name = "next", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "next")]
        public Link Next { get; set; }
    }

    /// <summary>
    /// The Link object which contains the href absolute url and an optional link relation
    /// </summary>
    [DataContract]
    public class Link
    {
        /// <summary>
        /// The url link to the linked resource.
        /// </summary>
        [DataMember(Name = "href", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "href")]
        public string href { get; set; }

        /// <summary>
        /// The optional link relation only specified if the link relation is not an IANA prespecified type.
        /// </summary>
        [DataMember(Name = "rel", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "rel")]
        public string rel { get; set; }
    }
}
