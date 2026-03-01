// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotCover
{
    /// <summary>
    /// Contains settings used by <see cref="DotCoverTool{TSettings}" />.
    /// </summary>
    public abstract class DotCoverCoverageSettings : DotCoverSettings
    {
        private readonly HashSet<string> _scope;
        private readonly HashSet<string> _filters;
        private readonly HashSet<string> _processFilters;
        private readonly HashSet<string> _attributeFilters;
        private readonly HashSet<string> _excludeAssemblies;
        private readonly HashSet<string> _excludeAttributes;
        private readonly HashSet<string> _excludeProcesses;

        /// <summary>
        /// Gets or sets program working directory
        /// This represents the <c>--target-working-directory</c> option for Cover command, <c>/TargetWorkingDir</c> for others.
        /// </summary>
        public DirectoryPath TargetWorkingDir { get; set; }

        /// <summary>
        /// Gets the assemblies loaded in the specified scope into coverage results.
        /// Ant-style patterns are supported here (e.g.ProjectFolder/**/*.dll)
        /// This represents the <c>/Scope</c> option.
        /// </summary>
        public ISet<string> Scope
        {
            get { return _scope; }
        }

        /// <summary>
        /// Gets the coverage filters using the following syntax: +:module=*;class=*;function=*;
        /// Use -:myassembly to exclude an assembly from code coverage.
        /// Asterisk wildcard (*) is supported here.
        /// This represents the <c>/Filters</c> option.
        /// </summary>
        public ISet<string> Filters
        {
            get { return _filters; }
        }

        /// <summary>
        /// Gets the attribute filters using the following syntax: filter1;filter2;...
        /// Asterisk wildcard(*) is supported here
        /// This represents the <c>/AttributeFilters</c> option.
        /// </summary>
        public ISet<string> AttributeFilters
        {
            get { return _attributeFilters; }
        }

        /// <summary>
        /// Gets the coverage process filters using the following syntax: +:test.exe;program.exe*;
        /// Use -:anexe to exclude an assembly from code coverage.
        /// This represents the <c>/ProcessFilters</c> option.
        /// </summary>
        public ISet<string> ProcessFilters
        {
            get { return _processFilters; }
        }

        /// <summary>
        /// Gets assembly names to exclude from analysis. Wildcards (*) allowed.
        /// This represents the <c>--exclude-assemblies</c> option.
        /// </summary>
        public ISet<string> ExcludeAssemblies
        {
            get { return _excludeAssemblies; }
        }

        /// <summary>
        /// Gets fully qualified attribute names to exclude from analysis. Wildcards (*) allowed.
        /// Code marked with these attributes will be excluded from coverage.
        /// This represents the <c>--exclude-attributes</c> option.
        /// </summary>
        public ISet<string> ExcludeAttributes
        {
            get { return _excludeAttributes; }
        }

        /// <summary>
        /// Gets process names to ignore during analysis. Wildcards (*) allowed.
        /// This represents the <c>--exclude-processes</c> option.
        /// </summary>
        public ISet<string> ExcludeProcesses
        {
            get { return _excludeProcesses; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the default (automatically added) filters should be disabled
        /// This represents the <c>/DisableDefaultFilters</c> option.
        /// </summary>
        public bool DisableDefaultFilters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotCoverCoverageSettings"/> class.
        /// </summary>
        protected DotCoverCoverageSettings()
        {
            _scope = new HashSet<string>(StringComparer.Ordinal);
            _filters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _attributeFilters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _processFilters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _excludeAssemblies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _excludeAttributes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _excludeProcesses = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }
    }
}