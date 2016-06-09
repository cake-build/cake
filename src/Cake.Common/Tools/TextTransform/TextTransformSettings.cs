// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.TextTransform
{
    /// <summary>
    /// Contains settings used by <see cref="TextTransformRunner"/>.
    /// </summary>
    public sealed class TextTransformSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the assembly used for compiling and running the text template.
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        public string Assembly { get; set; }

        /// <summary>
        /// Gets or sets the output file for the transform.
        /// </summary>
        /// <value>
        /// The output file.
        /// </value>
        public FilePath OutputFile { get; set; }

        /// <summary>
        /// Gets or sets namespace that is used for compiling the text template.
        /// </summary>
        /// <value>
        /// The namespace.
        /// </value>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets a directory that contains the text templates sourced in the specified text template.
        /// </summary>
        public DirectoryPath IncludeDirectory { get; set; }

        /// <summary>
        /// Gets or sets a directory to search for assemblies specified within the text template.
        /// </summary>
        /// <value>
        /// The reference path.
        /// </value>
        public DirectoryPath ReferencePath { get; set; }
    }
}
