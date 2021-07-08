// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.IO;
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
        private readonly HashSet<DirectoryPath> _excludeDirectories;
        private readonly HashSet<DirectoryPath> _searchDirectories;

        /// <summary>
        /// Gets the filters.
        /// This represents the <c>-filter</c> option.
        /// </summary>
        /// <value>The filters.</value>
        public ISet<string> Filters => _filters;

        /// <summary>
        /// Gets attribute filters used to exclude classes or methods.
        /// This represents the <c>-excludebyattribute</c> option.
        /// </summary>
        /// <value>The excluded attributes.</value>
        public ISet<string> ExcludedAttributeFilters => _excludedAttributeFilters;

        /// <summary>
        /// Gets file filters used to excluded classes or methods.
        /// This represents the <c>-excludebyfile</c> option.
        /// </summary>
        /// <value>The excluded file filters.</value>
        public ISet<string> ExcludedFileFilters => _excludedFileFilters;

        /// <summary>
        /// Gets or sets a value indicating whether or not auto-implemented properties should be skipped.
        /// </summary>
        public bool SkipAutoProps { get; set; }

        /// <summary>
        /// Gets or sets the register option.
        /// </summary>
        public OpenCoverRegisterOption Register { get; set; }

        /// <summary>
        /// Gets or sets the Return target code offset to be used.
        /// </summary>
        public int? ReturnTargetCodeOffset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the OldStyle option for OpenCover should be used.
        /// </summary>
        public bool OldStyle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to merge the results with an existing file.
        /// </summary>
        public bool MergeOutput { get; set; }

        /// <summary>
        /// Gets a list of directories where assemblies being loaded from will be ignored.
        /// </summary>
        public ISet<DirectoryPath> ExcludeDirectories => _excludeDirectories;

        /// <summary>
        /// Gets or sets the log level of OpenCover.
        /// </summary>
        public OpenCoverLogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the hide skipped option of OpenCover.
        /// </summary>
        public OpenCoverHideSkippedOption HideSkippedOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to merge the coverage results for an assembly
        /// regardless of where it was loaded assuming it has the same file-hash in each location.
        /// </summary>
        public bool MergeByHash { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the default filters should be applied or not.
        /// </summary>
        public bool NoDefaultFilters { get; set; }

        /// <summary>
        /// Gets a list of directories with alternative locations to look for PDBs.
        /// </summary>
        public ISet<DirectoryPath> SearchDirectories => _searchDirectories;

        /// <summary>
        /// Gets or sets a value indicating whether if the value provided in the target parameter
        /// is the name of a service rather than a name of a process.
        /// </summary>
        public bool IsService { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the path to the target directory.
        /// If the target already contains a path, this parameter can be used
        /// as additional path where PDBs may be found.
        /// </summary>
        public DirectoryPath TargetDirectory { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenCoverSettings"/> class.
        /// </summary>
        public OpenCoverSettings()
        {
            _filters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _excludedAttributeFilters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _excludedFileFilters = new HashSet<string>(StringComparer.Ordinal);
            Register = new OpenCoverRegisterOptionUser();
            LogLevel = OpenCoverLogLevel.Info;
            HideSkippedOption = OpenCoverHideSkippedOption.None;
            _excludeDirectories = new HashSet<DirectoryPath>();
            _searchDirectories = new HashSet<DirectoryPath>();
        }
    }
}