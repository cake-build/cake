// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Autofac;
using Cake.Arguments;
using Cake.Commands;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Packaging;
using Cake.Core.Scripting;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Tooling;
using Cake.Diagnostics;
using Cake.NuGet;
using Cake.Scripting;

namespace Cake.Autofac
{
    internal sealed class CakeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Core services.
            builder.RegisterType<CakeEngine>().As<ICakeEngine>().SingleInstance();
            builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            builder.RegisterType<CakeEnvironment>().As<ICakeEnvironment>().SingleInstance();
            builder.RegisterType<Globber>().As<IGlobber>().SingleInstance();
            builder.RegisterType<ProcessRunner>().As<IProcessRunner>().SingleInstance();
            builder.RegisterType<ScriptAliasFinder>().As<IScriptAliasFinder>().SingleInstance();
            builder.RegisterType<CakeReportPrinter>().As<ICakeReportPrinter>().SingleInstance();
            builder.RegisterType<CakeConsole>().As<IConsole>().SingleInstance();
            builder.RegisterType<ScriptAnalyzer>().As<IScriptAnalyzer>().SingleInstance();
            builder.RegisterType<ScriptProcessor>().As<IScriptProcessor>().SingleInstance();
            builder.RegisterType<ScriptConventions>().As<IScriptConventions>().SingleInstance();
            builder.RegisterType<NuGetToolResolver>().As<INuGetToolResolver>().SingleInstance();
            builder.RegisterType<WindowsRegistry>().As<IRegistry>().SingleInstance();
            builder.RegisterType<CakeContext>().As<ICakeContext>().SingleInstance();

            // Tooling
            builder.RegisterType<ToolRepository>().As<IToolRepository>().SingleInstance();
            builder.RegisterType<ToolResolutionStrategy>().As<IToolResolutionStrategy>().SingleInstance();
            builder.RegisterType<ToolLocator>().As<IToolLocator>().SingleInstance();

            // Configuration
            builder.RegisterType<CakeConfigurationProvider>().SingleInstance();

            // NuGet addins support
            builder.RegisterType<NuGetVersionUtilityAdapter>().As<INuGetFrameworkCompatibilityFilter>().As<IFrameworkNameParser>().SingleInstance();
            builder.RegisterType<NuGetPackageAssembliesLocator>().As<INuGetPackageAssembliesLocator>().SingleInstance();
            builder.RegisterType<NuGetPackageReferenceBundler>().As<INuGetPackageReferenceBundler>().SingleInstance();
            builder.RegisterType<NuGetAssemblyCompatibilityFilter>().As<INuGetAssemblyCompatibilityFilter>().SingleInstance();
            builder.RegisterType<AssemblyFrameworkNameParser>().As<IAssemblyFrameworkNameParser>().SingleInstance();

            // URI resource support.
            builder.RegisterType<NuGetPackageInstaller>().As<IPackageInstaller>().SingleInstance();
            builder.RegisterType<NuGetPackageContentResolver>().As<INuGetPackageContentResolver>().SingleInstance();

            // Cake services.
            builder.RegisterType<ArgumentParser>().As<IArgumentParser>().SingleInstance();
            builder.RegisterType<CommandFactory>().As<ICommandFactory>().SingleInstance();
            builder.RegisterType<CakeApplication>().SingleInstance();
            builder.RegisterType<ScriptRunner>().As<IScriptRunner>().SingleInstance();
            builder.RegisterType<CakeBuildLog>().As<ICakeLog>().SingleInstance();
            builder.RegisterType<VerbosityParser>().SingleInstance();
            builder.RegisterType<CakeDebugger>().As<IDebugger>().SingleInstance();

            // Register script hosts.
            builder.RegisterType<BuildScriptHost>().SingleInstance();
            builder.RegisterType<DescriptionScriptHost>().SingleInstance();
            builder.RegisterType<DryRunScriptHost>().SingleInstance();

            // Register commands.
            builder.RegisterType<BuildCommand>().AsSelf().InstancePerDependency();
            builder.RegisterType<DebugCommand>().AsSelf().InstancePerDependency();
            builder.RegisterType<DescriptionCommand>().AsSelf().InstancePerDependency();
            builder.RegisterType<DryRunCommand>().AsSelf().InstancePerDependency();
            builder.RegisterType<HelpCommand>().AsSelf().InstancePerDependency();
            builder.RegisterType<VersionCommand>().AsSelf().InstancePerDependency();
        }
    }
}
