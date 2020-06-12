using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{
    /// <summary>
    /// Interface contract which provides links for collection DTO so that they can be paginated.
    /// </summary>
    public interface IPaginationDto
    {
        /// <summary>
        /// Collection Links which contains the next link.
        /// </summary>
        Links Links { get; }
    }
}
