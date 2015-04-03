using System;
using Cake.Common.Tools.NuGet.Install;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Common.Tools.NuGet.Push;
using Cake.Common.Tools.NuGet.Restore;
using Cake.Common.Tools.NuGet.SetApiKey;
using Cake.Common.Tools.NuGet.Sources;
using Cake.Common.Tools.NuGet.Update;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet
{
    /// <summary>
    /// Contains functionality for working with NuGet.
    /// </summary>
    [CakeAliasCategory("NuGet")]
    public static class NuGetAliases
    {
        /// <summary>
        /// Creates a NuGet package using the specified Nuspec file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="nuspecFilePath">The nuspec file path.</param>
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
        ///                                     Summary                 = "Excellent summare of what the package does", 
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
        ///                                                                          new NuSpecContent {Source = "bin/SlackPRTGCommander.dll", Target = "bin"},
        ///                                                                       },
        ///                                     BasePath                = "./src/TestNuget/bin/release", 
        ///                                     OutputDirectory         = "./nuget"
        ///                                 };
        ///     
        ///     NuGetPack("./nuspec/SlackPRTGCommander.nuspec", nuGetPackSettings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Pack")]
        public static void NuGetPack(this ICakeContext context, FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var packer = new NuGetPacker(context.FileSystem, context.Environment, 
                context.ProcessRunner, context.Log, context.GetToolResolver("NuGet"));
            packer.Pack(nuspecFilePath, settings);
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

            var runner = new NuGetRestorer(context.FileSystem, context.Environment, context.ProcessRunner, context.GetToolResolver("NuGet"));
            runner.Restore(targetFilePath, settings);
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

            var packer = new NuGetPusher(context.FileSystem, context.Environment, context.ProcessRunner, context.GetToolResolver("NuGet"));
            packer.Push(packageFilePath, settings);
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

            var runner = new NuGetSources(context.FileSystem, context.Environment, context.ProcessRunner, context.GetToolResolver("NuGet"));
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

            var runner = new NuGetSources(context.FileSystem, context.Environment, context.ProcessRunner, context.GetToolResolver("NuGet"));
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
        /// Name = EnvironmentVariable("PRIVATE_FEED_NAME"),
        /// Source = EnvironmentVariable("PRIVATE_FEED_SOURCE")
        /// };
        /// if (!NuGetHasSource(
        /// source:feed.Source
        /// ))
        /// {
        /// Information("Source missing");
        /// }
        /// else
        /// {
        /// Information("Source already exists");
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
        /// UserName = EnvironmentVariable("PRIVATE_FEED_USERNAME"),
        /// Password = EnvironmentVariable("PRIVATE_FEED_PASSWORD"),
        /// IsSensitiveSource = true,
        /// Verbosity = NuGetVerbosity.Detailed
        /// };
        /// var feed = new
        /// {
        /// Name = EnvironmentVariable("PRIVATE_FEED_NAME"),
        /// Source = EnvironmentVariable("PRIVATE_FEED_SOURCE")
        /// };
        /// if (!NuGetHasSource(
        /// source:feed.Source,
        /// settings:nugetSourceSettings
        /// ))
        /// {
        /// Information("Source missing");
        /// }
        /// else
        /// {
        /// Information("Source already exists");
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

            var runner = new NuGetSources(context.FileSystem, context.Environment, context.ProcessRunner, context.GetToolResolver("NuGet"));
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

            var runner = new NuGetInstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.GetToolResolver("NuGet"));
            runner.Install(packageId, settings);
        }
        
        /// <summary>
        /// Installs NuGet packages using the specified package configuration.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageConfigPath">The package configuration to install.</param>
        /// <example>
        /// <code>
        /// NuGetInstall("./tools/packages.config");
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
        /// Installs NuGet packages using the specified package configuration and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageConfigPath">The package configuration to install.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// NuGetInstall("./tools/packages.config", new NuGetInstallSettings {
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

            var runner = new NuGetInstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.GetToolResolver("NuGet"));
            runner.InstallFromConfig(packageConfigPath, settings);
        }

        /// <summary>
        /// Installs NuGet packages using the specified API key, source and settings.
        /// </summary>
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

            var runner = new NuGetSetApiKey(context.Log, context.FileSystem, context.Environment, context.ProcessRunner, context.GetToolResolver("NuGet"));
            runner.SetApiKey(apiKey, source, settings);
        }

        /// <summary>
        /// Installs NuGet packages using the specified API key and source.
        /// </summary>
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
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new NuGetUpdater(context.FileSystem, context.Environment, context.ProcessRunner, context.GetToolResolver("NuGet"));
            runner.Update(targetFile, new NuGetUpdateSettings());
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

            var runner = new NuGetUpdater(context.FileSystem, context.Environment, context.ProcessRunner, context.GetToolResolver("NuGet"));
            runner.Update(targetFile, settings);
        }
    }
}
