using System.Runtime.Serialization;

namespace Authress.SDK.Client.JWT
{
    [DataContract]
    internal class JwtPayload
    {

        [DataMember(Name = "sub")]
        internal string Subject { get; set; }

        [DataMember(Name = "iss")]
        internal string Issuer { get; set; }
    }

    [DataContract]
    internal class JwtHeader
    {

        [DataMember(Name = "kid")]
        internal string KeyId { get; set; }
    }
}
