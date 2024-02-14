using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{
    /// <summary>
    /// The user invite used to invite users to your application or to Authress as an admin.
    /// </summary>
    [DataContract]
    public class Invite
    {
        /// <summary>
        /// The unique identifier for the invite. Use this ID to accept the invite.
        /// </summary>
        /// <value>The unique identifier for the invite. Use this ID to accept the invite.</value>
        [DataMember(Name="inviteId", EmitDefaultValue=false)]
        public string InviteId { get; private set; }

        /// <summary>
        /// Specify the tenant associated with the invite. The invited user must use this tenant's connection configuration to log in.
        /// </summary>
        /// <value>Specify the tenant associated with the invite. The invited user must use this tenant's connection configuration to log in.</value>
        [DataMember(Name="tenantId", EmitDefaultValue=false)]
        public string TenantId { get; private set; }

        /// <summary>
        /// A list of statements which match roles to resources. The invited user will all statements apply to them
        /// </summary>
        /// <value>A list of statements which match roles to resources. The invited user will all statements apply to them</value>
        [DataMember(Name="statements", EmitDefaultValue=false)]
        public List<InviteStatement> Statements { get; set; }

        /// <summary>
        /// Gets or Sets Links
        /// </summary>
        [DataMember(Name="links", EmitDefaultValue=false)]
        public Links Links { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class InviteStatement
    {
        /// <summary>
        /// Gets or Sets Roles
        /// </summary>
        [DataMember(Name = "roles", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "roles")]
        public List<string> Roles { get; set; }

        /// <summary>
        /// Gets or Sets Resources
        /// </summary>
        [DataMember(Name = "resources", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "resources")]
        public List<AccessRecordResource> Resources { get; set; }
    }
}
