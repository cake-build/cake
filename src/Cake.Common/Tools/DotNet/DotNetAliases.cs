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
using Cake.Common.Tools.DotNet.Package.Add;
using Cake.Common.Tools.DotNet.Package.List;
using Cake.Common.Tools.DotNet.Package.Remove;
using Cake.Common.Tools.DotNet.Package.Search;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Common.Tools.DotNet.Reference.Add;
using Cake.Common.Tools.DotNet.Reference.List;
using Cake.Common.Tools.DotNet.Reference.Remove;
using Cake.Common.Tools.DotNet.Restore;
using Cake.Common.Tools.DotNet.Run;
using Cake.Common.Tools.DotNet.SDKCheck;
using Cake.Common.Tools.DotNet.Sln.List;
using Cake.Common.Tools.DotNet.Test;
using Cake.Common.Tools.DotNet.Tool;
using Cake.Common.Tools.DotNet.VSTest;
using Cake.Common.Tools.DotNet.Workload.Install;
using Cake.Common.Tools.DotNet.Workload.List;
using Cake.Common.Tools.DotNet.Workload.Repair;
using Cake.Common.Tools.DotNet.Workload.Restore;
using Cake.Common.Tools.DotNet.Workload.Search;
using Cake.Common.Tools.DotNet.Workload.Uninstall;
using Cake.Common.Tools.DotNet.Workload.Update;
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

            var executor = new DotNetExecutor(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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
        ///     DotNetVerbosity.Information,
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
        ///     DotNetVerbosity.Information,
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

            var restorer = new DotNetRestorer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, context.Log);
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

            var builder = new DotNetBuilder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var publisher = new DotNetPublisher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var tester = new DotNetTester(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var cleaner = new DotNetCleaner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var nugetDeleter = new DotNetNuGetDeleter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var restorer = new DotNetNuGetPusher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var sourcer = new DotNetNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var sourcer = new DotNetNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var sourcer = new DotNetNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var sourcer = new DotNetNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var sourcer = new DotNetNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var sourcer = new DotNetNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var packer = new DotNetPacker(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var runner = new DotNetRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var builder = new DotNetMSBuildBuilder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var tester = new DotNetVSTester(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
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

            var runner = new DotNetToolRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

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

            var buildServer = new DotNetBuildServer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

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
        ///     DotNetVerbosity.Detailed
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

        /// <summary>
        /// Uninstalls a specified workload.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="workloadId">The workload ID to uninstall.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadUninstall("maui");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Uninstall")]
        public static void DotNetWorkloadUninstall(this ICakeContext context, string workloadId)
        {
            context.DotNetWorkloadUninstall(new string[] { workloadId });
        }

        /// <summary>
        /// Uninstalls one or more workloads.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="workloadIds">The workload ID or multiple IDs to uninstall.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadUninstall(new string[] { "maui", "maui-desktop", "maui-mobile" });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Uninstall")]
        public static void DotNetWorkloadUninstall(this ICakeContext context, IEnumerable<string> workloadIds)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var uninstaller = new DotNetWorkloadUninstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            uninstaller.Uninstall(workloadIds);
        }

        /// <summary>
        /// Installs a specified optional workload.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="workloadId">The workload ID to install.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadInstall("maui");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Install")]
        public static void DotNetWorkloadInstall(this ICakeContext context, string workloadId)
        {
            context.DotNetWorkloadInstall(workloadId, null);
        }

        /// <summary>
        /// Installs a specified optional workload.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="workloadId">The workload ID to install.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadInstallSettings
        /// {
        ///     IncludePreviews = true,
        ///     NoCache = true
        /// };
        ///
        /// DotNetWorkloadInstall("maui", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Install")]
        public static void DotNetWorkloadInstall(this ICakeContext context, string workloadId, DotNetWorkloadInstallSettings settings)
        {
            context.DotNetWorkloadInstall(new string[] { workloadId }, settings);
        }

        /// <summary>
        /// Installs one or more optional workloads.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="workloadIds">The workload ID or multiple IDs to install.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadInstall(new string[] { "maui", "maui-desktop", "maui-mobile" });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Install")]
        public static void DotNetWorkloadInstall(this ICakeContext context, IEnumerable<string> workloadIds)
        {
            context.DotNetWorkloadInstall(workloadIds, null);
        }

        /// <summary>
        /// Installs one or more optional workloads.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="workloadIds">The workload ID or multiple IDs to install.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadInstallSettings
        /// {
        ///     IncludePreviews = true,
        ///     NoCache = true
        /// };
        ///
        /// DotNetWorkloadInstall(new string[] { "maui", "maui-desktop", "maui-mobile" }, settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Install")]
        public static void DotNetWorkloadInstall(this ICakeContext context, IEnumerable<string> workloadIds, DotNetWorkloadInstallSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotNetWorkloadInstallSettings();
            }

            var installer = new DotNetWorkloadInstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            installer.Install(workloadIds, settings);
        }

        /// <summary>
        /// Lists all installed workloads.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The list of installed workloads.</returns>
        /// <example>
        /// <code>
        /// var workloadIds = DotNetWorkloadList();
        ///
        /// foreach (var workloadId in workloadIds)
        /// {
        ///      Information($"Installed Workload Id: {workloadId}");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.List")]
        public static IEnumerable<DotNetWorkloadListItem> DotNetWorkloadList(this ICakeContext context)
        {
            return context.DotNetWorkloadList(null);
        }

        /// <summary>
        /// Lists all installed workloads.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The list of installed workloads.</returns>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadListSettings
        /// {
        ///     Verbosity = DotNetVerbosity.Detailed
        /// };
        ///
        /// var workloads = DotNetWorkloadList(settings);
        ///
        /// foreach (var workload in workloads)
        /// {
        ///      Information($"Installed Workload Id: {workload.Id}\t Manifest Version: {workload.ManifestVersion}\t Installation Source: {workload.InstallationSource}");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.List")]
        public static IEnumerable<DotNetWorkloadListItem> DotNetWorkloadList(this ICakeContext context, DotNetWorkloadListSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotNetWorkloadListSettings();
            }

            var lister = new DotNetWorkloadLister(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return lister.List(settings);
        }

        /// <summary>
        /// Repairs all workloads installations.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadRepair();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Repair")]
        public static void DotNetWorkloadRepair(this ICakeContext context)
        {
            context.DotNetWorkloadRepair(null);
        }

        /// <summary>
        /// Repairs all workloads installations.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadRepairSettings
        /// {
        ///     IncludePreviews = true,
        ///     NoCache = true
        /// };
        ///
        /// DotNetWorkloadRepair(settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Repair")]
        public static void DotNetWorkloadRepair(this ICakeContext context, DotNetWorkloadRepairSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotNetWorkloadRepairSettings();
            }

            var repairer = new DotNetWorkloadRepairer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            repairer.Repair(settings);
        }

        /// <summary>
        /// Updates all installed workloads to the newest available version.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadUpdate();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Update")]
        public static void DotNetWorkloadUpdate(this ICakeContext context)
        {
            context.DotNetWorkloadUpdate(null);
        }

        /// <summary>
        /// Updates all installed workloads to the newest available version.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadUpdateSettings
        /// {
        ///     IncludePreviews = true,
        ///     NoCache = true
        /// };
        ///
        /// DotNetWorkloadUpdate(settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Update")]
        public static void DotNetWorkloadUpdate(this ICakeContext context, DotNetWorkloadUpdateSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotNetWorkloadUpdateSettings();
            }

            var updater = new DotNetWorkloadUpdater(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            updater.Update(settings);
        }

        /// <summary>
        /// Installs workloads needed for a project or a solution.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project or solution file to install workloads for.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadRestore("./src/project");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Restore")]
        public static void DotNetWorkloadRestore(this ICakeContext context, string project)
        {
            context.DotNetWorkloadRestore(project, null);
        }

        /// <summary>
        /// Installs workloads needed for a project or a solution.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project or solution file to install workloads for.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadRestoreSettings
        /// {
        ///     IncludePreviews = true,
        ///     NoCache = true
        /// };
        ///
        /// DotNetWorkloadRestore("./src/project", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Restore")]
        public static void DotNetWorkloadRestore(this ICakeContext context, string project, DotNetWorkloadRestoreSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetWorkloadRestoreSettings();
            }

            var restorer = new DotNetWorkloadRestorer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            restorer.Restore(project, settings);
        }

        /// <summary>
        /// Adds or updates a package reference in a project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">The package reference to add.</param>
        /// <example>
        /// <code>
        /// DotNetAddPackage("Cake.FileHelper");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Package")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Add")]
        public static void DotNetAddPackage(this ICakeContext context, string packageName)
        {
            context.DotNetAddPackage(packageName, null, null);
        }

        /// <summary>
        /// Adds or updates a package reference in a project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">The package reference to add.</param>
        /// <param name="project">The target project file path.</param>
        /// <example>
        /// <code>
        /// DotNetAddPackage("Cake.FileHelper", "ToDo.csproj");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Package")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Add")]
        public static void DotNetAddPackage(this ICakeContext context, string packageName, string project)
        {
            context.DotNetAddPackage(packageName, project, null);
        }

        /// <summary>
        /// Adds or updates a package reference in a project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">The package reference to add.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetPackageAddSettings
        /// {
        ///     NoRestore = true,
        ///     Version = "6.1.3"
        /// };
        ///
        /// DotNetAddPackage("Cake.FileHelper", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Package")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Add")]
        public static void DotNetAddPackage(this ICakeContext context, string packageName, DotNetPackageAddSettings settings)
        {
            context.DotNetAddPackage(packageName, null, settings);
        }

        /// <summary>
        /// Adds or updates a package reference in a project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">The package reference to add.</param>
        /// <param name="project">The target project file path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetPackageAddSettings
        /// {
        ///     NoRestore = true,
        ///     Version = "6.1.3"
        /// };
        ///
        /// DotNetAddPackage("Cake.FileHelper", "ToDo.csproj", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Package")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Add")]
        public static void DotNetAddPackage(this ICakeContext context, string packageName, string project, DotNetPackageAddSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetPackageAddSettings();
            }

            var adder = new DotNetPackageAdder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            adder.Add(packageName, project, settings);
        }

        /// <summary>
        /// Removes package reference from a project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">The package reference to remove.</param>
        /// <example>
        /// <code>
        /// DotNetRemovePackage("Cake.FileHelper");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Package")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Remove")]
        public static void DotNetRemovePackage(this ICakeContext context, string packageName)
        {
            context.DotNetRemovePackage(packageName, null);
        }

        /// <summary>
        /// Removes package reference from a project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">The package reference to remove.</param>
        /// <param name="project">The target project file path.</param>
        /// <example>
        /// <code>
        /// DotNetRemovePackage("Cake.FileHelper", "ToDo.csproj");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Package")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Remove")]
        public static void DotNetRemovePackage(this ICakeContext context, string packageName, string project)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var adder = new DotNetPackageRemover(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            adder.Remove(packageName, project);
        }

        /// <summary>
        /// Adds project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectReferences">One or more project references to add. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <example>
        /// <code>
        /// DotNetAddReference(GetFiles("./src/*.csproj"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Add")]
        public static void DotNetAddReference(this ICakeContext context, IEnumerable<FilePath> projectReferences)
        {
            context.DotNetAddReference(projectReferences, null);
        }

        /// <summary>
        /// Adds project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectReferences">One or more project references to add. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetReferenceAddSettings
        /// {
        ///     Framework = "net8.0"
        /// };
        ///
        /// DotNetAddReference(GetFiles("./src/*.csproj"), settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Add")]
        public static void DotNetAddReference(this ICakeContext context, IEnumerable<FilePath> projectReferences, DotNetReferenceAddSettings settings)
        {
            context.DotNetAddReference(null, projectReferences, settings);
        }

        /// <summary>
        /// Adds project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The target project file path. If not specified, the command searches the current directory for one.</param>
        /// <param name="projectReferences">One or more project references to add. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <example>
        /// <code>
        /// DotNetAddReference("./app/app.csproj", GetFiles("./src/*.csproj"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Add")]
        public static void DotNetAddReference(this ICakeContext context, string project, IEnumerable<FilePath> projectReferences)
        {
            context.DotNetAddReference(project, projectReferences, null);
        }

        /// <summary>
        /// Adds project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The target project file path. If not specified, the command searches the current directory for one.</param>
        /// <param name="projectReferences">One or more project references to add. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetReferenceAddSettings
        /// {
        ///     Framework = "net8.0"
        /// };
        ///
        /// DotNetAddReference("./app/app.csproj", GetFiles("./src/*.csproj"), settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Add")]
        public static void DotNetAddReference(this ICakeContext context, string project, IEnumerable<FilePath> projectReferences, DotNetReferenceAddSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetReferenceAddSettings();
            }

            var adder = new DotNetReferenceAdder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            adder.Add(project, projectReferences, settings);
        }

        /// <summary>
        /// Removes project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectReferences">Project-to-project (P2P) references to remove. You can specify one or multiple projects. Glob patterns are supported on Unix/Linux based terminals.</param>
        /// <example>
        /// <code>
        /// DotNetRemoveReference(GetFiles("./src/*.csproj"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Remove")]
        public static void DotNetRemoveReference(this ICakeContext context, IEnumerable<FilePath> projectReferences)
        {
            context.DotNetRemoveReference(projectReferences, null);
        }

        /// <summary>
        /// Removes project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectReferences">Project-to-project (P2P) references to remove. You can specify one or multiple projects. Glob patterns are supported on Unix/Linux based terminals.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetReferenceRemoveSettings
        /// {
        ///     Framework = "net8.0"
        /// };
        ///
        /// DotNetRemoveReference(GetFiles("./src/*.csproj"), settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Remove")]
        public static void DotNetRemoveReference(this ICakeContext context, IEnumerable<FilePath> projectReferences, DotNetReferenceRemoveSettings settings)
        {
            context.DotNetRemoveReference(null, projectReferences, settings);
        }

        /// <summary>
        /// Removes project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">Target project file. If not specified, the command searches the current directory for one.</param>
        /// <param name="projectReferences">Project-to-project (P2P) references to remove. You can specify one or multiple projects. Glob patterns are supported on Unix/Linux based terminals.</param>
        /// <example>
        /// <code>
        /// DotNetRemoveReference("./app/app.csproj", GetFiles("./src/*.csproj"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Remove")]
        public static void DotNetRemoveReference(this ICakeContext context, string project, IEnumerable<FilePath> projectReferences)
        {
            context.DotNetRemoveReference(project, projectReferences, null);
        }

        /// <summary>
        /// Removes project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">Target project file. If not specified, the command searches the current directory for one.</param>
        /// <param name="projectReferences">Project-to-project (P2P) references to remove. You can specify one or multiple projects. Glob patterns are supported on Unix/Linux based terminals.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetReferenceRemoveSettings
        /// {
        ///     Framework = "net8.0"
        /// };
        ///
        /// DotNetRemoveReference("./app/app.csproj", GetFiles("./src/*.csproj"), settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Remove")]
        public static void DotNetRemoveReference(this ICakeContext context, string project, IEnumerable<FilePath> projectReferences, DotNetReferenceRemoveSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetReferenceRemoveSettings();
            }

            var remover = new DotNetReferenceRemover(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            remover.Remove(project, projectReferences, settings);
        }

        /// <summary>
        /// Lists project-to-project references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The list of project-to-project references.</returns>
        /// <example>
        /// <code>
        /// var references = DotNetListReference();
        ///
        /// foreach (var reference in references)
        /// {
        ///      Information(reference);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.List")]
        public static IEnumerable<string> DotNetListReference(this ICakeContext context)
        {
            return context.DotNetListReference(null);
        }

        /// <summary>
        /// Lists project-to-project references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project file to operate on. If a file is not specified, the command will search the current directory for one.</param>
        /// <returns>The list of project-to-project references.</returns>
        /// <example>
        /// <code>
        /// var references = DotNetListReference("./app/app.csproj");
        ///
        /// foreach (var reference in references)
        /// {
        ///      Information(reference);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.List")]
        public static IEnumerable<string> DotNetListReference(this ICakeContext context, string project)
        {
            return context.DotNetListReference(project, null);
        }

        /// <summary>
        /// Lists project-to-project references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project file to operate on. If a file is not specified, the command will search the current directory for one.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The list of project-to-project references.</returns>
        /// <example>
        /// <code>
        /// var settings = new DotNetReferenceListSettings
        /// {
        ///     Verbosity = DotNetVerbosity.Diagnostic
        /// };
        ///
        /// var references = DotNetListReference("./app/app.csproj", settings);
        ///
        /// foreach (var reference in references)
        /// {
        ///      Information(reference);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.List")]
        public static IEnumerable<string> DotNetListReference(this ICakeContext context, string project, DotNetReferenceListSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetReferenceListSettings();
            }

            var lister = new DotNetReferenceLister(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return lister.List(project, settings);
        }

        /// <summary>
        /// List packages on available from source using specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>List of packages with their version.</returns>
        /// <example>
        /// <code>
        /// var packageList = DotNetPackageSearch("Cake", new DotNetPackageSearchSettings {
        ///     AllVersions = false,
        ///     Prerelease = false
        ///     });
        /// foreach(var package in packageList)
        /// {
        ///     Information("Found package {0}, version {1}", package.Name, package.Version);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Package")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Search")]
        public static IEnumerable<DotNetPackageSearchItem> DotNetSearchPackage(this ICakeContext context, string searchTerm, DotNetPackageSearchSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var runner = new DotNetPackageSearcher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return runner.Search(searchTerm, settings);
        }

        /// <summary>
        /// List packages on available from source using specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="searchTerm">The package Id.</param>
        /// <returns>List of packages with their version.</returns>
        /// <example>
        /// <code>
        /// var packageList = DotNetPackageSearch("Cake", new DotNetPackageSearchSettings {
        ///     AllVersions = false,
        ///     Prerelease = false
        ///     });
        /// foreach(var package in packageList)
        /// {
        ///     Information("Found package {0}, version {1}", package.Name, package.Version);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Package")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Search")]
        public static IEnumerable<DotNetPackageSearchItem> DotNetSearchPackage(this ICakeContext context, string searchTerm)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var runner = new DotNetPackageSearcher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return runner.Search(searchTerm, new DotNetPackageSearchSettings());
        }

        /// <summary>
        /// List packages on available from source using specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>List of packages with their version.</returns>
        /// <example>
        /// <code>
        /// var packageList = DotNetPackageSearch("Cake", new DotNetPackageSearchSettings {
        ///     AllVersions = false,
        ///     Prerelease = false
        ///     });
        /// foreach(var package in packageList)
        /// {
        ///     Information("Found package {0}, version {1}", package.Name, package.Version);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Package")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Search")]
        public static IEnumerable<DotNetPackageSearchItem> DotNetSearchPackage(this ICakeContext context, DotNetPackageSearchSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var runner = new DotNetPackageSearcher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return runner.Search(null, settings);
        }

        /// <summary>
        /// Lists the package references for a project or solution.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The the package references.</returns>
        /// <example>
        /// <code>
        /// DotNetPackageList output = DotNetListPackage();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Package")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.List")]
        public static DotNetPackageList DotNetListPackage(this ICakeContext context)
        {
            return context.DotNetListPackage(null);
        }

        /// <summary>
        /// Lists the package references for a project or solution.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project or solution file to operate on. If not specified, the command searches the current directory for one. If more than one solution or project is found, an error is thrown.</param>
        /// <returns>The the package references.</returns>
        /// <example>
        /// <code>
        /// DotNetPackageList output = DotNetListPackage("./src/MyProject/MyProject.csproj");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Package")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.List")]
        public static DotNetPackageList DotNetListPackage(this ICakeContext context, string project)
        {
            return context.DotNetListPackage(project, null);
        }

        /// <summary>
        /// Lists the package references for a project or solution.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project or solution file to operate on. If not specified, the command searches the current directory for one. If more than one solution or project is found, an error is thrown.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The the package references.</returns>
        /// <example>
        /// <code>
        /// var settings = new DotNetPackageListSettings
        /// {
        ///     Outdated = true
        /// };
        ///
        /// DotNetPackageList output = DotNetListPackage("./src/MyProject/MyProject.csproj", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Package")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.List")]
        public static DotNetPackageList DotNetListPackage(this ICakeContext context, string project, DotNetPackageListSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetPackageListSettings();
            }

            var lister = new DotNetPackageLister(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return lister.List(project, settings);
        }

        /// <summary>
        /// Lists all projects in a solution file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The list of projects.</returns>
        /// <example>
        /// <code>
        /// var projects = DotNetSlnList();
        ///
        /// foreach (var project in projects)
        /// {
        ///      Information(project);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Sln")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Sln.List")]
        public static IEnumerable<string> DotNetSlnList(this ICakeContext context)
        {
            return context.DotNetSlnList(null);
        }

        /// <summary>
        /// Lists all projects in a solution file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution file to use. If this argument is omitted, the command searches the current directory for one. If it finds no solution file or multiple solution files, the command fails.</param>
        /// <returns>The list of projects.</returns>
        /// <example>
        /// <code>
        /// var projects = DotNetSlnList("./app/app.sln");
        ///
        /// foreach (var project in projects)
        /// {
        ///      Information(project);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Sln")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Sln.List")]
        public static IEnumerable<string> DotNetSlnList(this ICakeContext context, FilePath solution)
        {
            return context.DotNetSlnList(solution, null);
        }

        /// <summary>
        /// Lists all projects in a solution file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution file to use. If this argument is omitted, the command searches the current directory for one. If it finds no solution file or multiple solution files, the command fails.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The list of projects.</returns>
        /// <example>
        /// <code>
        /// var settings = new DotNetSlnListSettings
        /// {
        ///     Verbosity = DotNetVerbosity.Diagnostic
        /// };
        ///
        /// var projects = DotNetSlnList("./app/app.sln");
        ///
        /// foreach (var project in projects)
        /// {
        ///      Information(project);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Sln")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Sln.List")]
        public static IEnumerable<string> DotNetSlnList(this ICakeContext context, FilePath solution, DotNetSlnListSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetSlnListSettings();
            }

            var lister = new DotNetSlnLister(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return lister.List(solution, settings);
        }
    }
}
