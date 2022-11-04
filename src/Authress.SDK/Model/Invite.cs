using System.Collections.Generic;
using System.Runtime.Serialization;

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
        /// A list of statements which match roles to resources. The invited user will all statements apply to them
        /// </summary>
        /// <value>A list of statements which match roles to resources. The invited user will all statements apply to them</value>
        [DataMember(Name="statements", EmitDefaultValue=false)]
        public List<AccessRecordStatements> Statements { get; set; }

        /// <summary>
        /// Gets or Sets Links
        /// </summary>
        [DataMember(Name="links", EmitDefaultValue=false)]
        public Links Links { get; set; }
    }
}
