using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Tools.Fixie
{
    /// <summary>
    /// Contains functionality related to running Fixie tests.
    /// </summary>
    [CakeAliasCategory("Fixie")]
    public static class FixieAliases
    {
        /// <summary>
        /// Runs all Fixie tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        [CakeMethodAlias]
        public static void Fixie(this ICakeContext context, string pattern)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var assemblies = context.Globber.GetFiles(pattern).ToArray();
            if (assemblies.Length == 0)
            {
                context.Log.Verbose("The provided pattern did not match any files.");
                return;
            }

            Fixie(context, assemblies, new FixieSettings());
        }

        /// <summary>
        /// Runs all Fixie tests in the assemblies matching the specified pattern,
        /// using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void Fixie(this ICakeContext context, string pattern, FixieSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var assemblies = context.Globber.GetFiles(pattern).ToArray();
            if (assemblies.Length == 0)
            {
                context.Log.Verbose("The provided pattern did not match any files.");
                return;
            }

            Fixie(context, assemblies, settings);
        }

        /// <summary>
        /// Runs all Fixie tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        [CakeMethodAlias]
        public static void Fixie(this ICakeContext context, IEnumerable<string> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            var paths = assemblies.Select(p => new FilePath(p));
            Fixie(context, paths, new FixieSettings());
        }

        /// <summary>
        /// Runs all Fixie tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        [CakeMethodAlias]
        public static void Fixie(this ICakeContext context, IEnumerable<FilePath> assemblies)
        {
            Fixie(context, assemblies, new FixieSettings());
        }

        /// <summary>
        /// Runs all Fixie tests in the specified assemblies,
        /// using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void Fixie(this ICakeContext context, IEnumerable<string> assemblies, FixieSettings settings)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            var paths = assemblies.Select(p => new FilePath(p));
            Fixie(context, paths, settings);
        }

        /// <summary>
        /// Runs all Fixie tests in the specified assemblies,
        /// using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void Fixie(this ICakeContext context, IEnumerable<FilePath> assemblies, FixieSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }

            var runner = new FixieRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run(assemblies, settings);
        }
    }
}