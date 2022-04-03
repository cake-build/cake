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
using Cake.Common.Tools.DotNet.Format;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Tools.DotNet.NuGet.Delete;
using Cake.Common.Tools.DotNet.NuGet.Push;
using Cake.Common.Tools.DotNet.NuGet.Source;
using Cake.Common.Tools.DotNet.Pack;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Common.Tools.DotNet.Restore;
using Cake.Common.Tools.DotNet.Run;
using Cake.Common.Tools.DotNet.SDKCheck;
using Cake.Common.Tools.DotNet.Test;
using Cake.Common.Tools.DotNet.Tool;
using Cake.Common.Tools.DotNet.VSTest;
using Cake.Common.Tools.DotNet.Workload.Search;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Common.Tools.DotNetCore.BuildServer;
using Cake.Common.Tools.DotNetCore.Clean;
using Cake.Common.Tools.DotNetCore.Execute;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.DotNetCore.NuGet.Delete;
using Cake.Common.Tools.DotNetCore.NuGet.Push;
using Cake.Common.Tools.DotNetCore.NuGet.Source;
using Cake.Common.Tools.DotNetCore.Pack;
using Cake.Common.Tools.DotNetCore.Publish;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Common.Tools.DotNetCore.Run;
using Cake.Common.Tools.DotNetCore.Test;
using Cake.Common.Tools.DotNetCore.Tool;
using Cake.Common.Tools.DotNetCore.VSTest;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/dotnet/cli">.NET CLI</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, the .NET CLI tools will need to be installed on the machine where
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
        /// <param name="root">Path to the project file to restore.</param>
        /// <example>
        /// <code>
        /// DotNetRestore("./src/MyProject/MyProject.csproj");
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
        /// <param name="root">Path to the project file to restore.</param>
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
        /// DotNetRestore("./src/MyProject/MyProject.csproj", settings);
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
        /// Publish all projects.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The projects path.</param>
        /// <example>
        /// <code>
        /// DotNetPublish("./src/*");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Publish")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Publish")]
        public static void DotNetPublish(this ICakeContext context, string project)
        {
            context.DotNetPublish(project, null);
        }

        /// <summary>
        /// Publish all projects.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The projects path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetPublishSettings
        /// {
        ///     Framework = "netcoreapp2.0",
        ///     Configuration = "Release",
        ///     OutputDirectory = "./artifacts/"
        /// };
        ///
        /// DotNetPublish("./src/*", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Publish")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Publish")]
        public static void DotNetPublish(this ICakeContext context, string project, DotNetPublishSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetPublishSettings();
            }

            var publisher = new DotNetCorePublisher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            publisher.Publish(project, settings);
        }

        /// <summary>
        /// Test project.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetTest();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Test")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Test")]
        public static void DotNetTest(this ICakeContext context)
        {
            context.DotNetTest(null, null, null);
        }

        /// <summary>
        /// Test project with path.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project path.</param>
        /// <example>
        /// <para>Specify the path to the .csproj file in the test project.</para>
        /// <code>
        /// DotNetTest("./test/Project.Tests/Project.Tests.csproj");
        /// </code>
        /// <para>You could also specify a task that runs multiple test projects.</para>
        /// <para>Cake task:</para>
        /// <code>
        /// Task("Test")
        ///     .Does(() =>
        /// {
        ///     var projectFiles = GetFiles("./test/**/*.csproj");
        ///     foreach(var file in projectFiles)
        ///     {
        ///         DotNetTest(file.FullPath);
        ///     }
        /// });
        /// </code>
        /// <para>If your test project is using project.json, the project parameter should just be the directory path.</para>
        /// <code>
        /// DotNetTest("./test/Project.Tests/");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Test")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Test")]
        public static void DotNetTest(this ICakeContext context, string project)
        {
            context.DotNetTest(project, null, null);
        }

        /// <summary>
        /// Test project with settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetTestSettings
        /// {
        ///     Configuration = "Release"
        /// };
        ///
        /// DotNetTest("./test/Project.Tests/Project.Tests.csproj", settings);
        /// </code>
        /// <para>You could also specify a task that runs multiple test projects.</para>
        /// <para>Cake task:</para>
        /// <code>
        /// Task("Test")
        ///     .Does(() =>
        /// {
        ///     var settings = new DotNetTestSettings
        ///     {
        ///         Configuration = "Release"
        ///     };
        ///
        ///     var projectFiles = GetFiles("./test/**/*.csproj");
        ///     foreach(var file in projectFiles)
        ///     {
        ///         DotNetTest(file.FullPath, settings);
        ///     }
        /// });
        /// </code>
        /// <para>If your test project is using project.json, the project parameter should just be the directory path.</para>
        /// <code>
        /// var settings = new DotNetTestSettings
        /// {
        ///     Configuration = "Release"
        /// };
        ///
        /// DotNetTest("./test/Project.Tests/", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Test")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Test")]
        public static void DotNetTest(this ICakeContext context, string project, DotNetTestSettings settings)
        {
            context.DotNetTest(project, null, settings);
        }

        /// <summary>
        /// Test project with settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project path.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetTestSettings
        /// {
        ///     Configuration = "Release"
        /// };
        ///
        /// DotNetTest("./test/Project.Tests/Project.Tests.csproj", settings);
        /// </code>
        /// <para>You could also specify a task that runs multiple test projects.</para>
        /// <para>Cake task:</para>
        /// <code>
        /// Task("Test")
        ///     .Does(() =>
        /// {
        ///     var settings = new DotNetTestSettings
        ///     {
        ///         Configuration = "Release"
        ///     };
        ///
        ///     var projectFiles = GetFiles("./test/**/*.csproj");
        ///     foreach(var file in projectFiles)
        ///     {
        ///         DotNetTest(file.FullPath, "MSTest.MapInconclusiveToFailed=true", settings);
        ///     }
        /// });
        /// </code>
        /// <para>If your test project is using project.json, the project parameter should just be the directory path.</para>
        /// <code>
        /// var settings = new DotNetTestSettings
        /// {
        ///     Configuration = "Release"
        /// };
        ///
        /// DotNetTest("./test/Project.Tests/", "MSTest.MapInconclusiveToFailed=true", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Test")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Test")]
        public static void DotNetTest(this ICakeContext context, string project, ProcessArgumentBuilder arguments, DotNetTestSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetTestSettings();
            }

            var tester = new DotNetCoreTester(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            tester.Test(project, arguments, settings);
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
        /// Delete a NuGet Package from a server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetNuGetDelete();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Delete")]
        public static void DotNetNuGetDelete(this ICakeContext context)
        {
            context.DotNetNuGetDelete(null, null, null);
        }

        /// <summary>
        /// Deletes a package from nuget.org.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">Name of package to delete.</param>
        /// <example>
        /// <code>
        /// DotNetNuGetDelete("Microsoft.AspNetCore.Mvc");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Delete")]
        public static void DotNetNuGetDelete(this ICakeContext context, string packageName)
        {
            context.DotNetNuGetDelete(packageName, null, null);
        }

        /// <summary>
        /// Deletes a specific version of a package from nuget.org.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">Name of package to delete.</param>
        /// <param name="packageVersion">Version of package to delete.</param>
        /// <example>
        /// <code>
        /// DotNetRestore("Microsoft.AspNetCore.Mvc", "1.0");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Delete")]
        public static void DotNetNuGetDelete(this ICakeContext context, string packageName, string packageVersion)
        {
            context.DotNetNuGetDelete(packageName, packageVersion, null);
        }

        /// <summary>
        /// Deletes a package from a server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">Name of package to delete.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetDeleteSettings
        /// {
        ///     Source = "https://www.example.com/nugetfeed",
        ///     NonInteractive = true
        /// };
        ///
        /// DotNetNuGetDelete("Microsoft.AspNetCore.Mvc", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Delete")]
        public static void DotNetNuGetDelete(this ICakeContext context, string packageName, DotNetNuGetDeleteSettings settings)
        {
            context.DotNetNuGetDelete(packageName, null, settings);
        }

        /// <summary>
        /// Deletes a package from a server using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetDeleteSettings
        /// {
        ///     Source = "https://www.example.com/nugetfeed",
        ///     NonInteractive = true
        /// };
        ///
        /// DotNetNuGetDelete(settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Delete")]
        public static void DotNetNuGetDelete(this ICakeContext context, DotNetNuGetDeleteSettings settings)
        {
            context.DotNetNuGetDelete(null, null, settings);
        }

        /// <summary>
        /// Deletes a package from a server using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">Name of package to delete.</param>
        /// <param name="packageVersion">Version of package to delete.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetDeleteSettings
        /// {
        ///     Source = "https://www.example.com/nugetfeed",
        ///     NonInteractive = true
        /// };
        ///
        /// DotNetNuGetDelete("Microsoft.AspNetCore.Mvc", "1.0", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Delete")]
        public static void DotNetNuGetDelete(this ICakeContext context, string packageName, string packageVersion, DotNetNuGetDeleteSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetNuGetDeleteSettings();
            }

            var nugetDeleter = new DotNetCoreNuGetDeleter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            nugetDeleter.Delete(packageName, packageVersion, settings);
        }

        /// <summary>
        /// Pushes one or more packages to a server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageFilePath"><see cref="FilePath"/> of the package to push.</param>
        /// <example>
        /// <code>
        /// // With FilePath instance
        /// var packageFilePath = GetFiles("*.nupkg").Single();
        /// DotNetNuGetPush(packageFilePath);
        /// // With string parameter
        /// DotNetNuGetPush("foo*.nupkg");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Push")]
        public static void DotNetNuGetPush(this ICakeContext context, FilePath packageFilePath)
        {
            context.DotNetNuGetPush(packageFilePath, null);
        }

        /// <summary>
        /// Pushes one or more packages to a server using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageFilePath"><see cref="FilePath"/> of the package to push.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetPushSettings
        /// {
        ///     Source = "https://www.example.com/nugetfeed",
        ///     ApiKey = "4003d786-cc37-4004-bfdf-c4f3e8ef9b3a"
        /// };
        /// // With FilePath instance
        /// var packageFilePath = GetFiles("foo*.nupkg").Single();
        /// DotNetNuGetPush(packageFilePath);
        /// // With string parameter
        /// DotNetNuGetPush("foo*.nupkg", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Push")]
        public static void DotNetNuGetPush(this ICakeContext context, FilePath packageFilePath, DotNetNuGetPushSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetNuGetPushSettings();
            }

            var restorer = new DotNetCoreNuGetPusher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            restorer.Push(packageFilePath?.FullPath, settings);
        }

        /// <summary>
        /// Add the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetSourceSettings
        /// {
        ///     Source = "https://www.example.com/nugetfeed",
        ///     UserName = "username",
        ///     Password = "password",
        ///     StorePasswordInClearText = true,
        ///     ValidAuthenticationTypes = "basic,negotiate"
        /// };
        ///
        /// DotNetNuGetAddSource("example", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetAddSource(this ICakeContext context, string name, DotNetNuGetSourceSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var sourcer = new DotNetCoreNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            sourcer.AddSource(name, settings);
        }

        /// <summary>
        /// Disable the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <example>
        /// <code>
        /// DotNetNuGetDisableSource("example");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetDisableSource(this ICakeContext context, string name)
        {
            context.DotNetNuGetDisableSource(name, null);
        }

        /// <summary>
        /// Disable the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetSourceSettings
        /// {
        ///     ConfigFile = "NuGet.config"
        /// };
        ///
        /// DotNetNuGetDisableSource("example", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetDisableSource(this ICakeContext context, string name, DotNetNuGetSourceSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var sourcer = new DotNetCoreNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            sourcer.DisableSource(name, settings ?? new DotNetNuGetSourceSettings());
        }

        /// <summary>
        /// Enable the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <example>
        /// <code>
        /// DotNetNuGetEnableSource("example");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetEnableSource(this ICakeContext context, string name)
        {
            context.DotNetNuGetEnableSource(name, null);
        }

        /// <summary>
        /// Enable the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetSourceSettings
        /// {
        ///     ConfigFile = "NuGet.config"
        /// };
        ///
        /// DotNetNuGetEnableSource("example", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetEnableSource(this ICakeContext context, string name, DotNetNuGetSourceSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var sourcer = new DotNetCoreNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            sourcer.EnableSource(name, settings ?? new DotNetNuGetSourceSettings());
        }

        /// <summary>
        /// Determines whether the specified NuGet source exists.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <returns>Whether the specified NuGet source exists.</returns>
        /// <example>
        /// <code>
        /// var exists = DotNetNuGetHasSource("example");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static bool DotNetNuGetHasSource(this ICakeContext context, string name)
        {
            return context.DotNetNuGetHasSource(name, null);
        }

        /// <summary>
        /// Determines whether the specified NuGet source exists.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Whether the specified NuGet source exists.</returns>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetSourceSettings
        /// {
        ///     ConfigFile = "NuGet.config"
        /// };
        ///
        /// var exists = DotNetNuGetHasSource("example", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static bool DotNetNuGetHasSource(this ICakeContext context, string name, DotNetNuGetSourceSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var sourcer = new DotNetCoreNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return sourcer.HasSource(name, settings ?? new DotNetNuGetSourceSettings());
        }

        /// <summary>
        /// Remove the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <example>
        /// <code>
        /// DotNetNuGetRemoveSource("example");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetRemoveSource(this ICakeContext context, string name)
        {
            context.DotNetNuGetRemoveSource(name, null);
        }

        /// <summary>
        /// Remove the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetSourceSettings
        /// {
        ///     ConfigFile = "NuGet.config"
        /// };
        ///
        /// DotNetNuGetRemoveSource("example", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetRemoveSource(this ICakeContext context, string name, DotNetNuGetSourceSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var sourcer = new DotNetCoreNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            sourcer.RemoveSource(name, settings ?? new DotNetNuGetSourceSettings());
        }

        /// <summary>
        /// Update the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetSourceSettings
        /// {
        ///     Source = "https://www.example.com/nugetfeed",
        ///     UserName = "username",
        ///     Password = "password",
        ///     StorePasswordInClearText = true,
        ///     ValidAuthenticationTypes = "basic,negotiate"
        /// };
        ///
        /// DotNetNuGetUpdateSource("example", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetUpdateSource(this ICakeContext context, string name, DotNetNuGetSourceSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var sourcer = new DotNetCoreNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            sourcer.UpdateSource(name, settings);
        }

        /// <summary>
        /// Package all projects.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The projects path.</param>
        /// <example>
        /// <code>
        /// DotNetPack("./src/*");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Pack")]
        public static void DotNetPack(this ICakeContext context, string project)
        {
            context.DotNetPack(project, null);
        }

        /// <summary>
        /// Package all projects.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The projects path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetPackSettings
        /// {
        ///     Configuration = "Release",
        ///     OutputDirectory = "./artifacts/"
        /// };
        ///
        /// DotNetPack("./src/*", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Pack")]
        public static void DotNetPack(this ICakeContext context, string project, DotNetPackSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetPackSettings();
            }

            var packer = new DotNetCorePacker(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            packer.Pack(project, settings);
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
        /// Test one or more projects specified by a path or glob pattern using the VS Test host runner.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="testFile">A path to the test file or glob for one or more test files.</param>
        /// <example>
        /// <para>Specify the path to the .csproj file in the test project.</para>
        /// <code>
        /// DotNetVSTest("./test/Project.Tests/bin/Release/netcoreapp2.1/Project.Tests.dll");
        /// </code>
        /// <para>You could also specify a glob pattern to run multiple test projects.</para>
        /// <code>
        /// DotNetVSTest("./**/bin/Release/netcoreapp2.1/*.Tests.dll");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Test")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.VSTest")]
        public static void DotNetVSTest(this ICakeContext context, GlobPattern testFile) => context.DotNetVSTest(testFile, null);

        /// <summary>
        /// Test one or more projects specified by a path or glob pattern with settings using the VS Test host runner.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="testFile">A path to the test file or glob for one or more test files.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <para>Specify the path to the .csproj file in the test project.</para>
        /// <code>
        /// var settings = new DotNetVSTestSettings
        /// {
        ///     Framework = "FrameworkCore10",
        ///     Platform = "x64"
        /// };
        ///
        /// DotNetTest("./test/Project.Tests/bin/Release/netcoreapp2.1/Project.Tests.dll", settings);
        /// </code>
        /// <para>You could also specify a glob pattern to run multiple test projects.</para>
        /// <code>
        /// var settings = new DotNetVSTestSettings
        /// {
        ///     Framework = "FrameworkCore10",
        ///     Platform = "x64",
        ///     Parallel = true
        /// };
        ///
        /// DotNetVSTest("./**/bin/Release/netcoreapp2.1/*.Tests.dll", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Test")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.VSTest")]
        public static void DotNetVSTest(this ICakeContext context, GlobPattern testFile, DotNetVSTestSettings settings)
        {
            var testFiles = context.GetFiles(testFile);

            context.DotNetVSTest(testFiles, settings);
        }

        /// <summary>
        /// Test one or more specified projects with settings using the VS Test host runner.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="testFiles">The project paths to test.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetVSTestSettings
        /// {
        ///     Framework = "FrameworkCore10",
        ///     Platform = "x64"
        /// };
        ///
        /// DotNetVSTest(new[] { (FilePath)"./test/Project.Tests/bin/Release/netcoreapp2.1/Project.Tests.dll" }, settings);
        /// </code>
        /// <para>You could also specify a task that runs multiple test projects.</para>
        /// <para>Cake task:</para>
        /// <code>
        /// Task("Test")
        ///     .Does(() =>
        /// {
        ///     var settings = new DotNetVSTestSettings
        ///     {
        ///         Framework = "FrameworkCore10",
        ///         Platform = "x64",
        ///         Parallel = true
        ///     };
        ///
        ///     var testFiles = GetFiles("./test/**/bin/Release/netcoreapp2.1/*.Test.dll");
        ///
        ///     DotNetVSTest(testFiles, settings);
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Test")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.VSTest")]
        public static void DotNetVSTest(this ICakeContext context, IEnumerable<FilePath> testFiles, DotNetVSTestSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetVSTestSettings();
            }

            var tester = new DotNetCoreVSTester(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            tester.Test(testFiles, settings);
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

        /// <summary>
        /// Formats code to match editorconfig settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <example>
        /// <code>
        /// DotNetFormat("./src/project");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormat(this ICakeContext context, string root)
        {
            context.DotNetFormat(root, null);
        }

        /// <summary>
        /// Formats code to match editorconfig settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetFormatSettings
        /// {
        ///     NoRestore = true,
        ///     Include = "Program.cs Utility\Logging.cs",
        ///     Severity = DotNetFormatSeverity.Error
        /// };
        ///
        /// DotNetFormat("./src/project", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormat(this ICakeContext context, string root, DotNetFormatSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetFormatSettings();
            }

            var formatter = new DotNetFormatter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            formatter.Format(root, null, settings);
        }

        /// <summary>
        /// Format code to match editorconfig settings for whitespace.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <example>
        /// <code>
        /// DotNetFormatWhitespace("./src/*");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormatWhitespace(this ICakeContext context, string root)
        {
            context.DotNetFormatWhitespace(root, null);
        }

        /// <summary>
        /// Format code to match editorconfig settings for whitespace.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetFormatSettings
        /// {
        ///     NoRestore = true,
        ///     Include = "Program.cs Utility\Logging.cs"
        /// };
        ///
        /// DotNetFormatWhitespace("./src/*", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormatWhitespace(this ICakeContext context, string root, DotNetFormatSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotNetFormatSettings();
            }

            var formatter = new DotNetFormatter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            formatter.Format(root, "whitespace", settings);
        }

        /// <summary>
        /// Format code to match editorconfig settings for code style.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <example>
        /// <code>
        /// DotNetFormatStyle("./src/*");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormatStyle(this ICakeContext context, string root)
        {
            context.DotNetFormatStyle(root, null);
        }

        /// <summary>
        /// Format code to match editorconfig settings for code style.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetFormatSettings
        /// {
        ///     NoRestore = true,
        ///     Include = "Program.cs Utility\Logging.cs"
        /// };
        ///
        /// DotNetFormatStyle("./src/*", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormatStyle(this ICakeContext context, string root, DotNetFormatSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotNetFormatSettings();
            }

            var formatter = new DotNetFormatter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            formatter.Format(root, "style", settings);
        }

        /// <summary>
        /// Format code to match editorconfig settings for analyzers.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project or solution path.</param>
        /// <example>
        /// <code>
        /// DotNetFormatAnalyzers("./src/*");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormatAnalyzers(this ICakeContext context, string project)
        {
            context.DotNetFormatAnalyzers(project, null);
        }

        /// <summary>
        /// Format code to match editorconfig settings for analyzers.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetFormatSettings
        /// {
        ///     NoRestore = true,
        ///     Include = "Program.cs Utility\Logging.cs"
        /// };
        ///
        /// DotNetFormatAnalyzers("./src/*", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormatAnalyzers(this ICakeContext context, string root, DotNetFormatSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotNetFormatSettings();
            }

            var formatter = new DotNetFormatter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            formatter.Format(root, "analyzers", settings);
        }

        /// <summary>
        /// Lists the latest available version of the .NET SDK and .NET Runtime.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetSDKCheck();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("SDK")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.SDKCheck")]
        public static void DotNetSDKCheck(this ICakeContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var checker = new DotNetSDKChecker(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            checker.Check();
        }

        /// <summary>
        /// Lists available workloads.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The list of available workloads.</returns>
        /// <example>
        /// <code>
        /// var workloads = DotNetWorkloadSearch();
        ///
        /// foreach (var workload in workloads)
        /// {
        ///      Information($"Id: {workload.Id}, Description: {workload.Description}");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Search")]
        public static IEnumerable<DotNetWorkload> DotNetWorkloadSearch(this ICakeContext context)
        {
            return context.DotNetWorkloadSearch(null);
        }

        /// <summary>
        /// Lists available workloads by specifying all or part of the workload ID.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="searchString">The workload ID to search for, or part of it.</param>
        /// <returns>The list of available workloads.</returns>
        /// <example>
        /// <code>
        /// var workloads = DotNetWorkloadSearch("maui");
        ///
        /// foreach (var workload in workloads)
        /// {
        ///      Information($"Id: {workload.Id}, Description: {workload.Description}");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Search")]
        public static IEnumerable<DotNetWorkload> DotNetWorkloadSearch(this ICakeContext context, string searchString)
        {
            return context.DotNetWorkloadSearch(searchString, null);
        }

        /// <summary>
        /// Lists available workloads by specifying all or part of the workload ID.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="searchString">The workload ID to search for, or part of it.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The list of available workloads.</returns>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadSearchSettings
        /// {
        ///     Verbosity = Detailed
        /// };
        ///
        /// var workloads = DotNetWorkloadSearch("maui", settings);
        ///
        /// foreach (var workload in workloads)
        /// {
        ///      Information($"Id: {workload.Id}, Description: {workload.Description}");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Search")]
        public static IEnumerable<DotNetWorkload> DotNetWorkloadSearch(this ICakeContext context, string searchString, DotNetWorkloadSearchSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotNetWorkloadSearchSettings();
            }

            var searcher = new DotNetWorkloadSearcher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return searcher.Search(searchString, settings);
        }
    }
}
