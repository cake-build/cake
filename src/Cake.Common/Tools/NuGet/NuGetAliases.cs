// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Common.Tools.NuGet.Install;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Common.Tools.NuGet.Push;
using Cake.Common.Tools.NuGet.Restore;
using Cake.Common.Tools.NuGet.SetApiKey;
using Cake.Common.Tools.NuGet.SetProxy;
using Cake.Common.Tools.NuGet.Sources;
using Cake.Common.Tools.NuGet.Update;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;

namespace Cake.Common.Tools.NuGet
{
    /// <summary>
    /// Contains functionality for working with <see href="https://www.nuget.org/">NuGet</see>.
    /// </summary>
    /// <para>
    /// Since Cake requires NuGet to be available very early in the build pipeline, we recommend that NuGet is made
    /// available via the <see href="https://github.com/cake-build/resources">Cake BootStrapper</see>.
    /// </para>
    [CakeAliasCategory("NuGet")]
    public static class NuGetAliases
    {
        /// <summary>
        /// Creates a NuGet package using the specified Nuspec or project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The nuspec or project file path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var nuGetPackSettings   = new NuGetPackSettings {
        ///                                     Id                      = "TestNuget",
        ///                                     Version                 = "0.0.0.1",
        ///                                     Title                   = "The tile of the package",
        ///                                     Authors                 = new[] {"John Doe"},
        ///                                     Owners                  = new[] {"Contoso"},
        ///                                     Description             = "The description of the package",
        ///                                     Summary                 = "Excellent summary of what the package does",
        ///                                     ProjectUrl              = new Uri("https://github.com/SomeUser/TestNuget/"),
        ///                                     IconUrl                 = new Uri("http://cdn.rawgit.com/SomeUser/TestNuget/master/icons/testnuget.png"),
        ///                                     LicenseUrl              = new Uri("https://github.com/SomeUser/TestNuget/blob/master/LICENSE.md"),
        ///                                     Copyright               = "Some company 2015",
        ///                                     ReleaseNotes            = new [] {"Bug fixes", "Issue fixes", "Typos"},
        ///                                     Tags                    = new [] {"Cake", "Script", "Build"},
        ///                                     RequireLicenseAcceptance= false,
        ///                                     Symbols                 = false,
        ///                                     NoPackageAnalysis       = true,
        ///                                     Files                   = new [] {
        ///                                                                          new NuSpecContent {Source = "bin/TestNuget.dll", Target = "bin"},
        ///                                                                       },
        ///                                     BasePath                = "./src/TestNuget/bin/release",
        ///                                     OutputDirectory         = "./nuget"
        ///                                 };
        ///
        ///     NuGetPack("./nuspec/TestNuget.nuspec", nuGetPackSettings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Pack")]
        public static void NuGetPack(this ICakeContext context, FilePath filePath, NuGetPackSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new NuGetToolResolver(context.FileSystem, context.Environment, context.Tools);
            var packer = new NuGetPacker(context.FileSystem, context.Environment, context.ProcessRunner, context.Log, context.Tools, resolver);
            packer.Pack(filePath, settings);
        }

        /// <summary>
        /// Creates NuGet packages using the specified Nuspec or project files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The nuspec or project file paths.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var nuGetPackSettings   = new NuGetPackSettings {
        ///                                     Id                      = "TestNuget",
        ///                                     Version                 = "0.0.0.1",
        ///                                     Title                   = "The tile of the package",
        ///                                     Authors                 = new[] {"John Doe"},
        ///                                     Owners                  = new[] {"Contoso"},
        ///                                     Description             = "The description of the package",
        ///                                     Summary                 = "Excellent summary of what the package does",
        ///                                     ProjectUrl              = new Uri("https://github.com/SomeUser/TestNuget/"),
        ///                                     IconUrl                 = new Uri("http://cdn.rawgit.com/SomeUser/TestNuget/master/icons/testnuget.png"),
        ///                                     LicenseUrl              = new Uri("https://github.com/SomeUser/TestNuget/blob/master/LICENSE.md"),
        ///                                     Copyright               = "Some company 2015",
        ///                                     ReleaseNotes            = new [] {"Bug fixes", "Issue fixes", "Typos"},
        ///                                     Tags                    = new [] {"Cake", "Script", "Build"},
        ///                                     RequireLicenseAcceptance= false,
        ///                                     Symbols                 = false,
        ///                                     NoPackageAnalysis       = true,
        ///                                     Files                   = new [] {
        ///                                                                          new NuSpecContent {Source = "bin/TestNuget.dll", Target = "bin"},
        ///                                                                       },
        ///                                     BasePath                = "./src/TestNuget/bin/release",
        ///                                     OutputDirectory         = "./nuget"
        ///                                 };
        ///
        ///     var nuspecFiles = GetFiles("./**/*.nuspec");
        ///     NuGetPack(nuspecFiles, nuGetPackSettings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Pack")]
        public static void NuGetPack(this ICakeContext context, IEnumerable<FilePath> filePaths, NuGetPackSettings settings)
        {
            if (filePaths == null)
            {
                throw new ArgumentNullException("filePaths");
            }

            foreach (var filePath in filePaths)
            {
                NuGetPack(context, filePath, settings);
            }
        }

        /// <summary>
        /// Creates a NuGet package using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var nuGetPackSettings   = new NuGetPackSettings {
        ///                                     Id                      = "TestNuget",
        ///                                     Version                 = "0.0.0.1",
        ///                                     Title                   = "The tile of the package",
        ///                                     Authors                 = new[] {"John Doe"},
        ///                                     Owners                  = new[] {"Contoso"},
        ///                                     Description             = "The description of the package",
        ///                                     Summary                 = "Excellent summary of what the package does",
        ///                                     ProjectUrl              = new Uri("https://github.com/SomeUser/TestNuget/"),
        ///                                     IconUrl                 = new Uri("http://cdn.rawgit.com/SomeUser/TestNuget/master/icons/testnuget.png"),
        ///                                     LicenseUrl              = new Uri("https://github.com/SomeUser/TestNuget/blob/master/LICENSE.md"),
        ///                                     Copyright               = "Some company 2015",
        ///                                     ReleaseNotes            = new [] {"Bug fixes", "Issue fixes", "Typos"},
        ///                                     Tags                    = new [] {"Cake", "Script", "Build"},
        ///                                     RequireLicenseAcceptance= false,
        ///                                     Symbols                 = false,
        ///                                     NoPackageAnalysis       = true,
        ///                                     Files                   = new [] {
        ///                                                                          new NuSpecContent {Source = "bin/TestNuget.dll", Target = "bin"},
        ///                                                                       },
        ///                                     BasePath                = "./src/TestNuget/bin/release",
        ///                                     OutputDirectory         = "./nuget"
        ///                                 };
        ///
        ///     NuGetPack(nuGetPackSettings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Pack")]
        public static void NuGetPack(this ICakeContext context, NuGetPackSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new NuGetToolResolver(context.FileSystem, context.Environment, context.Tools);
            var packer = new NuGetPacker(context.FileSystem, context.Environment, context.ProcessRunner, context.Log, context.Tools, resolver);
            packer.Pack(settings);
        }

        /// <summary>
        /// Restores NuGet packages for the specified target.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="targetFilePath">The target to restore.</param>
        /// <example>
        /// <code>
        ///     var solutions = GetFiles("./**/*.sln");
        ///     // Restore all NuGet packages.
        ///     foreach(var solution in solutions)
        ///     {
        ///         Information("Restoring {0}", solution);
        ///         NuGetRestore(solution);
        ///     }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Restore")]
        public static void NuGetRestore(this ICakeContext context, FilePath targetFilePath)
        {
            var settings = new NuGetRestoreSettings();
            NuGetRestore(context, targetFilePath, settings);
        }

        /// <summary>
        /// Restores NuGet packages for the specified targets.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="targetFilePaths">The targets to restore.</param>
        /// <example>
        /// <code>
        ///     var solutions = GetFiles("./**/*.sln");
        ///     NuGetRestore(solutions);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Restore")]
        public static void NuGetRestore(this ICakeContext context, IEnumerable<FilePath> targetFilePaths)
        {
            var settings = new NuGetRestoreSettings();
            NuGetRestore(context, targetFilePaths, settings);
        }

        /// <summary>
        /// Restores NuGet packages using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="targetFilePath">The target to restore.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var solutions = GetFiles("./**/*.sln");
        ///     // Restore all NuGet packages.
        ///     foreach(var solution in solutions)
        ///     {
        ///         Information("Restoring {0}", solution);
        ///         NuGetRestore(solution, new NuGetRestoreSettings { NoCache = true });
        ///     }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Restore")]
        public static void NuGetRestore(this ICakeContext context, FilePath targetFilePath, NuGetRestoreSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new NuGetToolResolver(context.FileSystem, context.Environment, context.Tools);
            var runner = new NuGetRestorer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.Restore(targetFilePath, settings);
        }

        /// <summary>
        /// Restores NuGet packages using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="targetFilePaths">The targets to restore.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var solutions = GetFiles("./**/*.sln");
        ///     NuGetRestore(solutions, new NuGetRestoreSettings { NoCache = true });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Restore")]
        public static void NuGetRestore(this ICakeContext context, IEnumerable<FilePath> targetFilePaths, NuGetRestoreSettings settings)
        {
            if (targetFilePaths == null)
            {
                throw new ArgumentNullException("context");
            }

            foreach (var targetFilePath in targetFilePaths)
            {
                NuGetRestore(context, targetFilePath, settings);
            }
        }

        /// <summary>
        /// Pushes a NuGet package to a NuGet server and publishes it.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageFilePath">The <c>.nupkg</c> file path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// // Get the path to the package.
        /// var package = "./nuget/SlackPRTGCommander.0.0.1.nupkg";
        ///
        /// // Push the package.
        /// NuGetPush(package, new NuGetPushSettings {
        ///     Source = "http://example.com/nugetfeed",
        ///     ApiKey = "4003d786-cc37-4004-bfdf-c4f3e8ef9b3a"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Push")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Push")]
        public static void NuGetPush(this ICakeContext context, FilePath packageFilePath, NuGetPushSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new NuGetToolResolver(context.FileSystem, context.Environment, context.Tools);
            var packer = new NuGetPusher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            packer.Push(packageFilePath, settings);
        }

        /// <summary>
        /// Pushes NuGet packages to a NuGet server and publishes them.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageFilePaths">The <c>.nupkg</c> file paths.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// // Get the paths to the packages.
        /// var packages = GetFiles("./**/*.nupkg");
        ///
        /// // Push the package.
        /// NuGetPush(packages, new NuGetPushSettings {
        ///     Source = "http://example.com/nugetfeed",
        ///     ApiKey = "4003d786-cc37-4004-bfdf-c4f3e8ef9b3a"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Push")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Push")]
        public static void NuGetPush(this ICakeContext context, IEnumerable<FilePath> packageFilePaths, NuGetPushSettings settings)
        {
            if (packageFilePaths == null)
            {
                throw new ArgumentNullException("packageFilePaths");
            }

            foreach (var packageFilePath in packageFilePaths)
            {
                NuGetPush(context, packageFilePath, settings);
            }
        }

        /// <summary>
        /// Adds NuGet package source using the specified name &amp;source to global user config
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <example>
        /// <code>
        /// var feed = new
        ///             {
        ///                 Name = EnvironmentVariable("PUBLIC_FEED_NAME"),
        ///                 Source = EnvironmentVariable("PUBLIC_FEED_SOURCE")
        ///             };
        ///
        /// NuGetAddSource(
        ///     name:feed.Name,
        ///     source:feed.Source
        /// );
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("AddSource")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Sources")]
        public static void NuGetAddSource(this ICakeContext context, string name, string source)
        {
            context.NuGetAddSource(name, source, new NuGetSourcesSettings());
        }

        /// <summary>
        /// Adds NuGet package source using the specified name, source &amp; settings to global user config
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var nugetSourceSettings = new NuGetSourcesSettings
        ///                             {
        ///                                 UserName = EnvironmentVariable("PRIVATE_FEED_USERNAME"),
        ///                                 Password = EnvironmentVariable("PRIVATE_FEED_PASSWORD"),
        ///                                 IsSensitiveSource = true,
        ///                                 Verbosity = NuGetVerbosity.Detailed
        ///                             };
        ///
        /// var feed = new
        ///             {
        ///                 Name = EnvironmentVariable("PRIVATE_FEED_NAME"),
        ///                 Source = EnvironmentVariable("PRIVATE_FEED_SOURCE")
        ///             };
        ///
        /// NuGetAddSource(
        ///     name:feed.Name,
        ///     source:feed.Source,
        ///     settings:nugetSourceSettings
        /// );
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("AddSource")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Sources")]
        public static void NuGetAddSource(this ICakeContext context, string name, string source, NuGetSourcesSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new NuGetToolResolver(context.FileSystem, context.Environment, context.Tools);
            var runner = new NuGetSources(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.AddSource(name, source, settings);
        }

        /// <summary>
        /// Removes NuGet package source using the specified name &amp; source from global user config
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <example>
        /// <code>
        /// var feed = new
        ///             {
        ///                 Name = EnvironmentVariable("PRIVATE_FEED_NAME"),
        ///                 Source = EnvironmentVariable("PRIVATE_FEED_SOURCE")
        ///             };
        ///
        /// NuGetRemoveSource(
        ///    name:feed.Name,
        ///    source:feed.Source
        /// );
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("RemoveSource")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Sources")]
        public static void NuGetRemoveSource(this ICakeContext context, string name, string source)
        {
            context.NuGetRemoveSource(name, source, new NuGetSourcesSettings());
        }

        /// <summary>
        /// Removes NuGet package source using the specified name, source &amp; settings from global user config
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var nugetSourceSettings = new NuGetSourcesSettings
        ///                             {
        ///                                 UserName = EnvironmentVariable("PRIVATE_FEED_USERNAME"),
        ///                                 Password = EnvironmentVariable("PRIVATE_FEED_PASSWORD"),
        ///                                 IsSensitiveSource = true,
        ///                                 Verbosity = NuGetVerbosity.Detailed
        ///                             };
        ///
        /// var feed = new
        ///             {
        ///                 Name = EnvironmentVariable("PRIVATE_FEED_NAME"),
        ///                 Source = EnvironmentVariable("PRIVATE_FEED_SOURCE")
        ///             };
        ///
        /// NuGetRemoveSource(
        ///    name:feed.Name,
        ///    source:feed.Source,
        ///    settings:nugetSourceSettings
        /// );
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("RemoveSource")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Sources")]
        public static void NuGetRemoveSource(this ICakeContext context, string name, string source, NuGetSourcesSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new NuGetToolResolver(context.FileSystem, context.Environment, context.Tools);
            var runner = new NuGetSources(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.RemoveSource(name, source, settings);
        }

        /// <summary>
        /// Checks whether or not a NuGet package source exists in the global user configuration, using the specified source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <returns>Whether or not the NuGet package source exists in the global user configuration.</returns>
        /// <example>
        ///   <code>
        /// var feed = new
        /// {
        ///     Name = EnvironmentVariable("PRIVATE_FEED_NAME"),
        ///     Source = EnvironmentVariable("PRIVATE_FEED_SOURCE")
        /// };
        /// if (!NuGetHasSource(source:feed.Source))
        /// {
        ///     Information("Source missing");
        /// }
        /// else
        /// {
        ///     Information("Source already exists");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("HasSource")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Sources")]
        public static bool NuGetHasSource(this ICakeContext context, string source)
        {
            return context.NuGetHasSource(source, new NuGetSourcesSettings());
        }

        /// <summary>
        /// Checks whether or not a NuGet package source exists in the global user configuration, using the specified source and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Whether the specified NuGet package source exist.</returns>
        /// <example>
        ///   <code>
        /// var nugetSourceSettings = new NuGetSourcesSettings
        /// {
        ///     UserName = EnvironmentVariable("PRIVATE_FEED_USERNAME"),
        ///     Password = EnvironmentVariable("PRIVATE_FEED_PASSWORD"),
        ///     IsSensitiveSource = true,
        ///     Verbosity = NuGetVerbosity.Detailed
        /// };
        /// var feed = new
        /// {
        ///     Name = EnvironmentVariable("PRIVATE_FEED_NAME"),
        ///     Source = EnvironmentVariable("PRIVATE_FEED_SOURCE")
        /// };
        /// if (!NuGetHasSource(
        ///     source:feed.Source,
        ///     settings:nugetSourceSettings))
        /// {
        ///     Information("Source missing");
        /// }
        /// else
        /// {
        ///     Information("Source already exists");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("HasSource")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Sources")]
        public static bool NuGetHasSource(this ICakeContext context, string source, NuGetSourcesSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new NuGetToolResolver(context.FileSystem, context.Environment, context.Tools);
            var runner = new NuGetSources(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            return runner.HasSource(source, settings);
        }

        /// <summary>
        /// Installs a NuGet package.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to install.</param>
        /// <example>
        /// <code>
        /// NuGetInstall("MyNugetPackage");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Install")]
        public static void NuGetInstall(this ICakeContext context, string packageId)
        {
            var settings = new NuGetInstallSettings();
            NuGetInstall(context, packageId, settings);
        }

        /// <summary>
        /// Installs NuGet packages.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageIds">The id's of the package to install.</param>
        /// <example>
        /// <code>
        /// NuGetInstall(new[] { "MyNugetPackage", "OtherNugetPackage" });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Install")]
        public static void NuGetInstall(this ICakeContext context, IEnumerable<string> packageIds)
        {
            var settings = new NuGetInstallSettings();
            NuGetInstall(context, packageIds, settings);
        }

        /// <summary>
        /// Installs a NuGet package using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to install.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// NuGetInstall("MyNugetPackage", new NuGetInstallSettings {
        ///     ExcludeVersion  = true,
        ///     OutputDirectory = "./tools"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Install")]
        public static void NuGetInstall(this ICakeContext context, string packageId, NuGetInstallSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new NuGetToolResolver(context.FileSystem, context.Environment, context.Tools);
            var runner = new NuGetInstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.Install(packageId, settings);
        }

        /// <summary>
        /// Installs NuGet packages using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageIds">The id's of the package to install.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// NuGetInstall(new[] { "MyNugetPackage", "OtherNugetPackage" }, new NuGetInstallSettings {
        ///     ExcludeVersion  = true,
        ///     OutputDirectory = "./tools"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Install")]
        public static void NuGetInstall(this ICakeContext context, IEnumerable<string> packageIds, NuGetInstallSettings settings)
        {
            if (packageIds == null)
            {
                throw new ArgumentNullException("packageIds");
            }

            foreach (var packageId in packageIds)
            {
                NuGetInstall(context, packageId, settings);
            }
        }

        /// <summary>
        /// Installs NuGet packages using the specified package configuration.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageConfigPath">The package configuration to install.</param>
        /// <example>
        /// <code>
        /// NuGetInstallFromConfig("./tools/packages.config");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Install")]
        public static void NuGetInstallFromConfig(this ICakeContext context, FilePath packageConfigPath)
        {
            var settings = new NuGetInstallSettings();
            NuGetInstallFromConfig(context, packageConfigPath, settings);
        }

        /// <summary>
        /// Installs NuGet packages using the specified package configurations.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageConfigPaths">The package configurations to install.</param>
        /// <example>
        /// <code>
        /// var packageConfigs = GetFiles("./**/packages.config");
        ///
        /// NuGetInstallFromConfig(packageConfigs);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Install")]
        public static void NuGetInstallFromConfig(this ICakeContext context, IEnumerable<FilePath> packageConfigPaths)
        {
            var settings = new NuGetInstallSettings();
            NuGetInstallFromConfig(context, packageConfigPaths, settings);
        }

        /// <summary>
        /// Installs NuGet packages using the specified package configuration and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageConfigPath">The package configuration to install.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// NuGetInstallFromConfig("./tools/packages.config", new NuGetInstallSettings {
        ///     ExcludeVersion  = true,
        ///     OutputDirectory = "./tools"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Install")]
        public static void NuGetInstallFromConfig(this ICakeContext context, FilePath packageConfigPath, NuGetInstallSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new NuGetToolResolver(context.FileSystem, context.Environment, context.Tools);
            var runner = new NuGetInstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.InstallFromConfig(packageConfigPath, settings);
        }

        /// <summary>
        /// Installs NuGet packages using the specified package configurations and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageConfigPaths">The package configurations to install.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var packageConfigs = GetFiles("./**/packages.config");
        ///
        /// NuGetInstallFromConfig(packageConfigs, new NuGetInstallSettings {
        ///     ExcludeVersion  = true,
        ///     OutputDirectory = "./tools"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Install")]
        public static void NuGetInstallFromConfig(this ICakeContext context, IEnumerable<FilePath> packageConfigPaths, NuGetInstallSettings settings)
        {
            if (packageConfigPaths == null)
            {
                throw new ArgumentNullException("packageConfigPaths");
            }

            foreach (var packageConfigPath in packageConfigPaths)
            {
                NuGetInstallFromConfig(context, packageConfigPath, settings);
            }
        }

        /// <summary>
        /// Installs NuGet packages using the specified API key, source and settings.
        /// </summary>
        /// <example>
        /// <code>
        /// var setting = new NuGetSetApiKeySettings {
        ///     Verbosity = NuGetVerbosity.Detailed
        ///     };
        /// NuGetSetApiKey("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", "https://nuget.org/api/v2/", setting);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="source">Server URL where the API key is valid.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("SetApiKey")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.SetApiKey")]
        public static void NuGetSetApiKey(this ICakeContext context, string apiKey, string source, NuGetSetApiKeySettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new NuGetToolResolver(context.FileSystem, context.Environment, context.Tools);
            var runner = new NuGetSetApiKey(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.SetApiKey(apiKey, source, settings);
        }

        /// <summary>
        /// Installs NuGet packages using the specified API key and source.
        /// </summary>
        /// <example>
        /// <code>
        /// NuGetSetApiKey("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", "https://nuget.org/api/v2/");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="source">Server URL where the API key is valid.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("SetApiKey")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.SetApiKey")]
        public static void NuGetSetApiKey(this ICakeContext context, string apiKey, string source)
        {
            context.NuGetSetApiKey(apiKey, source, new NuGetSetApiKeySettings());
        }

        /// <summary>
        /// Set the proxy settings to be used while connecting to your NuGet feed, including settings.
        /// </summary>
        /// <example>
        /// <code>
        /// var setting = new NuGetSetProxySettings {
        ///     Verbosity = NuGetVerbosity.Detailed
        ///     };
        /// NuGetSetProxy("127.0.0.1:8080", "proxyuser","Pa$$w0rd1", setting);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="proxy">The url of the proxy.</param>
        /// <param name="username">The username used to access the proxy.</param>
        /// <param name="password">The password used to access the proxy.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("SetProxy")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.SetProxy")]
        public static void NuGetSetProxy(this ICakeContext context, string proxy, string username, string password, NuGetSetProxySettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new NuGetToolResolver(context.FileSystem, context.Environment, context.Tools);
            var runner = new NuGetSetProxy(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.SetProxy(proxy, username, password, settings);
        }

        /// <summary>
        /// Set the proxy settings to be used while connecting to your NuGet feed.
        /// </summary>
        /// <example>
        /// <code>
        /// NuGetSetProxy("127.0.0.1:8080", "proxyuser","Pa$$w0rd1");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="proxy">The url of the proxy.</param>
        /// <param name="username">The username used to access the proxy.</param>
        /// <param name="password">The password used to access the proxy.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("SetProxy")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.SetProxy")]
        public static void NuGetSetProxy(this ICakeContext context, string proxy, string username, string password)
        {
            context.NuGetSetProxy(proxy, username, password, new NuGetSetProxySettings());
        }

        /// <summary>
        /// Updates NuGet packages.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="targetFile">The target to update.</param>
        /// <example>
        /// <code>
        /// NuGetUpdate("./tools/packages.config");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Update")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Update")]
        public static void NuGetUpdate(this ICakeContext context, FilePath targetFile)
        {
            var settings = new NuGetUpdateSettings();
            NuGetUpdate(context, targetFile, settings);
        }

        /// <summary>
        /// Updates NuGet packages.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="targetFiles">The targets to update.</param>
        /// <example>
        /// <code>
        /// var targets = GetFiles("./**/packages.config");
        ///
        /// NuGetUpdate(targets);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Update")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Update")]
        public static void NuGetUpdate(this ICakeContext context, IEnumerable<FilePath> targetFiles)
        {
            var settings = new NuGetUpdateSettings();
            NuGetUpdate(context, targetFiles, settings);
        }

        /// <summary>
        /// Updates NuGet packages using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="targetFile">The target to update.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// NuGetUpdate("./tools/packages.config", new NuGetUpdateSettings {
        ///     Prerelease = true,
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Update")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Update")]
        public static void NuGetUpdate(this ICakeContext context, FilePath targetFile, NuGetUpdateSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new NuGetToolResolver(context.FileSystem, context.Environment, context.Tools);
            var runner = new NuGetUpdater(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.Update(targetFile, settings);
        }

        /// <summary>
        /// Updates NuGet packages using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="targetFiles">The targets to update.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var targets = GetFiles("./**/packages.config");
        ///
        /// NuGetUpdate(targets, new NuGetUpdateSettings {
        ///     Prerelease = true,
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Update")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Update")]
        public static void NuGetUpdate(this ICakeContext context, IEnumerable<FilePath> targetFiles, NuGetUpdateSettings settings)
        {
            if (targetFiles == null)
            {
                throw new ArgumentNullException("targetFiles");
            }

            foreach (var targetFile in targetFiles)
            {
                NuGetUpdate(context, targetFile, settings);
            }
        }
    }
}
