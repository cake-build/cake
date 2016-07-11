// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Arguments;
using Cake.Commands;
using Cake.Composition;
using Cake.Core;
using Cake.Core.Composition;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Diagnostics;
using Cake.Scripting;

namespace Cake.Modules
{
    internal sealed class CakeModule : ICakeModule
    {
        public void Register(ICakeContainerRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            // Core services.
            registry.RegisterType<CakeReportPrinter>().As<ICakeReportPrinter>().Singleton();
            registry.RegisterType<CakeConsole>().As<IConsole>().Singleton();

            // Modules
            registry.RegisterType<ModuleSearcher>().Singleton();
            registry.RegisterType<ModuleLoader>().Singleton();

            // Configuration
            registry.RegisterType<CakeConfigurationProvider>().Singleton();

            // Cake services.
            registry.RegisterType<ArgumentParser>().As<IArgumentParser>().Singleton();
            registry.RegisterType<CommandFactory>().As<ICommandFactory>().Singleton();
            registry.RegisterType<CakeApplication>().Singleton();
            registry.RegisterType<CakeBuildLog>().As<ICakeLog>().Singleton();
            registry.RegisterType<VerbosityParser>().Singleton();
            registry.RegisterType<CakeDebugger>().As<IDebugger>().Singleton();

            // Scripting
            registry.RegisterType<BuildScriptHost>().Singleton();
            registry.RegisterType<DescriptionScriptHost>().Singleton();
            registry.RegisterType<DryRunScriptHost>().Singleton();

            // Register commands.
            registry.RegisterType<BuildCommand>().AsSelf().Transient();
            registry.RegisterType<DebugCommand>().AsSelf().Transient();
            registry.RegisterType<DescriptionCommand>().AsSelf().Transient();
            registry.RegisterType<DryRunCommand>().AsSelf().Transient();
            registry.RegisterType<HelpCommand>().AsSelf().Transient();
            registry.RegisterType<VersionCommand>().AsSelf().Transient();
        }
    }
}
