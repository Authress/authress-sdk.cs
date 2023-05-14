using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Authress.SDK.DTO
{
    /// <summary>
    /// `TOP_LEVEL_ONLY` - returns only directly nested resources under the resourceUri. A query to `resourceUri=Collection` will return `Collection/resource_1`.
    /// `INCLUDE_NESTED` - will return all sub-resources as well as deeply nested resources that the user has the specified permission to. A query to `resourceUri=Collection` will return `Collection/namespaces/ns/resources/resource_1`.
    ///
    /// To return matching resources for nested resources, set this parameter to `INCLUDE_NESTED`.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CollectionConfigurationEnum
    {
        /// <summary>
        /// Returns only directly nested resources under the resourceUri. A query to `resourceUri=Collection` will return `Collection/resource_1`
        /// </summary>
        TOP_LEVEL_ONLY = 1,
        /// <summary>
        /// will return all sub-resources as well as deeply nested resources that the user has the specified permission to. A query to `resourceUri=Collection` will return `Collection/namespaces/ns/resources/resource_1`
        /// </summary>
        INCLUDE_NESTED = 2
    }
}
