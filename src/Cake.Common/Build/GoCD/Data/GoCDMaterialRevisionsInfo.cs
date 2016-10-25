// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cake.Common.Build.GoCD.Data
{
    /// <summary>
    /// The Go.CD material revision information.
    /// </summary>
    [DataContract]
    public class GoCDMaterialRevisionsInfo
    {
        /// <summary>
        /// Gets or sets a value indicating whether a change was made.
        /// </summary>
        /// <value>
        /// <c>true</c> if changed; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "changed")]
        public bool Changed { get; set; }

        /// <summary>
        /// Gets or sets the modifications.
        /// </summary>
        /// <value>
        /// The modifications.
        /// </value>
        [DataMember(Name = "modifications")]
        public IEnumerable<GoCDModificationInfo> Modifications { get; set; }
    }
}
