using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Authress.SDK.DTO
{

    /// <summary>
    /// The collective action and associate grants on a permission
    /// </summary>
    [DataContract]
    public class PermissionObject
    {
        /// <summary>
        /// The action the permission grants, can be scoped using `:` and parent actions imply child permissions, action:* or action implies action:sub-action.
        /// </summary>
        /// <value>The action the permission grants, can be scoped using `:` and parent actions imply child permissions, action:* or action implies action:sub-action.</value>
        [DataMember(Name = "action", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }

        /// <summary>
        /// Does this permission grant the user the ability to execute the action?
        /// </summary>
        /// <value>Does this permission grant the user the ability to execute the action?</value>
        [DataMember(Name = "allow", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "allow")]
        public bool? Allow { get; set; }

        /// <summary>
        /// Allows the user to give the permission to others without being able to execute the action.
        /// </summary>
        /// <value>Allows the user to give the permission to others without being able to execute the action.</value>
        [DataMember(Name = "grant", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "grant")]
        public bool? Grant { get; set; }

        /// <summary>
        /// Allows delegating or granting the permission to others without being able to execute tha action.
        /// </summary>
        /// <value>Allows delegating or granting the permission to others without being able to execute tha action.</value>
        [DataMember(Name = "delegate", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "delegate")]
        public bool? Delegate { get; set; }
    }
}
