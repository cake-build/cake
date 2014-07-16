using System;
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
        public static void NuGetPack(this ICakeContext context, FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            var packer = new NuGetPacker(context.Environment, context.Globber, context.ProcessRunner);
            packer.Pack(nuspecFilePath, settings);
        }

        /// <summary>
        /// Restores NuGet packages in the specified solution.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The Visual Studio solution.</param>
        [CakeMethodAlias]
        public static void NuGetRestore(this ICakeContext context, FilePath solution)
        {
            context.NuGetRestore(solution, settings => { });
        }

        /// <summary>
        /// Restores NuGet packages in the specified solution.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution.</param>
        /// <param name="configurator">The configurator.</param>
        [CakeMethodAlias]
        public static void NuGetRestore(this ICakeContext context, FilePath solution, Action<NuGetRestoreSettings> configurator)
        {
            var settings = new NuGetRestoreSettings(solution);
            configurator(settings);
            context.NuGetRestore(settings);
        }

        /// <summary>
        /// Restores NuGet packages using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void NuGetRestore(this ICakeContext context, NuGetRestoreSettings settings)
        {   
            var runner = new NuGetRestorer(context.Environment, context.Globber, context.ProcessRunner);
            runner.Restore(settings);
        }
    }
}
