using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Authress.SDK.Client.JWT
{
    [DataContract]
    internal class JwtPayload
    {

        [DataMember(Name = "sub")]
        internal string Subject { get; set; }

        [DataMember(Name = "iss")]
        internal string Issuer { get; set; }

        [DataMember(Name = "exp")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        internal DateTimeOffset Expires { get; set; }
    }

    [DataContract]
    internal class JwtHeader
    {

        [DataMember(Name = "kid")]
        internal string KeyId { get; set; }
    }
}
