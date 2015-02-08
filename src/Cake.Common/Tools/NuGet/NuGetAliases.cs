using System;
using Cake.Common.Tools.NuGet.Install;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Common.Tools.NuGet.Push;
using Cake.Common.Tools.NuGet.Restore;
using Cake.Common.Tools.NuGet.Sources;
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
        /// <param name="packageFilePath">The nupkg file path.</param>
        /// <param name="settings">The settings.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("AddSource")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Sources")]
        public static void NuGetAddSource(this ICakeContext context, string name, string source)
        {
            context.NuGetAddSource(name, source, NuGetSourcesSettings.Default);
        }

        /// <summary>
        /// Adds NuGet package source using the specified name, source &amp; settings to global user config
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("RemoveSource")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Sources")]
        public static void NuGetRemoveSource(this ICakeContext context, string name, string source)
        {
            context.NuGetRemoveSource(name, source, NuGetSourcesSettings.Default);
        }

        /// <summary>
        /// Removes NuGet package source using the specified name, source &amp; settings from global user config
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
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
        /// Checks if NuGet package source exists in global user config using the specified source
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="source">Path to the package(s) source.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("HasSource")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Sources")]
        public static bool NuGetHasSource(this ICakeContext context, string source)
        {
            return context.NuGetHasSource(source, NuGetSourcesSettings.Default);
        }

        /// <summary>
        /// Checks if NuGet package source exists in global user config using the specified source &amp; settings
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
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
        /// Installs NuGet packages for the specified target.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to install.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Install")]
        public static void NuGetInstall(this ICakeContext context, string packageId)
        {
            var settings = new NuGetInstallSettings();
            NuGetInstall(context, packageId, settings);
        }

        /// <summary>
        /// Installs NuGet packages using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The id of the package to install.</param>
        /// <param name="settings">The settings.</param>
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
        /// Installs NuGet packages for the specified target.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageConfigPath">The target to install.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Install")]
        public static void NuGetInstallFromConfig(this ICakeContext context, FilePath packageConfigPath)
        {
            var settings = new NuGetInstallSettings();
            NuGetInstallFromConfig(context, packageConfigPath, settings);
        }

        /// <summary>
        /// Installs NuGet packages using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageConfigPath">The target to install.</param>
        /// <param name="settings">The settings.</param>
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
    }
}
