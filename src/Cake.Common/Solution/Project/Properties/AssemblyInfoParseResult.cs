// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace Cake.Common.Solution.Project.Properties
{
    /// <summary>
    /// Represents the content in an assembly info file.
    /// </summary>
    public sealed class AssemblyInfoParseResult
    {
        private readonly List<string> _internalsVisibleTo;

        /// <summary>
        /// Gets a value indicating whether the assembly is CLS compliant.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the assembly is CLS compliant; otherwise, <c>false</c>.
        /// </value>
        public bool ClsCompliant { get; }

        /// <summary>
        /// Gets the assembly company attribute.
        /// </summary>
        /// <value>The assembly company attribute.</value>
        public string Company { get; }

        /// <summary>
        /// Gets a value indicating whether the assembly is accessible from COM.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the assembly is accessible from COM; otherwise, <c>false</c>.
        /// </value>
        public bool ComVisible { get; }

        /// <summary>
        /// Gets the assembly configuration attribute.
        /// </summary>
        /// <value>The assembly Configuration attribute.</value>
        public string Configuration { get; }

        /// <summary>
        /// Gets the assembly copyright attribute.
        /// </summary>
        /// <value>The assembly copyright attribute.</value>
        public string Copyright { get; }

        /// <summary>
        /// Gets the assembly's description attribute.
        /// </summary>
        /// <value>The assembly's Description attribute.</value>
        public string Description { get; }

        /// <summary>
        /// Gets the assembly file version.
        /// </summary>
        /// <value>The assembly file version.</value>
        public string AssemblyFileVersion { get; }

        /// <summary>
        /// Gets the assembly GUID attribute.
        /// </summary>
        /// <value>The assembly GUID attribute.</value>
        public string Guid { get; }

        /// <summary>
        /// Gets the assembly informational version.
        /// </summary>
        /// <value>The assembly informational version.</value>
        public string AssemblyInformationalVersion { get; }

        /// <summary>
        /// Gets the assembly product Attribute.
        /// </summary>
        /// <value>The assembly product attribute.</value>
        public string Product { get; }

        /// <summary>
        /// Gets the assembly title Attribute.
        /// </summary>
        /// <value>The assembly Title attribute.</value>
        public string Title { get; }

        /// <summary>
        /// Gets the assembly trademark Attribute.
        /// </summary>
        /// <value>The assembly Trademark attribute.</value>
        public string Trademark { get; }

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        /// <value>The assembly version.</value>
        public string AssemblyVersion { get; }

        /// <summary>
        /// Gets the assemblies that internals are visible to.
        /// </summary>
        /// <value>The assemblies that internals are visible to.</value>
        public ICollection<string> InternalsVisibleTo => _internalsVisibleTo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfoParseResult"/> class.
        /// </summary>
        /// <param name="clsCompliant">Whether the assembly is CLS compliant.</param>
        /// <param name="company">The assembly company attribute.</param>
        /// <param name="comVisible">Whether the assembly is accessible from COM.</param>
        /// <param name="configuration">The assembly configuration attribute.</param>
        /// <param name="copyright">The assembly copyright attribute.</param>
        /// <param name="description">The assembly description attribute.</param>
        /// <param name="assemblyFileVersion">The assembly file version.</param>
        /// <param name="guid">The assembly GUID attribute.</param>
        /// <param name="assemblyInformationalVersion">The assembly informational version.</param>
        /// <param name="product">The assembly product attribute.</param>
        /// <param name="title">The assembly title attribute.</param>
        /// <param name="trademark">The assembly trademark attribute.</param>
        /// <param name="assemblyVersion">The assembly version.</param>
        /// <param name="internalsVisibleTo">The assemblies that internals are visible to.</param>
        public AssemblyInfoParseResult(string clsCompliant,
            string company,
            string comVisible,
            string configuration,
            string copyright,
            string description,
            string assemblyFileVersion,
            string guid,
            string assemblyInformationalVersion,
            string product,
            string title,
            string trademark,
            string assemblyVersion,
            IEnumerable<string> internalsVisibleTo)
        {
            ClsCompliant = !string.IsNullOrWhiteSpace(clsCompliant) && bool.Parse(clsCompliant);
            Company = company ?? string.Empty;
            ComVisible = !string.IsNullOrWhiteSpace(comVisible) && bool.Parse(comVisible);
            Configuration = configuration ?? string.Empty;
            Copyright = copyright ?? string.Empty;
            Description = description ?? string.Empty;
            AssemblyFileVersion = assemblyFileVersion ?? string.Empty;
            Guid = guid ?? string.Empty;
            AssemblyInformationalVersion = assemblyInformationalVersion ?? string.Empty;
            Product = product ?? string.Empty;
            Title = title ?? string.Empty;
            Trademark = trademark ?? string.Empty;
            AssemblyVersion = assemblyVersion ?? string.Empty;
            _internalsVisibleTo = new List<string>(internalsVisibleTo ?? Enumerable.Empty<string>());
        }
    }
}