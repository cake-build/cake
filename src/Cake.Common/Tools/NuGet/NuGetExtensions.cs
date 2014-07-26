using Cake.Common.Tools.NuGet.Pack;
using Cake.Common.Tools.NuGet.Push;
using Cake.Common.Tools.NuGet.Restore;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet
{
    /// <summary>
    /// Contains functionality for working with NuGet.
    /// </summary>
    public static class NuGetExtensions
    {
        /// <summary>
        /// Creates a NuGet package using the specified Nuspec file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="nuspecFilePath">The nuspec file path.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Pack")]
        public static void NuGetPack(this ICakeContext context, FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            var packer = new NuGetPacker(context.FileSystem, context.Environment, 
                context.Globber, context.ProcessRunner, context.Log);
            packer.Pack(nuspecFilePath, settings);
        }

        /// <summary>
        /// Restores NuGet packages for the specified target.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="targetFilePath">The target to restore.</param>
        [CakeMethodAlias]
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
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Restore")]
        public static void NuGetRestore(this ICakeContext context, FilePath targetFilePath, NuGetRestoreSettings settings)
        {   
            var runner = new NuGetRestorer(context.FileSystem, context.Environment, context.Globber, context.ProcessRunner);
            runner.Restore(targetFilePath, settings);
        }

        /// <summary>
        /// Pushes a NuGet package to a NuGet server and publishes it.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageFilePath">The nupkg file path.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeNamespaceImport("Cake.Common.Tools.NuGet.Push")]
        public static void NuGetPush(this ICakeContext context, FilePath packageFilePath, NuGetPushSettings settings)
        {
            var packer = new NuGetPusher(context.FileSystem, context.Environment, context.Globber, context.ProcessRunner);
            packer.Push(packageFilePath, settings);
        }
    }
}
