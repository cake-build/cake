// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Cli;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Features.Bootstrapping;
using Cake.Features.Building;
using Spectre.Console.Cli;

namespace Cake.Commands
{
    public sealed class DefaultCommand : Command<DefaultCommandSettings>
    {
        private readonly IBuildFeature _builder;
        private readonly IBootstrapFeature _bootstrapper;
        private readonly ICakeVersionFeature _version;
        private readonly ICakeInfoFeature _info;
        private readonly IConsole _console;
        private readonly ICakeLog _log;

        public DefaultCommand(
            IBuildFeature builder,
            IBootstrapFeature bootstrapper,
            ICakeVersionFeature version,
            ICakeInfoFeature info,
            IConsole console,
            ICakeLog log)
        {
            _builder = builder;
            _bootstrapper = bootstrapper;
            _version = version;
            _info = info;
            _console = console;
            _log = log;
        }

        public override int Execute(CommandContext context, DefaultCommandSettings settings)
        {
            try
            {
                // Set log verbosity.
                _log.Verbosity = settings.Verbosity;

                if (settings.ShowVersion)
                {
                    _version.Run(_console);
                    return 0;
                }
                else if (settings.ShowInfo)
                {
                    _info.Run(_console);
                    return 0;
                }

                // Get the build host type.
                var host = GetBuildHostKind(settings);

                // Run the bootstrapper?
                if (!settings.SkipBootstrap || settings.Bootstrap)
                {
                    int bootstrapperResult = PerformBootstrapping(context, settings, host);
                    if (bootstrapperResult != 0 || settings.Bootstrap)
                    {
                        return bootstrapperResult;
                    }
                }

                var arguments = CreateCakeArguments(context.Remaining, settings);

                // Run the build feature.
                return _builder.Run(arguments, new BuildFeatureSettings(host)
                {
                    Script = settings.Script,
                    Verbosity = settings.Verbosity,
                    Exclusive = settings.Exclusive,
                    Debug = settings.Debug,
                    NoBootstrapping = settings.SkipBootstrap,
                });
            }
            catch (Exception ex)
            {
                return _log.LogException(ex);
            }
        }

        private BuildHostKind GetBuildHostKind(DefaultCommandSettings settings)
        {
            if (settings.DryRun)
            {
                return BuildHostKind.DryRun;
            }
            else if (settings.Description)
            {
                return BuildHostKind.Description;
            }
            else if (settings.Tree)
            {
                return BuildHostKind.Tree;
            }

            return BuildHostKind.Build;
        }

        private int PerformBootstrapping(CommandContext context, DefaultCommandSettings settings, BuildHostKind host)
        {
            if (host != BuildHostKind.Build && host != BuildHostKind.DryRun)
            {
                return 0;
            }

            var arguments = CreateCakeArguments(context.Remaining, settings);

            return _bootstrapper.Run(arguments, new BootstrapFeatureSettings
            {
                Script = settings.Script,
                Verbosity = settings.Verbosity
            });
        }

        private static CakeArguments CreateCakeArguments(IRemainingArguments remainingArguments, DefaultCommandSettings settings)
        {
            var arguments = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            // Keep the actual remaining arguments in the cake arguments
            foreach (var group in remainingArguments.Parsed)
            {
                string key = group.Key.TrimStart('-');
                arguments[key] = new List<string>();
                foreach (var argument in group)
                {
                    arguments[key].Add(argument);
                }
            }

            // Fixes #4157, We have to add arguments manually which are defined within the DefaultCommandSettings type. Those are not considered "as remaining" because they could be parsed
            const string recompileArgumentName = Infrastructure.Constants.Cache.InvalidateScriptCache;
            if (settings.Recompile && !arguments.ContainsKey(recompileArgumentName))
            {
                arguments[recompileArgumentName] = new List<string>();
                arguments[recompileArgumentName].Add(true.ToString());
            }

            var argumentLookUp = arguments.SelectMany(a => a.Value, Tuple.Create).ToLookup(a => a.Item1.Key, a => a.Item2);
            return new CakeArguments(argumentLookUp);
        }
    }
}
