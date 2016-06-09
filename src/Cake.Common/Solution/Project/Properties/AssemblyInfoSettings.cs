// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;

namespace Cake.Common.Solution.Project.Properties
{
    /// <summary>
    /// Contains settings used by <see cref="AssemblyInfoCreator"/>.
    /// </summary>
    public sealed class AssemblyInfoSettings
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The assembly title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The assembly description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public string Guid { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>The assembly product.</value>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        public string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the trademark.
        /// </summary>
        /// <value>The trademark.</value>
        public string Trademark { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the file version.
        /// </summary>
        /// <value>The file version.</value>
        public string FileVersion { get; set; }

        /// <summary>
        /// Gets or sets the informational version.
        /// </summary>
        /// <value>The informational version.</value>
        public string InformationalVersion { get; set; }

        /// <summary>
        /// Gets or sets whether or not the assembly is COM visible.
        /// </summary>
        /// <value>Whether or not the assembly is COM visible.</value>
        public bool? ComVisible { get; set; }

        /// <summary>
        /// Gets or sets whether or not the assembly is CLS compliant.
        /// </summary>
        /// <value>Whether or not the assembly is CLS compliant.</value>
        public bool? CLSCompliant { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the name(s) of the assembly(s) that internals should be visible to.
        /// </summary>
        /// <value>The name(s) of the assembly(s).</value>
        public ICollection<string> InternalsVisibleTo { get; set; }

        /// <summary>
        /// Gets or sets the configuration of the assembly.
        /// </summary>
        /// <value>The configuration.</value>
        public string Configuration { get; set; }
    }
}
