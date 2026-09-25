// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Cli;
using Cake.Core;
using Cake.Core.Composition;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using Cake.Infrastructure.Scripting;

namespace Cake.Infrastructure;

/// <summary>
/// Represents a container configurator for Cake.
/// </summary>
public sealed class ContainerConfigurator : IContainerConfigurator
{
    /// <summary>
    /// Configures the container with the specified registrar, configuration, and arguments.
    /// </summary>
    /// <param name="registrar">The container registrar.</param>
    /// <param name="configuration">The Cake configuration.</param>
    /// <param name="arguments">The Cake arguments.</param>
    public void Configure(
        ICakeContainerRegistrar registrar,
        ICakeConfiguration configuration,
        ICakeArguments arguments)
    {
        // Arguments
        registrar.RegisterInstance(arguments).AsSelf().As<ICakeArguments>();

        // Scripting
        registrar.RegisterType<RoslynScriptEngine>().As<IScriptEngine>().Singleton();
        registrar.RegisterType<BuildScriptHost>().Singleton();
        registrar.RegisterType<DryRunScriptHost>().Singleton();
        registrar.RegisterType<TreeScriptHost>().Singleton();
        registrar.RegisterType<DescriptionScriptHost>().Singleton();
        registrar.RegisterType<ReferenceAssemblyResolver>().As<IReferenceAssemblyResolver>().Singleton();

        // Diagnostics
        registrar.AddCakeDiagnostics();
        registrar.RegisterType<CakeDebugger>().As<ICakeDebugger>().Singleton();

        // External modules
        registrar.UseCakeDefaultModules();

        // Misc registrations.
        RegisterPlainReportPrinterIfDisabled(registrar, configuration);

        registrar.RegisterInstance(configuration).As<ICakeConfiguration>().Singleton();
    }

    private static void RegisterPlainReportPrinterIfDisabled(ICakeContainerRegistrar registrar, ICakeConfiguration configuration)
    {
        var useSpectre = configuration.GetValue(Constants.Settings.UseSpectreConsoleForConsoleOutput) ?? "true";
        if (!useSpectre.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            registrar.RegisterType<CakeReportPrinter>().As<ICakeReportPrinter>().Singleton();
        }
    }
}
