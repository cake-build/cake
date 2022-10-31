// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Cake.Common.Build.GitHubActions.Data
{
    /// <summary>
    /// Provides optional annotation data associated with a GitHub Actions command.
    /// </summary>
    public sealed class GitHubActionsAnnotation
    {
        /// <summary>
        /// Gets or sets the custom title.
        /// </summary>
        /// <value>
        /// The custom title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The path of the file.
        /// </value>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the start line number.
        /// </summary>
        /// <value>
        /// The start line number.
        /// </value>
        public int? StartLine { get; set; }

        /// <summary>
        /// Gets or sets the end line number.
        /// </summary>
        /// <value>
        /// The end line number.
        /// </value>
        public int? EndLine { get; set; }

        /// <summary>
        /// Gets or sets the start column number.
        /// </summary>
        /// <value>
        /// The start column number.
        /// </value>
        public int? StartColumn { get; set; }

        /// <summary>
        /// Gets or sets the end column number.
        /// </summary>
        /// <value>
        /// The end column number.
        /// </value>
        public int? EndColumn { get; set; }

        internal Dictionary<string, string> GetParameters()
        {
            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(Title))
            {
                parameters.Add("title", Title);
            }
            if (!string.IsNullOrWhiteSpace(File))
            {
                parameters.Add("file", File);
            }
            if (StartLine.HasValue)
            {
                parameters.Add("line", StartLine.ToString());
            }
            if (EndLine.HasValue)
            {
                parameters.Add("endLine", EndLine.ToString());
            }
            if (StartColumn.HasValue)
            {
                parameters.Add("col", StartColumn.ToString());
            }
            if (EndColumn.HasValue)
            {
                parameters.Add("endColumn", EndColumn.ToString());
            }
            return parameters;
        }
    }
}