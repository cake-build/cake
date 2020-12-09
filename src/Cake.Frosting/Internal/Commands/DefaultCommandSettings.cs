// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Cake.Cli;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Spectre.Cli;

namespace Cake.Frosting.Internal
{
    internal sealed class DefaultCommandSettings : CommandSettings
    {
        [CommandOption("--target|-t <TARGET>")]
        [DefaultValue("Default")]
        [Description("Target task to invoke.")]
        public string Target { get; set; }

        [CommandOption("-e|--exclusive")]
        [Description("Executes the target task without any dependencies.")]
        public bool Exclusive { get; set; }

        [CommandOption("--working|-w <PATH>")]
        [TypeConverter(typeof(DirectoryPathConverter))]
        [Description("Sets the working directory")]
        public DirectoryPath WorkingDirectory { get; set; }

        [CommandOption("--verbosity|-v <VERBOSITY>")]
        [TypeConverter(typeof(VerbosityConverter))]
        [DefaultValue(Verbosity.Normal)]
        [Description("Specifies the amount of information to be displayed.\n(Quiet, Minimal, Normal, Verbose, Diagnostic)")]
        public Verbosity Verbosity { get; set; }

        [CommandOption("--dryrun|--noop|--whatif")]
        [Description("Performs a dry run.")]
        public bool DryRun { get; set; }

        [CommandOption("--tree")]
        [Description("Shows the task dependency tree.")]
        public bool Tree { get; set; }

        [CommandOption("--descriptions")]
        [Description("Shows task descriptions.")]
        public bool Descriptions { get; set; }

        [CommandOption("--version")]
        [Description("Displays version information.")]
        public bool Version { get; set; }

        [CommandOption("--info")]
        [Description("Displays additional information about Cake.")]
        public bool Info { get; set; }
    }
}
