// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Solution.Project
{
    /// <summary>
    /// Represents a project assembly reference.
    /// </summary>
    /// <remarks>
    /// Schema from https://msdn.microsoft.com/en-us/library/ms164283.aspx
    /// and https://msdn.microsoft.com/en-us/library/bb629388.aspx.
    /// </remarks>
    public sealed class ProjectAssemblyReference
    {
        /// <summary>
        /// Gets or sets the reference to include.
        /// </summary>
        /// <value>
        /// The reference to include.
        /// </value>
        public string Include { get; set; }

        /// <summary>
        /// Gets or sets the relative or absolute path of the assembly.
        /// </summary>
        /// <value>
        /// The relative or absolute path of the assembly.
        /// </value>
        public FilePath HintPath { get; set; }

        /// <summary>
        /// Gets or sets the display name of the assembly.
        /// </summary>
        /// <value>
        /// The display name of the assembly.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the simple or strong fusion name for the item.
        /// </summary>
        /// <value>
        /// The simple or strong fusion name for the item.
        /// </value>
        public string FusionName { get; set; }

        /// <summary>
        /// Gets or sets whether only the version in the fusion name should be referenced.
        /// </summary>
        /// <value>
        /// Whether only the version in the fusion name should be referenced.
        /// </value>
        public bool? SpecificVersion { get; set; }

        /// <summary>
        /// Gets or sets any aliases for the reference.
        /// </summary>
        /// <value>
        /// Any aliases for the reference.
        /// </value>
        public string Aliases { get; set; }

        /// <summary>
        /// Gets or sets whether the reference should be copied to the output folder.
        /// </summary>
        /// <value>
        /// Whether the reference should be copied to the output folder.
        /// </value>
        public bool? Private { get; set; }
    }
}