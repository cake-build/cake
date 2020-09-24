// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Solution.Project.Properties
{
    /// <summary>
    /// Metadata Attribute class used by <see cref="AssemblyInfoSettings"/>.
    /// </summary>
    public sealed class AssemblyInfoMetadataAttribute
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The attribute name.</value>
        public string Name { get; } = "AssemblyMetadata";

        /// <summary>
        /// Gets or sets the key for meta data.
        /// </summary>
        /// <value>The key for meta data.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <value>The namespace for the meta data attribute.</value>
        public string NameSpace { get; } = "System.Reflection";

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value for the meta data.</value>
        public string Value { get; set; }
    }
}