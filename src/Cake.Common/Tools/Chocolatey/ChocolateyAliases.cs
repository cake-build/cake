using System;
using Cake.Common.Tools.Chocolatey.Pack;
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
    }
}