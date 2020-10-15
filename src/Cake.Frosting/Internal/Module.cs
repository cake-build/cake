// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Cake.Core;
using Cake.Core.Composition;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Frosting.Internal;
using Cake.Frosting.Internal.Commands;
using Cake.Frosting.Internal.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting
{
    internal class Module : ICakeModule
    {
        public void Register(ICakeContainerRegistrar registry)
        {
            // Register the entry assembly.
            registry.RegisterInstance(Assembly.GetEntryAssembly());

            // Core services
            registry.RegisterType<CakeHost>().As<ICakeHost>().Singleton();
            registry.RegisterType<DefaultExecutionStrategy>().As<IExecutionStrategy>().Singleton();
            registry.RegisterType<EngineInitializer>().Singleton();
            registry.RegisterType<TaskFinder>().As<ICakeTaskFinder>().Singleton();

            // Logging
            registry.RegisterType<CakeLog>().As<ICakeLog>().Singleton();
            registry.RegisterType<CakeConsole>().As<IConsole>().Singleton();

            // Configuration
            registry.RegisterType<CakeConfigurationProvider>().AsSelf().Singleton();
            registry.RegisterType<Configuration>().As<ICakeConfiguration>().Singleton();

            // Commands
            registry.RegisterType<CommandFactory>().AsSelf().Singleton();
            registry.RegisterType<RunCommand>().AsSelf().Singleton();
            registry.RegisterType<DryRunCommand>().AsSelf().Singleton();
            registry.RegisterType<VersionCommand>().AsSelf().Singleton();
            registry.RegisterType<HelpCommand>().AsSelf().Singleton();

            // Misc
            registry.RegisterType<ReportPrinter>().As<ICakeReportPrinter>().Singleton();
            registry.RegisterType<RawArguments>().As<ICakeArguments>().Singleton();

            // Tooling
            registry.RegisterType<ToolInstaller>().As<IToolInstaller>().Singleton();

            // Register default stuff.
            registry.RegisterType<FrostingContext>().AsSelf().As<IFrostingContext>().Singleton();
            registry.RegisterType<CakeHostOptions>().AsSelf().Singleton();
        }
    }
}
