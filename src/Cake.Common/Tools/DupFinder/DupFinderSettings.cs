// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DupFinder
{
    /// <summary>
    /// Contains settings used by <see cref="DupFinderRunner"/> .
    /// </summary>
    public sealed class DupFinderSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the debug output should be enabled.
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets the complexity threshold for duplicate fragments.
        /// Code fragment with lower complexity are discarded.
        /// </summary>
        public int? DiscardCost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to discard similar fields with different names.
        /// </summary>
        public bool DiscardFieldsName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to discard similar lines of code with different literals.
        /// </summary>
        public bool DiscardLiterals { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to discard similar local variables with different names.
        /// </summary>
        public bool DiscardLocalVariablesName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to discard similar types with different names.
        /// </summary>
        public bool DiscardTypes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the process priority should be set to idle.
        /// </summary>
        public bool IdlePriority { get; set; }

        /// <summary>
        /// Gets or sets a list of keywords to exclude files that contain one of the keywords in their opening comments.
        /// </summary>
        public string[] ExcludeFilesByStartingCommentSubstring { get; set; }

        /// <summary>
        /// Gets or sets a list of keywords to exclude regions that contain one of the keywords in their message.
        /// </summary>
        public string[] ExcludeCodeRegionsByNameSubstring { get; set; }

        /// <summary>
        /// Gets or sets a lift of patterns which will be excluded from the analysis.
        /// </summary>
        public string[] ExcludePattern { get; set; }

        /// <summary>
        /// Gets or sets MsBuild properties.
        /// </summary>
        public Dictionary<string, string> MsBuildProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to normalize type names to the last subtype.
        /// </summary>
        public bool NormalizeTypes { get; set; }

        /// <summary>
        /// Gets or sets the directory where caches will be stored.
        /// The default is %TEMP%.
        /// </summary>
        public DirectoryPath CachesHome { get; set; }

        /// <summary>
        /// Gets or sets the location DupFinder should write its output.
        /// </summary>
        /// <value>The location DupFinder should write its output</value>
        public FilePath OutputFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show CPU and memory usage statistics.
        /// </summary>
        public bool ShowStats { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show duplicates text in the report.
        /// </summary>
        public bool ShowText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to throw an exception on finding duplicates
        /// </summary>
        public bool ThrowExceptionOnFindingDuplicates { get; set; }
    }
}
