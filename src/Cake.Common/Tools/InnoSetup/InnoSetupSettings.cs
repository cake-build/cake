// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.InnoSetup
{
    /// <summary>
    /// Contains settings used by the <see cref="InnoSetupRunner"/>.
    /// </summary>
    public sealed class InnoSetupSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the script compiler defines. Emulates <c>#define public</c> preprocessor directive.
        /// </summary>
        public IDictionary<string, string> Defines { get; set; }

        /// <summary>
        /// Gets or sets whether or not the compiler should generate output (<c>/O+</c> and <c>/O-</c> command line options,
        /// overrides the script's <c>Output</c> attribute).
        /// </summary>
        public bool? EnableOutput { get; set; }

        /// <summary>
        /// Gets or sets the output directory (<c>/O&lt;path&gt;</c> command line option, overrides the script's <c>OutputDir</c>
        /// attribute).
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the output base file name (<c>/F&lt;filename&gt;</c> command line option, overrides the script's
        /// <c>OutputBaseFilename</c> attribute).
        /// </summary>
        public string OutputBaseFilename { get; set; }

        /// <summary>
        /// Gets or sets the script compiler's quiet mode (<c>/Q</c> and <c>/Qp</c> command line options).
        /// </summary>
        public InnoSetupQuietMode QuietMode { get; set; }

        /// <summary>
        /// Gets or sets the version of Inno Setup to be used with this command. By default the highest installed version of Inno Setup is used.
        /// </summary>
        public InnoSetupVersion? Version { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InnoSetupSettings"/> class with the default settings.
        /// </summary>
        public InnoSetupSettings()
        {
            Defines = new Dictionary<string, string>();
        }
    }
}
