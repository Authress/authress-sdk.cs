using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Authress.SDK.DTO
{
    /// <summary>
    /// A logical grouping of access related elements
    /// </summary>
    [DataContract]
    public class AccessTemplate
    {
        /// <summary>
        /// The list of users the access applies to
        /// </summary>
        /// <value>The list of users the access applies to</value>
        [DataMember(Name="users", EmitDefaultValue=false)]
        public List<AccessRecordUser> Users { get; set; }

        /// <summary>
        /// A list of statements which match roles to resources. Users here will have all statements apply to them
        /// </summary>
        /// <value>A list of statements which match roles to resources. Users here will have all statements apply to them</value>
        [DataMember(Name="statements", EmitDefaultValue=false)]
        public List<AccessRecordStatement> Statements { get; set; }
    }
}
