// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Cake.Cli;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Spectre.Console.Cli;

namespace Cake.Commands
{
    /// <summary>
    /// Settings for the default command.
    /// </summary>
    public sealed class DefaultCommandSettings : CommandSettings
    {
        /// <summary>
        /// Gets or sets the Cake script to execute.
        /// </summary>
        [CommandArgument(0, "[SCRIPT]")]
        [Description("The Cake script. Defaults to [grey]build.cake[/]")]
        [TypeConverter(typeof(Cli.FilePathConverter))]
        [DefaultValue("build.cake")]
        public FilePath Script { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to bootstrap modules without running the build.
        /// </summary>
        [CommandOption("--bootstrap")]
        [Description("Download/install modules defined by [grey]#module[/] directives, but do not run build.")]
        public bool Bootstrap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip bootstrapping when running the build.
        /// </summary>
        [CommandOption("--skip-bootstrap")]
        [Description("Skips bootstrapping when running build.")]
        public bool SkipBootstrap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to launch the script in debug mode.
        /// </summary>
        [CommandOption("--debug|-d")]
        [Description("Launches script in debug mode.")]
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets the verbosity level for logging.
        /// </summary>
        [CommandOption("--verbosity|-v <VERBOSITY>")]
        [Description("Specifies the amount of information to be displayed.\n(Quiet, Minimal, Normal, Verbose, Diagnostic)")]
        [TypeConverter(typeof(VerbosityConverter))]
        [DefaultValue(Verbosity.Normal)]
        public Verbosity Verbosity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show descriptions for each task.
        /// </summary>
        [CommandOption("--description|--descriptions|--showdescription|--showdescriptions")]
        [Description("Shows description for each task.")]
        public bool Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the task dependency tree.
        /// </summary>
        [CommandOption("--tree|--showtree")]
        [Description("Shows the task dependency tree.")]
        public bool Tree { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to perform a dry run.
        /// </summary>
        [CommandOption("--dryrun|--noop|--whatif")]
        [Description("Performs a dry run.")]
        public bool DryRun { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to execute the target task without any dependencies.
        /// </summary>
        [CommandOption("--exclusive|-e")]
        [Description("Executes the target task without any dependencies.")]
        public bool Exclusive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display version information.
        /// </summary>
        [CommandOption("--version|--ver")]
        [Description("Displays version information.")]
        public bool ShowVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display additional information about Cake.
        /// </summary>
        [CommandOption("--info")]
        [Description("Displays additional information about Cake.")]
        public bool ShowInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force the script to be recompiled if caching is enabled.
        /// </summary>
        [CommandOption("--" + Infrastructure.Constants.Cache.InvalidateScriptCache)]
        [Description("Forces the script to be recompiled if caching is enabled.")]
        public bool Recompile { get; set; }
    }
}
