// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.BuildServer;
using Cake.Common.Tools.DotNet.Clean;
using Cake.Common.Tools.DotNet.Execute;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Tools.DotNet.Restore;
using Cake.Common.Tools.DotNet.Run;
using Cake.Common.Tools.DotNet.Tool;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Common.Tools.DotNetCore.BuildServer;
using Cake.Common.Tools.DotNetCore.Clean;
using Cake.Common.Tools.DotNetCore.Execute;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Common.Tools.DotNetCore.Run;
using Cake.Common.Tools.DotNetCore.Tool;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/dotnet/cli">.NET Core CLI</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, the .Net Core CLI tools will need to be installed on the machine where
    /// the Cake script is being executed.  See this <see href="https://www.microsoft.com/net/core">page</see> for information
    /// on how to install.
    /// </para>
    /// </summary>
    [CakeAliasCategory("DotNet")]
    public static class DotNetAliases
    {
        /// <summary>
        /// Execute an assembly.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <example>
        /// <code>
        /// DotNetExecute("./bin/Debug/app.dll");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Execute")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Execute")]
        public static void DotNetExecute(this ICakeContext context, FilePath assemblyPath)
        {
            context.DotNetExecute(assemblyPath, null);
        }

        /// <summary>
        /// Execute an assembly with arguments in the specific path.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="arguments">The arguments.</param>
        /// <example>
        /// <code>
        /// DotNetExecute("./bin/Debug/app.dll", "--arg");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Execute")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Execute")]
        public static void DotNetExecute(this ICakeContext context, FilePath assemblyPath, ProcessArgumentBuilder arguments)
        {
            context.DotNetExecute(assemblyPath, arguments, null);
        }

        /// <summary>
        /// Execute an assembly with arguments in the specific path with settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetExecuteSettings
        /// {
        ///     FrameworkVersion = "1.0.3"
        /// };
        ///
        /// DotNetExecute("./bin/Debug/app.dll", "--arg", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Execute")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Execute")]
        public static void DotNetExecute(this ICakeContext context, FilePath assemblyPath, ProcessArgumentBuilder arguments, DotNetExecuteSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (assemblyPath is null)
            {
                throw new ArgumentNullException(nameof(assemblyPath));
            }

            if (settings is null)
            {
                settings = new DotNetExecuteSettings();
            }

            var executor = new DotNetCoreExecutor(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            executor.Execute(assemblyPath, arguments, settings);
        }

        /// <summary>
        /// Restore all NuGet Packages.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetRestore();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Restore")]
        public static void DotNetRestore(this ICakeContext context)
        {
            context.DotNetRestore(null, null);
        }

        /// <summary>
        /// Restore all NuGet Packages in the specified path.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">List of projects and project folders to restore. Each value can be: a path to a project.json or global.json file, or a folder to recursively search for project.json files.</param>
        /// <example>
        /// <code>
        /// DotNetRestore("./src/*");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Restore")]
        public static void DotNetRestore(this ICakeContext context, string root)
        {
            context.DotNetRestore(root, null);
        }

        /// <summary>
        /// Restore all NuGet Packages with the settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetRestoreSettings
        /// {
        ///     Sources = new[] {"https://www.example.com/nugetfeed", "https://www.example.com/nugetfeed2"},
        ///     FallbackSources = new[] {"https://www.example.com/fallbacknugetfeed"},
        ///     PackagesDirectory = "./packages",
        ///     Verbosity = Information,
        ///     DisableParallel = true,
        ///     InferRuntimes = new[] {"runtime1", "runtime2"}
        /// };
        ///
        /// DotNetRestore(settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Restore")]
        public static void DotNetRestore(this ICakeContext context, DotNetRestoreSettings settings)
        {
            context.DotNetRestore(null, settings);
        }

        /// <summary>
        /// Restore all NuGet Packages in the specified path with settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">List of projects and project folders to restore. Each value can be: a path to a project.json or global.json file, or a folder to recursively search for project.json files.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetRestoreSettings
        /// {
        ///     Sources = new[] {"https://www.example.com/nugetfeed", "https://www.example.com/nugetfeed2"},
        ///     FallbackSources = new[] {"https://www.example.com/fallbacknugetfeed"},
        ///     PackagesDirectory = "./packages",
        ///     Verbosity = Information,
        ///     DisableParallel = true,
        ///     InferRuntimes = new[] {"runtime1", "runtime2"}
        /// };
        ///
        /// DotNetRestore("./src/*", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Restore")]
        public static void DotNetRestore(this ICakeContext context, string root, DotNetRestoreSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetRestoreSettings();
            }

            var restorer = new DotNetCoreRestorer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, context.Log);
            restorer.Restore(root, settings);
        }

        /// <summary>
        /// Build all projects.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The projects path.</param>
        /// <example>
        /// <code>
        /// DotNetBuild("./src/*");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Build")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Build")]
        public static void DotNetBuild(this ICakeContext context, string project)
        {
            context.DotNetBuild(project, null);
        }

        /// <summary>
        /// Build all projects.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The projects path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetBuildSettings
        /// {
        ///     Framework = "netcoreapp2.0",
        ///     Configuration = "Debug",
        ///     OutputDirectory = "./artifacts/"
        /// };
        ///
        /// DotNetBuild("./src/*", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Build")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Build")]
        public static void DotNetBuild(this ICakeContext context, string project, DotNetBuildSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetBuildSettings();
            }

            var builder = new DotNetCoreBuilder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            builder.Build(project, settings);
        }

        /// <summary>
        /// Cleans a project's output.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project's path.</param>
        /// <example>
        /// <code>
        /// DotNetClean("./src/project");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Clean")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Clean")]
        public static void DotNetClean(this ICakeContext context, string project)
        {
            context.DotNetClean(project, null);
        }

        /// <summary>
        /// Cleans a project's output.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The projects path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetCleanSettings
        /// {
        ///     Framework = "netcoreapp2.0",
        ///     Configuration = "Debug",
        ///     OutputDirectory = "./artifacts/"
        /// };
        ///
        /// DotNetClean("./src/project", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Clean")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Clean")]
        public static void DotNetClean(this ICakeContext context, string project, DotNetCleanSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetCleanSettings();
            }

            var cleaner = new DotNetCoreCleaner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            cleaner.Clean(project, settings);
        }

        /// <summary>
        /// Run all projects.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetRun();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Run")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Run")]
        public static void DotNetRun(this ICakeContext context)
        {
            context.DotNetRun(null, null, null);
        }

        /// <summary>
        /// Run project.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project path.</param>
        /// <example>
        /// <code>
        /// DotNetRun("./src/Project");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Run")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Run")]
        public static void DotNetRun(this ICakeContext context, string project)
        {
            context.DotNetRun(project, null, null);
        }

        /// <summary>
        /// Run project with path and arguments.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project path.</param>
        /// <param name="arguments">The arguments.</param>
        /// <example>
        /// <code>
        /// DotNetRun("./src/Project", "--args");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Run")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Run")]
        public static void DotNetRun(this ICakeContext context, string project, ProcessArgumentBuilder arguments)
        {
            context.DotNetRun(project, arguments, null);
        }

        /// <summary>
        /// Run project with settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project path.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetRunSettings
        /// {
        ///     Framework = "netcoreapp2.0",
        ///     Configuration = "Release"
        /// };
        ///
        /// DotNetRun("./src/Project", "--args", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Run")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Run")]
        public static void DotNetRun(this ICakeContext context, string project, ProcessArgumentBuilder arguments, DotNetRunSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetRunSettings();
            }

            var runner = new DotNetCoreRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(project, arguments, settings);
        }

        /// <summary>
        /// Run project with settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetRunSettings
        /// {
        ///     Framework = "netcoreapp2.0",
        ///     Configuration = "Release"
        /// };
        ///
        /// DotNetRun("./src/Project", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Run")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Run")]
        public static void DotNetRun(this ICakeContext context, string project, DotNetRunSettings settings)
        {
            context.DotNetRun(project, null, settings);
        }

        /// <summary>
        /// Builds the specified targets in a project file found in the current working directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetMSBuild();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("MSBuild")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.MSBuild")]
        public static void DotNetMSBuild(this ICakeContext context)
        {
            context.DotNetMSBuild(null, null);
        }

        /// <summary>
        /// Builds the specified targets in the project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectOrDirectory">Project file or directory to search for project file.</param>
        /// <example>
        /// <code>
        /// DotNetMSBuild("foobar.proj");
        /// </code>
        /// </example>
        /// <remarks>
        /// If a directory is specified, MSBuild searches that directory for a project file.
        /// </remarks>
        [CakeMethodAlias]
        [CakeAliasCategory("MSBuild")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.MSBuild")]
        public static void DotNetMSBuild(this ICakeContext context, string projectOrDirectory)
        {
            if (string.IsNullOrWhiteSpace(projectOrDirectory))
            {
                throw new ArgumentNullException(nameof(projectOrDirectory));
            }

            context.DotNetMSBuild(projectOrDirectory, null);
        }

        /// <summary>
        /// Builds the specified targets in a project file found in the current working directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetMSBuildSettings
        /// {
        ///     NoLogo = true,
        ///     MaxCpuCount = -1
        /// };
        ///
        /// DotNetMSBuild(settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("MSBuild")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.MSBuild")]
        public static void DotNetMSBuild(this ICakeContext context, DotNetMSBuildSettings settings)
        {
            context.DotNetMSBuild(null, settings);
        }

        /// <summary>
        /// Builds the specified targets in the project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectOrDirectory">Project file or directory to search for project file.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetMSBuildSettings
        /// {
        ///     NoLogo = true,
        ///     MaxCpuCount = -1
        /// };
        ///
        /// DotNetMSBuild("foobar.proj", settings);
        /// </code>
        /// </example>
        /// <remarks>
        /// If a project file is not specified, MSBuild searches the current working directory for a file that has a file
        /// extension that ends in "proj" and uses that file. If a directory is specified, MSBuild searches that directory for a project file.
        /// </remarks>
        [CakeMethodAlias]
        [CakeAliasCategory("MSBuild")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.MSBuild")]
        public static void DotNetMSBuild(this ICakeContext context, string projectOrDirectory, DotNetMSBuildSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetMSBuildSettings();
            }

            var builder = new DotNetCoreMSBuildBuilder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            builder.Build(projectOrDirectory, settings);
        }

        /// <summary>
        /// Execute an .NET Core Extensibility Tool.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="command">The command to execute.</param>
        /// <example>
        /// <code>
        /// DotNetTool("cake");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetTool(this ICakeContext context, string command)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var arguments = new ProcessArgumentBuilder();
            var settings = new DotNetToolSettings();

            context.DotNetTool(null, command, arguments, settings);
        }

        /// <summary>
        /// Execute an .NET Core Extensibility Tool.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="command">The command to execute.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetToolSettings
        /// {
        ///     DiagnosticOutput = true
        /// };
        ///
        /// DotNetTool("cake", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetTool(this ICakeContext context, string command, DotNetToolSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var arguments = new ProcessArgumentBuilder();

            context.DotNetTool(null, command, arguments, settings);
        }

        /// <summary>
        /// Execute an .NET Core Extensibility Tool.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectPath">The project path.</param>
        /// <param name="command">The command to execute.</param>
        /// <example>
        /// <code>
        /// DotNetTool("./src/project", "xunit", "-xml report.xml");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetTool(this ICakeContext context, FilePath projectPath, string command)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var arguments = new ProcessArgumentBuilder();
            var settings = new DotNetToolSettings();

            context.DotNetTool(projectPath, command, arguments, settings);
        }

        /// <summary>
        /// Execute an .NET Core Extensibility Tool.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectPath">The project path.</param>
        /// <param name="command">The command to execute.</param>
        /// <param name="arguments">The arguments.</param>
        /// <example>
        /// <code>
        /// DotNetTool("./src/project", "xunit", "-xml report.xml");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetTool(this ICakeContext context, FilePath projectPath, string command, ProcessArgumentBuilder arguments)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var settings = new DotNetToolSettings();

            context.DotNetTool(projectPath, command, arguments, settings);
        }

        /// <summary>
        /// Execute an .NET Core Extensibility Tool.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectPath">The project path.</param>
        /// <param name="command">The command to execute.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotNetTool("./src/project", "xunit", "-xml report.xml");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetTool(this ICakeContext context, FilePath projectPath, string command, ProcessArgumentBuilder arguments, DotNetToolSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new DotNetCoreToolRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            runner.Execute(projectPath, command, arguments, settings);
        }

        /// <summary>
        /// Shuts down build servers that are started from dotnet.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetBuildServerShutdown();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Build Server")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.BuildServer")]
        public static void DotNetBuildServerShutdown(this ICakeContext context)
        {
            context.DotNetBuildServerShutdown(null);
        }

        /// <summary>
        /// Shuts down build servers that are started from dotnet.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetBuildServerShutdownSettings
        /// {
        ///     MSBuild = true
        /// };
        ///
        /// DotNetBuildServerShutdown(settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Build Server")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.BuildServer")]
        public static void DotNetBuildServerShutdown(this ICakeContext context, DotNetBuildServerShutdownSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildServer = new DotNetCoreBuildServer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            buildServer.Shutdown(settings ?? new DotNetBuildServerShutdownSettings());
        }
    }
}
