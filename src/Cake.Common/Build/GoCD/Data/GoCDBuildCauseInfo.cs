// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cake.Common.Build.GoCD.Data
{
    /// <summary>
    /// The Go.CD build cause.
    /// </summary>
    [DataContract]
    public class GoCDBuildCauseInfo
    {
        /// <summary>
        /// Gets or sets the approver.
        /// </summary>
        /// <value>
        /// The approver.
        /// </value>
        [DataMember(Name = "approver")]
        public string Approver { get; set; }

        /// <summary>
        /// Gets or sets the material revisions.
        /// </summary>
        /// <value>
        /// The material revisions.
        /// </value>
        [DataMember(Name = "material_revisions")]
        public IEnumerable<GoCDMaterialRevisionsInfo> MaterialRevisions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the trigger was forced.
        /// </summary>
        /// <value>
        /// <c>true</c> if the trigger was forced; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "trigger_forced")]
        public bool TriggerForced { get; set; }

        /// <summary>
        /// Gets or sets the trigger message.
        /// </summary>
        /// <value>
        /// The trigger message.
        /// </value>
        [DataMember(Name = "trigger_message")]
        public string TriggerMessage { get; set; }
    }
}
