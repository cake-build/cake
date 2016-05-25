using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.OpenCover
{
    /// <summary>
    /// Contains functionality related to OpenCover.
    /// </summary>
    [CakeAliasCategory("OpenCover")]
    public static class OpenCoverAliases
    {
        /// <summary>
        /// Runs <see href="https://github.com/OpenCover/opencover">OpenCover</see>
        /// for the specified action and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="action">The action to run OpenCover for.</param>
        /// <param name="outputFile">The OpenCover output file.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// OpenCover(tool => {
        ///   tool.XUnit2("./**/App.Tests.dll",
        ///     new XUnit2Settings {
        ///       ShadowCopy = false
        ///     });
        ///   },
        ///   new FilePath("./result.xml"),
        ///   new OpenCoverSettings()
        ///     .WithFilter("+[App]*")
        ///     .WithFilter("-[App.Tests]*"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void OpenCover(
            this ICakeContext context,
            Action<ICakeContext> action,
            FilePath outputFile,
            OpenCoverSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            // Create the OpenCover runner.
            var runner = new OpenCoverRunner(
                context.FileSystem, context.Environment,
                context.ProcessRunner, context.Tools);

            // Run OpenCover.
            runner.Run(context, action, outputFile, settings);
        }
    }
}
