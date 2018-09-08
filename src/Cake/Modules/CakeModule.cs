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
        public void Register(ICakeContainerRegistrar registrar)
        {
            if (registrar == null)
            {
                throw new ArgumentNullException(nameof(registrar));
            }

            // Core services.
            registrar.RegisterType<CakeReportPrinter>().As<ICakeReportPrinter>().Singleton();
            registrar.RegisterType<CakeConsole>().As<IConsole>().Singleton();

            // Modules
            registrar.RegisterType<ModuleSearcher>().Singleton();
            registrar.RegisterType<ModuleLoader>().Singleton();

            // Configuration
            registrar.RegisterType<CakeConfigurationProvider>().Singleton();

            // Cake services.
            registrar.RegisterType<ArgumentParser>().As<IArgumentParser>().Singleton();
            registrar.RegisterType<CommandFactory>().As<ICommandFactory>().Singleton();
            registrar.RegisterType<CakeApplication>().Singleton();
            registrar.RegisterType<CakeBuildLog>().As<ICakeLog>().Singleton();
            registrar.RegisterType<VerbosityParser>().Singleton();
            registrar.RegisterType<CakeDebugger>().As<IDebugger>().Singleton();

            // Scripting
            registrar.RegisterType<BuildScriptHost>().Singleton();
            registrar.RegisterType<DescriptionScriptHost>().Singleton();
            registrar.RegisterType<TaskTreeScriptHost>().Singleton();
            registrar.RegisterType<DryRunScriptHost>().Singleton();

            // Register commands.
            registrar.RegisterType<BootstrapCommand>().AsSelf().Transient();
            registrar.RegisterType<BuildCommand>().AsSelf().Transient();
            registrar.RegisterType<DebugCommand>().AsSelf().Transient();
            registrar.RegisterType<DescriptionCommand>().AsSelf().Transient();
            registrar.RegisterType<TaskTreeCommand>().AsSelf().Transient();
            registrar.RegisterType<DryRunCommand>().AsSelf().Transient();
            registrar.RegisterType<HelpCommand>().AsSelf().Transient();
            registrar.RegisterType<VersionCommand>().AsSelf().Transient();
            registrar.RegisterType<InfoCommand>().AsSelf().Transient();
        }
    }
}
