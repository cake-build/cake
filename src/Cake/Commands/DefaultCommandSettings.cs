// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Infrastructure.Converters;
using Spectre.Cli;

namespace Cake.Commands
{
    public sealed class DefaultCommandSettings : CommandSettings
    {
        [CommandArgument(0, "[SCRIPT]")]
        [Description("The Cake script")]
        [TypeConverter(typeof(FilePathConverter))]
        [DefaultValue("build.cake")]
        public FilePath Script { get; set; }

        [CommandOption("-v|--verbosity")]
        [Description("Specifies the amount of information to be displayed.")]
        [TypeConverter(typeof(VerbosityConverter))]
        [DefaultValue(Verbosity.Normal)]
        public Verbosity Verbosity { get; set; }

        [CommandOption("-d|--debug")]
        [Description("Launches script in debug mode.")]
        public bool Debug { get; set; }

        [CommandOption("--dryrun|--noop|--whatif")]
        [Description("Performs a dry run.")]
        public bool DryRun { get; set; }

        [CommandOption("--exclusive")]
        [Description("Execute a single task without any dependencies.")]
        public bool Exclusive { get; set; }

        [CommandOption("--bootstrap")]
        [Description("Download/install modules defined by #module directives")]
        public bool Bootstrap { get; set; }

        [CommandOption("--showdescription|--description")]
        [Description("Shows description about tasks.")]
        public bool ShowDescription { get; set; }

        [CommandOption("--showtree|--tree")]
        [Description("Shows the task dependency tree.")]
        public bool ShowTree { get; set; }

        [CommandOption("--version|--ver")]
        [Description("Displays version information.")]
        public bool ShowVersion { get; set; }

        [CommandOption("--info")]
        [Description("Displays additional information about Cake.")]
        public bool ShowInfo { get; set; }
    }
}
