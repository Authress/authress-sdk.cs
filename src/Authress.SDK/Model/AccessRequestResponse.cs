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
        [DataMember(Name="status", EmitDefaultValue=false)]
        public AccessRequest.StatusEnum Status { get; set; }
    }
}
