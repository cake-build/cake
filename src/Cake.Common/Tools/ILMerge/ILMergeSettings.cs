// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Tooling;

namespace Cake.Common.Tools.ILMerge
{
    /// <summary>
    /// Contains settings used by <see cref="ILMergeRunner"/>.
    /// </summary>
    public sealed class ILMergeSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether whether types in assemblies other
        /// than the primary assembly should have their visibility modified to internal.
        /// </summary>
        /// <value>
        /// <c>true</c> if types in assemblies other than the primary assembly should
        /// have their visibility modified to internal; otherwise, <c>false</c>.
        /// </value>
        public bool Internalize { get; set; }

        /// <summary>
        /// Gets or sets the target kind.
        /// </summary>
        /// <value>The target kind.</value>
        public TargetKind TargetKind { get; set; }

        /// <summary>
        /// Gets or sets the target platform.
        /// </summary>
        /// <value>The target platform.</value>
        public TargetPlatform TargetPlatform { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ILMergeSettings"/> class.
        /// </summary>
        public ILMergeSettings()
        {
            Internalize = false;
            TargetKind = TargetKind.Default;
        }
    }
}
