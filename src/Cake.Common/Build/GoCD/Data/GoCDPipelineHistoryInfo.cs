// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.Serialization;

namespace Cake.Common.Build.GoCD.Data
{
    /// <summary>
    /// The Go.CD pipeline history.
    /// </summary>
    [DataContract]
    public class GoCDPipelineHistoryInfo
    {
        /// <summary>
        /// Gets or sets the build cause.
        /// </summary>
        /// <value>
        /// The build cause.
        /// </value>
        [DataMember(Name = "build_cause")]
        public GoCDBuildCauseInfo BuildCause { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the natural order.
        /// </summary>
        /// <value>
        /// The natural order.
        /// </value>
        [DataMember(Name = "natural_order")]
        public string NaturalOrder { get; set; }
    }
}
