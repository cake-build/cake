// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Solution.Project
{
    /// <summary>
    /// Represents a project reference to another project.
    /// </summary>
    /// <remarks>
    /// Schema from https://msdn.microsoft.com/en-us/library/ms164283.aspx
    /// and https://msdn.microsoft.com/en-us/library/bb629388.aspx.
    /// </remarks>
    public sealed class ProjectReference
    {
        /// <summary>
        /// Gets or sets the path to the referenced project file.
        /// </summary>
        /// <value>
        /// The path to the referenced project file.
        /// </value>
        public FilePath FilePath { get; set; }

        /// <summary>
        /// Gets or sets the relative path to the referenced project file.
        /// </summary>
        /// <value>
        /// The relative path to the referenced project file.
        /// </value>
        public string RelativePath { get; set; }

        /// <summary>
        /// Gets or sets the display name of the reference.
        /// </summary>
        /// <value>
        /// The display name of the reference.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a GUID for the reference.
        /// </summary>
        /// <value>
        /// A GUID for the reference.
        /// </value>
        public string Project { get; set; }

        /// <summary>
        /// Gets or sets the path of the project file that is being referenced.
        /// </summary>
        /// <value>
        /// The path of the project file that is being referenced.
        /// </value>
        public FilePath Package { get; set; }

        /// <summary>
        /// Gets or sets whether the reference should be copied to the output folder.
        /// </summary>
        /// <value>
        /// Whether the reference should be copied to the output folder.
        /// </value>
        public bool? Private { get; set; }
    }
}