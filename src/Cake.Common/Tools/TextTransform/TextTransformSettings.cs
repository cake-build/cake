// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
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
        /// Gets or sets the namespace that is used for compiling the text template.
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

        /// <summary>
        /// Gets or sets the class name used for converting T4 template into a C# class that can be compiled into your app and executed at runtime.
        /// </summary>
        /// <value>
        /// The class name.
        /// </value>
        /// <remarks>
        /// Requires T4 text template processor version 2 or newer.
        /// </remarks>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets properties passes to the template's Session dictionary.
        /// </summary>
        /// <value>
        /// The properties dictionary.
        /// </value>
        /// <remarks>
        /// Requires T4 text template processor version 2 or newer.
        /// These can also be accessed using strongly typed properties
        /// (declared with &lt;#@ parameter name="&lt;name&gt;" type="&lt;type&gt;" #&gt; directives.)
        /// </remarks>
        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}