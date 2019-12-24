// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Solution.Project.Properties
{
    /// <summary>
    /// Custom Attribute class used by <see cref="AssemblyInfoSettings"/>.
    /// </summary>
    public sealed class AssemblyInfoCustomAttribute
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The attribute name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace for the attribute.</value>
        public string NameSpace { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value for the attribute.</value>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value is raw or should be quoted in the created attribute.
        /// </summary>
        /// <value>
        ///   <c>true</c> if should be treated as raw; otherwise, <c>false</c>.
        /// </value>
        public bool UseRawValue { get; set; }
    }
}