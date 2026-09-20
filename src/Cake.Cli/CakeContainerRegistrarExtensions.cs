// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Modules;
using Cake.Core;
using Cake.Core.Composition;
using Cake.Core.Diagnostics;
using Cake.Core.Modules;
using Cake.DotNetTool.Module;
using Cake.NuGet;
using Spectre.Console;

namespace Cake.Cli
{
    /// <summary>
    /// Contains extension methods for <see cref="ICakeContainerRegistrar"/>.
    /// </summary>
    public static class CakeContainerRegistrarExtensions
    {
        /// <summary>
        /// Registers the default Cake diagnostics services.
        /// </summary>
        /// <param name="registrar">The container registrar.</param>
        /// <returns>The same <see cref="ICakeContainerRegistrar"/> instance so that multiple calls can be chained.</returns>
        public static ICakeContainerRegistrar AddCakeDiagnostics(this ICakeContainerRegistrar registrar)
        {
            ArgumentNullException.ThrowIfNull(registrar);

            registrar.RegisterType<CakeBuildLog>().As<ICakeLog>().Singleton();
            registrar.RegisterType<CakeConsole>().As<IConsole>().Singleton();
            registrar.RegisterInstance(AnsiConsole.Console).As<IAnsiConsole>().Singleton();
            registrar.RegisterType<CakeSpectreReportPrinter>().As<ICakeReportPrinter>().Singleton();
            return registrar;
        }

        /// <summary>
        /// Registers the default Cake modules.
        /// </summary>
        /// <param name="registrar">The container registrar.</param>
        /// <returns>The same <see cref="ICakeContainerRegistrar"/> instance so that multiple calls can be chained.</returns>
        public static ICakeContainerRegistrar UseCakeDefaultModules(this ICakeContainerRegistrar registrar)
        {
            ArgumentNullException.ThrowIfNull(registrar);

            new CoreModule().Register(registrar);
            new CommonModule().Register(registrar);
            new NuGetModule().Register(registrar);
            new DotNetToolModule().Register(registrar);
            return registrar;
        }
    }
}
