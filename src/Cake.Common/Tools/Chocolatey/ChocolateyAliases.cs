using System;
using Cake.Common.Tools.Chocolatey.ApiKey;
using Cake.Common.Tools.Chocolatey.Config;
using Cake.Common.Tools.Chocolatey.Features;
using Cake.Common.Tools.Chocolatey.Install;
using Cake.Common.Tools.Chocolatey.Pack;
using Cake.Common.Tools.Chocolatey.Pin;
using Cake.Common.Tools.Chocolatey.Push;
using Cake.Common.Tools.Chocolatey.Sources;
using Cake.Common.Tools.Chocolatey.Upgrade;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.Chocolatey
{
    /// <summary>
    /// Contains functionality for working with Chocolatey.
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
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Pack")]
        public static void ChocolateyPack(this ICakeContext context, FilePath nuspecFilePath, ChocolateyPackSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var packer = new ChocolateyPacker(context.FileSystem, context.Environment, context.ProcessRunner, context.Log, context.Globber, resolver);
            packer.Pack(nuspecFilePath, settings);
        }

        /// <summary>
        /// Creates a Chocolatey package using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Pack")]
        public static void ChocolateyPack(this ICakeContext context, ChocolateyPackSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var packer = new ChocolateyPacker(context.FileSystem, context.Environment, context.ProcessRunner, context.Log, context.Globber, resolver);
            packer.Pack(settings);
        }

        /// <summary>
        /// Installs a Chocolatey package.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to install.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Install")]
        public static void ChocolateyInstall(this ICakeContext context, string packageId, ChocolateyInstallSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyInstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            runner.Install(packageId, settings);
        }

        /// <summary>
        /// Installs Chocolatey packages using the specified package configuration.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageConfigPath">The package configuration to install.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Install")]
        public static void ChocolateyInstallFromConfig(this ICakeContext context, FilePath packageConfigPath, ChocolateyInstallSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyInstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            runner.InstallFromConfig(packageConfigPath, settings);
        }

        /// <summary>
        /// Pins a Chocolatey package using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Pin")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Pin")]
        public static void ChocolateyPin(this ICakeContext context, string name, ChocolateyPinSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var packer = new ChocolateyPinner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            packer.Pin(name, settings);
        }

        /// <summary>
        /// Sets the Api Key for a Chocolatey Source using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="apiKey">The API Key.</param>
        /// <param name="source">The source.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("ApiKey")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.ApiKey")]
        public static void ChocolateyApiKey(this ICakeContext context, string apiKey, string source, ChocolateyApiKeySettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var packer = new ChocolateyApiKeySetter(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            packer.Set(apiKey, source, settings);
        }

        /// <summary>
        /// Sets the config parameter using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Config")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Config")]
        public static void ChocolateyConfig(this ICakeContext context, string name, string value, ChocolateyConfigSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var packer = new ChocolateyConfigSetter(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            packer.Set(name, value, settings);
        }

        /// <summary>
        /// Enables a Chocolatey Feature using the specified name
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the feature.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("EnableFeature")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Features")]
        public static void ChocolateyEnableFeature(this ICakeContext context, string name, ChocolateyFeatureSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyFeatureToggler(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            runner.EnableFeature(name, settings);
        }

        /// <summary>
        /// Disables a Chocolatey Feature using the specified name
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the feature.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("DisableFeature")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Features")]
        public static void ChocolateyDisableFeature(this ICakeContext context, string name, ChocolateyFeatureSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyFeatureToggler(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            runner.DisableFeature(name, settings);
        }

        /// <summary>
        /// Adds Chocolatey package source using the specified name &amp;source to global user config
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("AddSource")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Sources")]
        public static void ChocolateyAddSource(this ICakeContext context, string name, string source, ChocolateySourcesSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateySources(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            runner.AddSource(name, source, settings);
        }

        /// <summary>
        /// Removes Chocolatey package source using the specified name &amp; source from global user config
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("RemoveSource")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Sources")]
        public static void ChocolateyRemoveSource(this ICakeContext context, string name, ChocolateySourcesSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateySources(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            runner.RemoveSource(name, settings);
        }

        /// <summary>
        /// Enables a Chocolatey Source using the specified name
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("EnableSource")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Sources")]
        public static void ChocolateyEnableSource(this ICakeContext context, string name, ChocolateySourcesSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateySources(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            runner.EnableSource(name, settings);
        }

        /// <summary>
        /// Disables a Chocolatey Source using the specified name
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("DisableSource")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Sources")]
        public static void ChocolateyDisableSource(this ICakeContext context, string name, ChocolateySourcesSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateySources(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            runner.DisableSource(name, settings);
        }

        /// <summary>
        /// Pushes a Chocolatey package to a Chocolatey server and publishes it.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageFilePath">The <c>.nupkg</c> file path.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Push")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Push")]
        public static void ChocolateyPush(this ICakeContext context, FilePath packageFilePath, ChocolateyPushSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var packer = new ChocolateyPusher(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            packer.Push(packageFilePath, settings);
        }

        /// <summary>
        /// Upgrades Chocolatey package.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to upgrade.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Upgrade")]
        [CakeNamespaceImport("Cake.Common.Tools.Chocolatey.Upgrade")]
        public static void ChocolateyUpgrade(this ICakeContext context, string packageId)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyUpgrader(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            runner.Upgrade(packageId, new ChocolateyUpgradeSettings());
        }

        /// <summary>
        /// Upgrades Chocolatey package using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to upgrade.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Update")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Update")]
        public static void ChocolateyUpgrade(this ICakeContext context, string packageId, ChocolateyUpgradeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new ChocolateyToolResolver(context.FileSystem, context.Environment);
            var runner = new ChocolateyUpgrader(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            runner.Upgrade(packageId, settings);
        }
    }
}