using System;
using System.Collections.Generic;
using Cake.Common.Tools.DotCover;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotCover.Analyse
{
    /// <summary>
    /// Contains settings used by <see cref="DotCoverAnalyser" />.
    /// </summary>
    public sealed class DotCoverAnalyseSettings : ToolSettings
    {
        private readonly HashSet<string> _scope;
        private readonly HashSet<string> _filters;
        private readonly HashSet<string> _attributeFilters;

        /// <summary>
        /// Gets or sets the type of the report.
        /// This represents the <c>/ReportType</c> option.
        /// The Default value is <see cref="DotCoverReportType.XML"/>.
        /// </summary>
        public DotCoverReportType ReportType { get; set; }

        /// <summary>
        /// Gets or sets program working directory
        /// This represents the <c>/TargetWorkingDir</c> option.
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
        /// Gets or sets a value indicating whether the default (automatically added) filters should be disabled
        /// This represents the <c>/DisableDefaultFilters</c> option.
        /// </summary>
        public bool DisableDefaultFilters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotCoverAnalyseSettings"/> class.
        /// </summary>
        public DotCoverAnalyseSettings()
        {
            _scope = new HashSet<string>(StringComparer.Ordinal);
            _filters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _attributeFilters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }
    }
}