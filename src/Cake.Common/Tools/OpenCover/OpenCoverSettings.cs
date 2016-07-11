// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.OpenCover
{
    /// <summary>
    /// Contains settings used by <see cref="OpenCoverRunner"/>.
    /// </summary>
    public sealed class OpenCoverSettings : ToolSettings
    {
        private readonly HashSet<string> _filters;
        private readonly HashSet<string> _excludedAttributeFilters;
        private readonly HashSet<string> _excludedFileFilters;

        /// <summary>
        /// Gets the filters.
        /// This represents the <c>-filter</c> option.
        /// </summary>
        /// <value>The filters.</value>
        public ISet<string> Filters
        {
            get { return _filters; }
        }

        /// <summary>
        /// Gets attribute filters used to exclude classes or methods.
        /// This represents the <c>-excludebyattribute</c> option.
        /// </summary>
        /// <value>The excluded attributes.</value>
        public ISet<string> ExcludedAttributeFilters
        {
            get { return _excludedAttributeFilters; }
        }

        /// <summary>
        /// Gets file filters used to excluded classes or methods.
        /// This represents the <c>-excludebyfile</c> option.
        /// </summary>
        /// <value>The excluded file filters.</value>
        public ISet<string> ExcludedFileFilters
        {
            get { return _excludedFileFilters; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not auto-implemented properties should be skipped.
        /// </summary>
        public bool SkipAutoProps { get; set; }

        /// <summary>
        /// Gets or sets the register option
        /// </summary>
        public string Register { get; set; }

        /// <summary>
        /// Gets or sets the Return target code offset to be used
        /// </summary>
        public int? ReturnTargetCodeOffset { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenCoverSettings"/> class.
        /// </summary>
        public OpenCoverSettings()
        {
            _filters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _excludedAttributeFilters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _excludedFileFilters = new HashSet<string>(StringComparer.Ordinal);
            this.Register = "user";
        }
    }
}
