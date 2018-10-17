// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Features.Bootstrapping;
using Cake.Features.Building;
using Cake.Features.Introspection;
using Spectre.Cli;

namespace Cake.Commands
{
    public sealed class DefaultCommand : Command<DefaultCommandSettings>
    {
        private readonly IBuildFeature _builder;
        private readonly IBootstrapFeature _bootstrapper;
        private readonly IVersionFeature _version;
        private readonly IInfoFeature _info;
        private readonly ICakeLog _log;

        public DefaultCommand(
            IBuildFeature builder,
            IBootstrapFeature bootstrapper,
            IVersionFeature version,
            IInfoFeature info,
            ICakeLog log)
        {
            _builder = builder;
            _bootstrapper = bootstrapper;
            _version = version;
            _info = info;
            _log = log;
        }

        public override int Execute(CommandContext context, DefaultCommandSettings settings)
        {
            // Set log verbosity.
            _log.Verbosity = settings.Verbosity;

            if (settings.ShowVersion)
            {
                return _version.Run();
            }
            if (settings.ShowInfo)
            {
                return _info.Run();
            }
            if (settings.Bootstrap)
            {
                return _bootstrapper.Run(context.Remaining, new BootstrapFeatureSettings
                {
                    Script = settings.Script,
                    Verbosity = settings.Verbosity
                });
            }

            // Get the build host type.
            var host = GetBuildHostKind(settings);

            // Run the build feature.
            return _builder.Run(context.Remaining, new BuildFeatureSettings(host)
            {
                Script = settings.Script,
                Verbosity = settings.Verbosity,
                Exclusive = settings.Exclusive,
                Debug = settings.Debug
            });
        }

        private BuildHostKind GetBuildHostKind(DefaultCommandSettings settings)
        {
            if (settings.DryRun)
            {
                return BuildHostKind.DryRun;
            }
            if (settings.ShowDescription)
            {
                return BuildHostKind.Description;
            }
            if (settings.ShowTree)
            {
                return BuildHostKind.Tree;
            }
            return BuildHostKind.Build;
        }
    }
}
