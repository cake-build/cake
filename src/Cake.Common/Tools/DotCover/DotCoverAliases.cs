using System;
using Cake.Common.Tools.DotCover.Analyse;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotCover
{
    /// <summary>
    /// Contains functionality related to DotCover.
    /// </summary>
    [CakeAliasCategory("DotCover")]
    public static class DotCoverAliases
    {
        /// <summary>
        /// Runs <see href="https://www.jetbrains.com/dotcover/help/dotCover__Console_Runner_Commands.html#analyse">DotCover Analyse</see>
        /// for the specified action and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="action">The action to run DotCover for.</param>
        /// <param name="outputFile">The DotCover output file.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotCoverAnalyse(tool => {
        ///   tool.XUnit2("./**/App.Tests.dll",
        ///     new XUnit2Settings {
        ///       ShadowCopy = false
        ///     });
        ///   },
        ///   new FilePath("./result.xml"),
        ///   new DotCoverAnalyseSettings()
        ///     .WithFilter("+:App")
        ///     .WithFilter("-:App.Tests"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Analyse")]
        [CakeNamespaceImport("Cake.Common.Tools.DotCover.Analyse")]
        public static void DotCoverAnalyse(
            this ICakeContext context,
            Action<ICakeContext> action,
            FilePath outputFile,
            DotCoverAnalyseSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (settings == null)
            {
                settings = new DotCoverAnalyseSettings();
            }

            // Create the DotCover analyser.
            var analyser = new DotCoverAnalyser(
                context.FileSystem, context.Environment,
                context.ProcessRunner, context.Globber);

            // Run DotCover analyse.
            analyser.Analyse(context, action, outputFile, settings);
        }
    }
}