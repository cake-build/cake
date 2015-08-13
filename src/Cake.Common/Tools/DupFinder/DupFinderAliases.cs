using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.DupFinder
{
    /// <summary>
    ///  Contains functionality related to ReSharper's duplication finder
    /// </summary>
    [CakeAliasCategory("ReSharper")]
    public static class DupFinderAliases
    {
        /// <summary>
        /// Analyses the specified file with ReSharper's DupFinder.
        /// The file can either be a solution/project or a source file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to analyze.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinder(this ICakeContext context, FilePath file)
        {
            DupFinder(context, new[] { file });
        }

        /// <summary>
        /// Analyses the specified file with ReSharper's DupFinder using the specified settings.
        /// The file can either be a solution/project or a source file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to analyze.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinder(this ICakeContext context, FilePath file, DupFinderSettings settings)
        {
            DupFinder(context, new[] { file }, settings);
        }

        /// <summary>
        /// Analyses the specified projects with ReSharper's DupFinder.
        /// The files can either be solutions and projects or a source files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="files">The files to analyze.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinder(this ICakeContext context, IEnumerable<FilePath> files)
        {
            DupFinder(context, files, new DupFinderSettings());
        }

        /// <summary>
        /// Analyses the specified projects with ReSharper's DupFinder using the specified settings.
        /// The files can either be solutions and projects or a source files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="files">The files to analyze.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinder(this ICakeContext context, IEnumerable<FilePath> files, DupFinderSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new DupFinderRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run(files, settings);
        }

        /// <summary>
        /// Analyses all files matching the specified pattern with ReSharper's DupFinder.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinder(this ICakeContext context, string pattern)
        {
            DupFinder(context, pattern, new DupFinderSettings());
        }

        /// <summary>
        /// Analyses all files matching the specified pattern with ReSharper's DupFinder,
        /// using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinder(this ICakeContext context, string pattern, DupFinderSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            var sourceFiles = context.Globber.Match(pattern).OfType<FilePath>();

            var runner = new DupFinderRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run(sourceFiles, settings);
        }

        /// <summary>
        /// Runs ReSharper's DupFinder using the provided config file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configFile">The config file.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinderFromConfig(this ICakeContext context, FilePath configFile)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new DupFinderRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.RunFromConfig(configFile);
        }
    }
}