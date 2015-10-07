using System;
using Cake.Common.Tools.Chocolatey.ApiKey;
using Cake.Common.Tools.Chocolatey.Config;
using Cake.Common.Tools.Chocolatey.Install;
using Cake.Common.Tools.Chocolatey.Pack;
using Cake.Common.Tools.Chocolatey.Pin;
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
    }
}