﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Configuration;
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

                // Run the build feature.
                return _builder.Run(context.Remaining, new BuildFeatureSettings(host)
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
                return LogException(_log, ex);
            }
        }

        private static int LogException<T>(ICakeLog log, T ex)
            where T : Exception
        {
            log = log ?? new CakeBuildLog(
                new CakeConsole(new CakeEnvironment(new CakePlatform(), new CakeRuntime()), new CakeConfiguration(new Dictionary<string, string>())));

            if (log.Verbosity == Verbosity.Diagnostic)
            {
                log.Error("Error: {0}", ex);
            }
            else
            {
                log.Error("Error: {0}", ex.Message);
                if (ex is AggregateException aex)
                {
                    foreach (var exception in aex.Flatten().InnerExceptions)
                    {
                        log.Error("\t{0}", exception.Message);
                    }
                }
            }

            return 1;
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

            return _bootstrapper.Run(context.Remaining, new BootstrapFeatureSettings
            {
                Script = settings.Script,
                Verbosity = settings.Verbosity
            });
        }
    }
}
