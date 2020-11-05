// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Modules;
using Cake.Core;
using Cake.Core.Composition;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.Modules;
using Cake.Core.Scripting;
using Cake.Features.Building.Hosts;
using Cake.Infrastructure.Scripting;
using Cake.NuGet;
using Spectre.Cli;

namespace Cake.Infrastructure
{
    public sealed class ContainerConfigurator : IContainerConfigurator
    {
        public void Configure(
            ICakeContainerRegistrar registrar,
            ICakeConfiguration configuration,
            IRemainingArguments arguments)
        {
            // Arguments
            registrar.RegisterInstance(new CakeArguments(arguments.Parsed)).AsSelf().As<ICakeArguments>();

            // Scripting
            registrar.RegisterType<RoslynScriptEngine>().As<IScriptEngine>().Singleton();
            registrar.RegisterType<BuildScriptHost>().Singleton();
            registrar.RegisterType<DryRunScriptHost>().Singleton();
            registrar.RegisterType<TreeScriptHost>().Singleton();
            registrar.RegisterType<DescriptionScriptHost>().Singleton();

            // Diagnostics
            registrar.RegisterType<CakeBuildLog>().As<ICakeLog>().Singleton();
            registrar.RegisterType<CakeDebugger>().As<ICakeDebugger>().Singleton();

            // External modules
            new CoreModule().Register(registrar);
            new CommonModule().Register(registrar);
            new NuGetModule(configuration).Register(registrar);

            // Misc registrations.
            registrar.RegisterType<CakeReportPrinter>().As<ICakeReportPrinter>().Singleton();
            registrar.RegisterType<CakeConsole>().As<IConsole>().Singleton();
            registrar.RegisterInstance(configuration).As<ICakeConfiguration>().Singleton();
        }
    }
}
