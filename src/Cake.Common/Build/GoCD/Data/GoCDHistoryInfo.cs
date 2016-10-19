// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cake.Common.Build.GoCD.Data
{
    /// <summary>
    /// The Go.CD history.
    /// </summary>
    [DataContract]
    public class GoCDHistoryInfo
    {
        /// <summary>
        /// Gets or sets the pipelines.
        /// </summary>
        /// <value>
        /// The pipelines.
        /// </value>
        [DataMember(Name = "pipelines")]
        public IEnumerable<GoCDPipelineHistoryInfo> Pipelines { get; set; }
    }
}
