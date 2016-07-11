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
        private readonly bool _clsCompliant;
        private readonly string _company;
        private readonly bool _comVisible;
        private readonly string _configuration;
        private readonly string _copyright;
        private readonly string _description;
        private readonly string _assemblyFileVersion;
        private readonly string _guid;
        private readonly string _assemblyInformationalVersion;
        private readonly string _product;
        private readonly string _title;
        private readonly string _trademark;
        private readonly string _assemblyVersion;
        private readonly List<string> _internalsVisibleTo;

        /// <summary>
        /// Gets a value indicating whether the assembly is CLS compliant.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the assembly is CLS compliant; otherwise, <c>false</c>.
        /// </value>
        public bool ClsCompliant
        {
            get { return _clsCompliant; }
        }

        /// <summary>
        /// Gets the assembly company attribute.
        /// </summary>
        /// <value>The assembly company attribute.</value>
        public string Company
        {
            get { return _company; }
        }

        /// <summary>
        /// Gets a value indicating whether the assembly is accessible from COM.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the assembly is accessible from COM; otherwise, <c>false</c>.
        /// </value>
        public bool ComVisible
        {
            get { return _comVisible; }
        }

        /// <summary>
        /// Gets the assembly configuration attribute.
        /// </summary>
        /// <value>The assembly Configuration attribute.</value>
        public string Configuration
        {
            get { return _configuration; }
        }

        /// <summary>
        /// Gets the assembly copyright attribute.
        /// </summary>
        /// <value>The assembly copyright attribute.</value>
        public string Copyright
        {
            get { return _copyright; }
        }

        /// <summary>
        /// Gets the assembly's description attribute.
        /// </summary>
        /// <value>The assembly's Description attribute.</value>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Gets the assembly file version.
        /// </summary>
        /// <value>The assembly file version.</value>
        public string AssemblyFileVersion
        {
            get { return _assemblyFileVersion; }
        }

        /// <summary>
        /// Gets the assembly GUID attribute.
        /// </summary>
        /// <value>The assembly GUID attribute.</value>
        public string Guid
        {
            get { return _guid; }
        }

        /// <summary>
        /// Gets the assembly informational version.
        /// </summary>
        /// <value>The assembly informational version.</value>
        public string AssemblyInformationalVersion
        {
            get { return _assemblyInformationalVersion; }
        }

        /// <summary>
        /// Gets the assembly product Attribute.
        /// </summary>
        /// <value>The assembly product attribute.</value>
        public string Product
        {
            get { return _product; }
        }

        /// <summary>
        /// Gets the assembly title Attribute.
        /// </summary>
        /// <value>The assembly Title attribute.</value>
        public string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Gets the assembly trademark Attribute.
        /// </summary>
        /// <value>The assembly Trademark attribute.</value>
        public string Trademark
        {
            get { return _trademark; }
        }

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        /// <value>The assembly version.</value>
        public string AssemblyVersion
        {
            get { return _assemblyVersion; }
        }

        /// <summary>
        /// Gets the assemblies that internals are visible to.
        /// </summary>
        /// <value>The assemblies that internals are visible to.</value>
        public ICollection<string> InternalsVisibleTo
        {
            get { return _internalsVisibleTo; }
        }

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
            _clsCompliant = !string.IsNullOrWhiteSpace(clsCompliant) && bool.Parse(clsCompliant);
            _company = company ?? string.Empty;
            _comVisible = !string.IsNullOrWhiteSpace(comVisible) && bool.Parse(comVisible);
            _configuration = configuration ?? string.Empty;
            _copyright = copyright ?? string.Empty;
            _description = description ?? string.Empty;
            _assemblyFileVersion = assemblyFileVersion ?? string.Empty;
            _guid = guid ?? string.Empty;
            _assemblyInformationalVersion = assemblyInformationalVersion ?? string.Empty;
            _product = product ?? string.Empty;
            _title = title ?? string.Empty;
            _trademark = trademark ?? string.Empty;
            _assemblyVersion = assemblyVersion ?? string.Empty;
            _internalsVisibleTo = new List<string>(internalsVisibleTo ?? Enumerable.Empty<string>());
        }
    }
}
