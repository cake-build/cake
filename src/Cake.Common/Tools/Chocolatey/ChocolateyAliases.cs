// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tools.Chocolatey.ApiKey;
using Cake.Common.Tools.Chocolatey.Config;
using Cake.Common.Tools.Chocolatey.Download;
using Cake.Common.Tools.Chocolatey.Features;
using Cake.Common.Tools.Chocolatey.Install;
using Cake.Common.Tools.Chocolatey.New;
using Cake.Common.Tools.Chocolatey.Pack;
using Cake.Common.Tools.Chocolatey.Pin;
using Cake.Common.Tools.Chocolatey.Push;
using Cake.Common.Tools.Chocolatey.Sources;
using Cake.Common.Tools.Chocolatey.Uninstall;
using Cake.Common.Tools.Chocolatey.Upgrade;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.Chocolatey
{
    /// <summary>
    /// <para>Contains functionality for working with <see href="https://github.com/chocolatey/choco">Chocolatey</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, Chocolatey will require to be installed on the machine where the build script
    /// is being run.  See this <see href="https://github.com/chocolatey/choco/wiki/Installation">page</see> for details on how
    /// Chocolatey can be installed.
    /// </para>
    /// </summary>
    [CakeAliasCategory("Chocolatey")]
    public static class ChocolateyAliases
    {
        /// <summary>
        /// Creates a Chocolatey package using the specified Nuspec file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="nuspecFilePath">The nuspec file path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var chocolateyPackSettings   = new ChocolateyPackSettings {
        ///                                     Id                      = "TestChocolatey",
        ///                                     Title                   = "The tile of the package",
        ///                                     Version                 = "0.0.0.1",
        ///                                     Authors                 = new[] {"John Doe"},
        ///                                     Owners                  = new[] {"Contoso"},
        ///                                     Summary                 = "Excellent summary of what the package does",
        ///                                     Description             = "The description of the package",
        ///                                     ProjectUrl              = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     PackageSourceUrl        = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     ProjectSourceUrl        = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     DocsUrl                 = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     MailingListUrl          = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     BugTrackerUrl           = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     Tags                    = new [] {"Cake", "Script", "Build"},
        ///                                     Copyright               = "Some company 2015",
        ///                                     LicenseUrl              = new Uri("https://github.com/SomeUser/TestChocolatey/blob/master/LICENSE.md"),
        ///                                     RequireLicenseAcceptance= false,
        ///                                     IconUrl                 = new Uri("http://cdn.rawgit.com/SomeUser/TestChocolatey/master/icons/testchocolatey.png"),
        ///                                     ReleaseNotes            = new [] {"Bug fixes", "Issue fixes", "Typos"},
        ///                                     Files                   = new [] {
        ///                                                                          new ChocolateyNuSpecContent {Source = "bin/TestChocolatey.dll", Target = "bin"},
        ///                                                                       },
        ///                                     Debug                   = false,
        ///                                     Verbose                 = false,
        ///                                     Force                   = false,
        ///                                     Noop                    = false,
        ///                                     LimitOutput             = false,
        ///                                     ExecutionTimeout        = 13,
        ///                                     CacheLocation           = @"C:\temp",
        ///                                     AllowUnofficial          = false
        ///                                 };
        ///
        ///     ChocolateyPack("./nuspec/TestChocolatey.nuspec", chocolateyPackSettings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Pack")]
        public static void ChocolateyPack(this ICakeContext context, FilePath nuspecFilePath, ChocolateyPackSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var packer = new ChocolateyPacker(context.FileSystem, context.Environment, context.ProcessRunner, context.Log, context.Tools, resolver);
            packer.Pack(nuspecFilePath, settings);
        }

        /// <summary>
        /// Creates Chocolatey packages using the specified Nuspec files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The nuspec file paths.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var chocolateyPackSettings   = new ChocolateyPackSettings {
        ///                                     Id                      = "TestChocolatey",
        ///                                     Title                   = "The tile of the package",
        ///                                     Version                 = "0.0.0.1",
        ///                                     Authors                 = new[] {"John Doe"},
        ///                                     Owners                  = new[] {"Contoso"},
        ///                                     Summary                 = "Excellent summary of what the package does",
        ///                                     Description             = "The description of the package",
        ///                                     ProjectUrl              = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     PackageSourceUrl        = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     ProjectSourceUrl        = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     DocsUrl                 = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     MailingListUrl          = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     BugTrackerUrl           = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     Tags                    = new [] {"Cake", "Script", "Build"},
        ///                                     Copyright               = "Some company 2015",
        ///                                     LicenseUrl              = new Uri("https://github.com/SomeUser/TestChocolatey/blob/master/LICENSE.md"),
        ///                                     RequireLicenseAcceptance= false,
        ///                                     IconUrl                 = new Uri("http://cdn.rawgit.com/SomeUser/TestChocolatey/master/icons/testchocolatey.png"),
        ///                                     ReleaseNotes            = new [] {"Bug fixes", "Issue fixes", "Typos"},
        ///                                     Files                   = new [] {
        ///                                                                          new ChocolateyNuSpecContent {Source = "bin/TestChocolatey.dll", Target = "bin"},
        ///                                                                       },
        ///                                     Debug                   = false,
        ///                                     Verbose                 = false,
        ///                                     Force                   = false,
        ///                                     Noop                    = false,
        ///                                     LimitOutput             = false,
        ///                                     ExecutionTimeout        = 13,
        ///                                     CacheLocation           = @"C:\temp",
        ///                                     AllowUnofficial          = false
        ///                                 };
        ///
        ///     var nuspecFiles = GetFiles("./**/*.nuspec");
        ///     ChocolateyPack(nuspecFiles, chocolateyPackSettings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Pack")]
        public static void ChocolateyPack(this ICakeContext context, IEnumerable<FilePath> filePaths, ChocolateyPackSettings settings)
        {
            if (filePaths == null)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }

            foreach (var filePath in filePaths)
            {
                ChocolateyPack(context, filePath, settings);
            }
        }

        /// <summary>
        /// Creates a Chocolatey package using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var chocolateyPackSettings   = new ChocolateyPackSettings {
        ///                                     Id                      = "TestChocolatey",
        ///                                     Title                   = "The tile of the package",
        ///                                     Version                 = "0.0.0.1",
        ///                                     Authors                 = new[] {"John Doe"},
        ///                                     Owners                  = new[] {"Contoso"},
        ///                                     Summary                 = "Excellent summary of what the package does",
        ///                                     Description             = "The description of the package",
        ///                                     ProjectUrl              = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     PackageSourceUrl        = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     ProjectSourceUrl        = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     DocsUrl                 = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     MailingListUrl          = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     BugTrackerUrl           = new Uri("https://github.com/SomeUser/TestChocolatey/"),
        ///                                     Tags                    = new [] {"Cake", "Script", "Build"},
        ///                                     Copyright               = "Some company 2015",
        ///                                     LicenseUrl              = new Uri("https://github.com/SomeUser/TestChocolatey/blob/master/LICENSE.md"),
        ///                                     RequireLicenseAcceptance= false,
        ///                                     IconUrl                 = new Uri("http://cdn.rawgit.com/SomeUser/TestChocolatey/master/icons/testchocolatey.png"),
        ///                                     ReleaseNotes            = new [] {"Bug fixes", "Issue fixes", "Typos"},
        ///                                     Files                   = new [] {
        ///                                                                          new ChocolateyNuSpecContent {Source = "bin/TestChocolatey.dll", Target = "bin"},
        ///                                                                       },
        ///                                     Debug                   = false,
        ///                                     Verbose                 = false,
        ///                                     Force                   = false,
        ///                                     Noop                    = false,
        ///                                     LimitOutput             = false,
        ///                                     ExecutionTimeout        = 13,
        ///                                     CacheLocation           = @"C:\temp",
        ///                                     AllowUnofficial          = false
        ///                                 };
        ///
        ///     ChocolateyPack(chocolateyPackSettings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Pack")]
        public static void ChocolateyPack(this ICakeContext context, ChocolateyPackSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var packer = new ChocolateyPacker(context.FileSystem, context.Environment, context.ProcessRunner, context.Log, context.Tools, resolver);
            packer.Pack(settings);
        }

        /// <summary>
        /// Installs a Chocolatey package.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to install.</param>
        /// <example>
        /// <code>
        /// ChocolateyInstall("MyChocolateyPackage");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Install")]
        public static void ChocolateyInstall(this ICakeContext context, string packageId)
        {
            var settings = new ChocolateyInstallSettings();
            ChocolateyInstall(context, packageId, settings);
        }

        /// <summary>
        /// Installs a Chocolatey package using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to install.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyInstall("MyChocolateyPackage", new ChocolateyInstallSettings {
        ///     Source                = true,
        ///     Version               = "1.2.3",
        ///     Prerelease            = false,
        ///     Forcex86              = false,
        ///     InstallArguments      = "arg1",
        ///     OverrideArguments     = false,
        ///     NotSilent             = false,
        ///     PackageParameters     = "param1",
        ///     AllowDowngrade        = false,
        ///     SideBySide            = false,
        ///     IgnoreDependencies    = false,
        ///     ForceDependencies     = false,
        ///     SkipPowerShell        = false,
        ///     User                  = "user",
        ///     Password              = "password",
        ///     IgnoreChecksums       = false,
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Install")]
        public static void ChocolateyInstall(this ICakeContext context, string packageId, ChocolateyInstallSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyInstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.Install(packageId, settings);
        }

        /// <summary>
        /// Installs Chocolatey packages using the specified package configuration.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageConfigPath">The package configuration to install.</param>
        /// <example>
        /// <code>
        /// ChocolateyInstallFromConfig("./tools/packages.config");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Install")]
        public static void ChocolateyInstallFromConfig(this ICakeContext context, FilePath packageConfigPath)
        {
            var settings = new ChocolateyInstallSettings();
            ChocolateyInstallFromConfig(context, packageConfigPath, settings);
        }

        /// <summary>
        /// Installs Chocolatey packages using the specified package configuration and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageConfigPath">The package configuration to install.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyInstallFromConfig("./tools/packages.config", new ChocolateyInstallSettings {
        ///     Source                = true,
        ///     Version               = "1.2.3",
        ///     Prerelease            = false,
        ///     Forcex86              = false,
        ///     InstallArguments      = "arg1",
        ///     OverrideArguments     = false,
        ///     NotSilent             = false,
        ///     PackageParameters     = "param1",
        ///     AllowDowngrade        = false,
        ///     SideBySide            = false,
        ///     IgnoreDependencies    = false,
        ///     ForceDependencies     = false,
        ///     SkipPowerShell        = false,
        ///     User                  = "user",
        ///     Password              = "password",
        ///     IgnoreChecksums       = false,
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Install")]
        public static void ChocolateyInstallFromConfig(this ICakeContext context, FilePath packageConfigPath, ChocolateyInstallSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyInstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.InstallFromConfig(packageConfigPath, settings);
        }

        /// <summary>
        /// Uninstalls a Chocolatey package.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to uninstall.</param>
        /// <example>
        /// <code>
        /// ChocolateyUninstall("MyChocolateyPackage");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Uninstall")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Uninstall")]
        public static void ChocolateyUninstall(this ICakeContext context, string packageId)
        {
            var settings = new ChocolateyUninstallSettings();
            ChocolateyUninstall(context, packageId, settings);
        }

        /// <summary>
        /// Uninstalls a Chocolatey package using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to uninstall.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyUninstall("MyChocolateyPackage", new ChocolateyUninstallSettings {
        ///     Source                  = true,
        ///     Version                 = "1.2.3",
        ///     UninstallArguments      = "arg1",
        ///     OverrideArguments       = false,
        ///     NotSilent               = false,
        ///     PackageParameters       = "param1",
        ///     SideBySide              = false,
        ///     IgnoreDependencies      = false,
        ///     ForceDependencies       = false,
        ///     SkipPowerShell          = false,
        ///     Debug                   = false,
        ///     Verbose                 = false,
        ///     FailOnStandardError     = false,
        ///     UseSystemPowershell     = false,
        ///     AllVersions             = false,
        ///     Force                   = false,
        ///     Noop                    = false,
        ///     LimitOutput             = false,
        ///     ExecutionTimeout        = 13,
        ///     CacheLocation           = @"C:\temp",
        ///     AllowUnofficial         = false,
        ///     GlobalArguments         = false,
        ///     GlobalPackageParameters = false,
        ///     IgnorePackageExitCodes  = false,
        ///     UsePackageExitCodes     = false,
        ///     UseAutoUninstaller      = false,
        ///     SkipAutoUninstaller     = false,
        ///     FailOnAutoUninstaller   = false,
        ///     IgnoreAutoUninstaller   = false
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Uninstall")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Uninstall")]
        public static void ChocolateyUninstall(this ICakeContext context, string packageId, ChocolateyUninstallSettings settings)
        {
            ChocolateyUninstall(context, new[] { packageId }, settings);
        }

        /// <summary>
        /// Uninstalls a Chocolatey package.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageIds">The ids of the packages to uninstall.</param>
        /// <example>
        /// <code>
        /// ChocolateyUninstall("MyChocolateyPackage");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Uninstall")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Uninstall")]
        public static void ChocolateyUninstall(this ICakeContext context, IEnumerable<string> packageIds)
        {
            var settings = new ChocolateyUninstallSettings();
            ChocolateyUninstall(context, packageIds, settings);
        }

        /// <summary>
        /// Uninstalls Chocolatey packages using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageIds">The ids of the packages to uninstall.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyUninstall("MyChocolateyPackage", new ChocolateyUninstallSettings {
        ///     Source                  = true,
        ///     Version                 = "1.2.3",
        ///     UninstallArguments      = "arg1",
        ///     OverrideArguments       = false,
        ///     NotSilent               = false,
        ///     PackageParameters       = "param1",
        ///     SideBySide              = false,
        ///     IgnoreDependencies      = false,
        ///     ForceDependencies       = false,
        ///     SkipPowerShell          = false,
        ///     Debug                   = false,
        ///     Verbose                 = false,
        ///     FailOnStandardError     = false,
        ///     UseSystemPowershell     = false,
        ///     AllVersions             = false,
        ///     Force                   = false,
        ///     Noop                    = false,
        ///     LimitOutput             = false,
        ///     ExecutionTimeout        = 13,
        ///     CacheLocation           = @"C:\temp",
        ///     AllowUnofficial         = false,
        ///     GlobalArguments         = false,
        ///     GlobalPackageParameters = false,
        ///     IgnorePackageExitCodes  = false,
        ///     UsePackageExitCodes     = false,
        ///     UseAutoUninstaller      = false,
        ///     SkipAutoUninstaller     = false,
        ///     FailOnAutoUninstaller   = false,
        ///     IgnoreAutoUninstaller   = false
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Uninstall")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Uninstall")]
        public static void ChocolateyUninstall(this ICakeContext context, IEnumerable<string> packageIds, ChocolateyUninstallSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyUninstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.Uninstall(packageIds, settings);
        }

        /// <summary>
        /// Pins a Chocolatey package using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyPin("MyChocolateyPackage", new ChocolateyPinSettings {
        ///     Version               = "1.2.3",
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Pin")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Pin")]
        public static void ChocolateyPin(this ICakeContext context, string name, ChocolateyPinSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var packer = new ChocolateyPinner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            packer.Pin(name, settings);
        }

        /// <summary>
        /// Sets the Api Key for a Chocolatey Source using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="apiKey">The API Key.</param>
        /// <param name="source">The source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyApiKey("myApiKey", "http://www.mysource.com", new ChocolateyApiKeySettings {
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("ApiKey")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.ApiKey")]
        public static void ChocolateyApiKey(this ICakeContext context, string apiKey, string source, ChocolateyApiKeySettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var packer = new ChocolateyApiKeySetter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            packer.Set(apiKey, source, settings);
        }

        /// <summary>
        /// Sets the config parameter using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyConfig("cacheLocation", @"c:\temp", new ChocolateyConfigSettings {
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Config")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Config")]
        public static void ChocolateyConfig(this ICakeContext context, string name, string value, ChocolateyConfigSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var packer = new ChocolateyConfigSetter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            packer.Set(name, value, settings);
        }

        /// <summary>
        /// Enables a Chocolatey Feature using the specified name
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the feature.</param>
        /// <example>
        /// <code>
        /// ChocolateyEnableFeature("checkSumFiles");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("EnableFeature")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Features")]
        public static void ChocolateyEnableFeature(this ICakeContext context, string name)
        {
            context.ChocolateyEnableFeature(name, new ChocolateyFeatureSettings());
        }

        /// <summary>
        /// Enables a Chocolatey Feature using the specified name and settings
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the feature.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyEnableFeature("checkSumFiles", new ChocolateyFeatureSettings {
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("EnableFeature")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Features")]
        public static void ChocolateyEnableFeature(this ICakeContext context, string name, ChocolateyFeatureSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyFeatureToggler(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.EnableFeature(name, settings);
        }

        /// <summary>
        /// Disables a Chocolatey Feature using the specified name
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the feature.</param>
        /// <example>
        /// <code>
        /// ChocolateyDisableFeature("checkSumFiles");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("DisableFeature")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Features")]
        public static void ChocolateyDisableFeature(this ICakeContext context, string name)
        {
            context.ChocolateyDisableFeature(name, new ChocolateyFeatureSettings());
        }

        /// <summary>
        /// Disables a Chocolatey Feature using the specified name and settings
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the feature.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyDisableFeature("checkSumFiles", new ChocolateyFeatureSettings {
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("DisableFeature")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Features")]
        public static void ChocolateyDisableFeature(this ICakeContext context, string name, ChocolateyFeatureSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyFeatureToggler(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.DisableFeature(name, settings);
        }

        /// <summary>
        /// Adds Chocolatey package source using the specified name &amp;source to global user config
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <example>
        /// <code>
        /// ChocolateyAddSource("MySource", "http://www.mysource.com");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("AddSource")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Sources")]
        public static void ChocolateyAddSource(this ICakeContext context, string name, string source)
        {
            context.ChocolateyAddSource(name, source, new ChocolateySourcesSettings());
        }

        /// <summary>
        /// Adds Chocolatey package source using the specified name, source &amp; settings to global user config
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyAddSource("MySource", "http://www.mysource.com", new ChocolateySourcesSettings {
        ///     UserName              = "user",
        ///     Password              = "password",
        ///     Priority              = 13,
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("AddSource")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Sources")]
        public static void ChocolateyAddSource(this ICakeContext context, string name, string source, ChocolateySourcesSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateySources(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.AddSource(name, source, settings);
        }

        /// <summary>
        /// Removes Chocolatey package source using the specified name &amp; source from global user config
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <example>
        /// <code>
        /// ChocolateyRemoveSource("MySource");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("RemoveSource")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Sources")]
        public static void ChocolateyRemoveSource(this ICakeContext context, string name)
        {
            context.ChocolateyRemoveSource(name, new ChocolateySourcesSettings());
        }

        /// <summary>
        /// Removes Chocolatey package source using the specified name, source &amp; settings from global user config
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyRemoveSource("MySource", new ChocolateySourcesSettings {
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("RemoveSource")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Sources")]
        public static void ChocolateyRemoveSource(this ICakeContext context, string name, ChocolateySourcesSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateySources(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.RemoveSource(name, settings);
        }

        /// <summary>
        /// Enables a Chocolatey Source using the specified name
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <example>
        /// <code>
        /// ChocolateyEnableSource("MySource");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("EnableSource")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Sources")]
        public static void ChocolateyEnableSource(this ICakeContext context, string name)
        {
            context.ChocolateyEnableSource(name, new ChocolateySourcesSettings());
        }

        /// <summary>
        /// Enables a Chocolatey Source using the specified name and settings
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyEnableSource("MySource", new ChocolateySourcesSettings {
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("EnableSource")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Sources")]
        public static void ChocolateyEnableSource(this ICakeContext context, string name, ChocolateySourcesSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateySources(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.EnableSource(name, settings);
        }

        /// <summary>
        /// Disables a Chocolatey Source using the specified name
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <example>
        /// <code>
        /// ChocolateyDisableSource("MySource");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("DisableSource")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Sources")]
        public static void ChocolateyDisableSource(this ICakeContext context, string name)
        {
            context.ChocolateyDisableSource(name, new ChocolateySourcesSettings());
        }

        /// <summary>
        /// Disables a Chocolatey Source using the specified name and settings
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyDisableSource("MySource", new ChocolateySourcesSettings {
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("DisableSource")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Sources")]
        public static void ChocolateyDisableSource(this ICakeContext context, string name, ChocolateySourcesSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateySources(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.DisableSource(name, settings);
        }

        /// <summary>
        /// Pushes a Chocolatey package to a Chocolatey server and publishes it.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageFilePath">The <c>.nupkg</c> file path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// // Get the path to the package.
        /// var package = "./chocolatey/MyChocolateyPackage.0.0.1.nupkg";
        ///
        /// // Push the package.
        /// ChocolateyPush(package, new ChocolateyPushSettings {
        ///     Source                = "http://example.com/chocolateyfeed",
        ///     ApiKey                = "4003d786-cc37-4004-bfdf-c4f3e8ef9b3a"
        ///     Timeout               = 300
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Push")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Push")]
        public static void ChocolateyPush(this ICakeContext context, FilePath packageFilePath, ChocolateyPushSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var packer = new ChocolateyPusher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            packer.Push(packageFilePath, settings);
        }

        /// <summary>
        /// Pushes Chocolatey packages to a Chocolatey server and publishes them.
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
        /// ChocolateyPush(packages, new ChocolateyPushSettings {
        ///     Source                = "http://example.com/chocolateyfeed",
        ///     ApiKey                = "4003d786-cc37-4004-bfdf-c4f3e8ef9b3a"
        ///     Timeout               = 300
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Push")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Push")]
        public static void ChocolateyPush(this ICakeContext context, IEnumerable<FilePath> packageFilePaths, ChocolateyPushSettings settings)
        {
            if (packageFilePaths == null)
            {
                throw new ArgumentNullException(nameof(packageFilePaths));
            }

            foreach (var packageFilePath in packageFilePaths)
            {
                ChocolateyPush(context, packageFilePath, settings);
            }
        }

        /// <summary>
        /// Upgrades Chocolatey package.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to upgrade.</param>
        /// <example>
        /// <code>
        /// ChocolateyUpgrade("MyChocolateyPackage");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Upgrade")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Upgrade")]
        public static void ChocolateyUpgrade(this ICakeContext context, string packageId)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyUpgrader(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.Upgrade(packageId, new ChocolateyUpgradeSettings());
        }

        /// <summary>
        /// Upgrades Chocolatey package using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to upgrade.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyUpgrade("MyChocolateyPackage", new ChocolateyUpgradeSettings {
        ///     Source                = true,
        ///     Version               = "1.2.3",
        ///     Prerelease            = false,
        ///     Forcex86              = false,
        ///     InstallArguments      = "arg1",
        ///     OverrideArguments     = false,
        ///     NotSilent             = false,
        ///     PackageParameters     = "param1",
        ///     AllowDowngrade        = false,
        ///     SideBySide            = false,
        ///     IgnoreDependencies    = false,
        ///     SkipPowerShell        = false,
        ///     FailOnUnfound        = false,
        ///     FailOnNotInstalled        = false,
        ///     User                  = "user",
        ///     Password              = "password",
        ///     IgnoreChecksums       = false,
        ///     Debug                 = false,
        ///     Verbose               = false,
        ///     Force                 = false,
        ///     Noop                  = false,
        ///     LimitOutput           = false,
        ///     ExecutionTimeout      = 13,
        ///     CacheLocation         = @"C:\temp",
        ///     AllowUnofficial        = false
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Upgrade")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Upgrade")]
        public static void ChocolateyUpgrade(this ICakeContext context, string packageId, ChocolateyUpgradeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyUpgrader(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.Upgrade(packageId, settings);
        }

        /// <summary>
        /// Generate package specification files for a new package using the default settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to create.</param>
        /// <example>
        /// <code>
        /// ChocolateyNew("MyChocolateyPackage");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("New")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.New")]
        public static void ChocolateyNew(this ICakeContext context, string packageId)
        {
            ChocolateyNew(context, packageId, new ChocolateyNewSettings());
        }

        /// <summary>
        /// Generate package specification files for a new package using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to create.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ChocolateyNew("MyChocolateyPackage", new ChocolateyNewSettings {
        ///     PackageVersion = "1.2.3",
        ///     MaintainerName = "John Doe",
        ///     MaintainerRepo = "johndoe"
        /// });
        /// </code>
        /// </example>
        /// <example>
        /// <code>
        /// var settings = new ChocolateyNewSettings {
        ///     MaintainerName = "John Doe"
        /// }
        /// settings.AdditionalPropertyValues("Tags", "CustomPackage");
        /// ChocolateyNew("MyChocolateyPackage", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("New")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.New")]
        public static void ChocolateyNew(this ICakeContext context, string packageId, ChocolateyNewSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyScaffolder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.CreatePackage(packageId, settings);
        }

        /// <summary>
        /// Downloads a Chocolatey package to the current working directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to download.</param>
        /// <example>
        /// <code>
        /// ChocolateyDownload("MyChocolateyPackage");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Download")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Download")]
        public static void ChocolateyDownload(this ICakeContext context, string packageId)
        {
            var settings = new ChocolateyDownloadSettings();
            ChocolateyDownload(context, packageId, settings);
        }

        /// <summary>
        /// Downloads a Chocolatey package using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to install.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <para>Download a package to a specific folder:</para>
        /// <code>
        /// ChocolateyDownload(
        ///     "MyChocolateyPackage",
        ///     new ChocolateyDownloadSettings {
        ///         OutputDirectory = @"C:\download\"
        ///     });
        /// </code>
        /// <para>Download and internalize a package:</para>
        /// <code>
        /// ChocolateyDownload(
        ///     "MyChocolateyPackage",
        ///     new ChocolateyDownloadSettings {
        ///         Internalize = true
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Download")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Download")]
        public static void ChocolateyDownload(this ICakeContext context, string packageId, ChocolateyDownloadSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyDownloader(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);
            runner.Download(packageId, settings);
        }
    }
}