// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Cake.Cli;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Spectre.Console.Cli;

namespace Cake.Frosting.Internal
{
    internal sealed class DefaultCommandSettings : CommandSettings
    {
        [CommandOption("--target|-t <TARGET>")]
        [DefaultValue("Default")]
        [Description("Target task to invoke.")]
        public string Target { get; set; }

        [CommandOption("--working|-w <PATH>")]
        [TypeConverter(typeof(Cli.DirectoryPathConverter))]
        [Description("Sets the working directory")]
        public DirectoryPath WorkingDirectory { get; set; }

        [CommandOption("--verbosity|-v <VERBOSITY>")]
        [Description("Specifies the amount of information to be displayed.\n(Quiet, Minimal, Normal, Verbose, Diagnostic)")]
        [TypeConverter(typeof(VerbosityConverter))]
        [DefaultValue(Verbosity.Normal)]
        public Verbosity Verbosity { get; set; }

        [CommandOption("--description|--descriptions|--showdescription|--showdescriptions")]
        [Description("Shows description for each task.")]
        public bool Description { get; set; }

        [CommandOption("--tree|--showtree")]
        [Description("Shows the task dependency tree.")]
        public bool Tree { get; set; }

        [CommandOption("--dryrun|--noop|--whatif")]
        [Description("Performs a dry run.")]
        public bool DryRun { get; set; }

        [CommandOption("--exclusive|-e")]
        [Description("Executes the target task without any dependencies.")]
        public bool Exclusive { get; set; }

        [CommandOption("--version|--ver")]
        [Description("Displays version information.")]
        public bool Version { get; set; }

        [CommandOption("--info")]
        [Description("Displays additional information about Cake.")]
        public bool Info { get; set; }
    }
}
