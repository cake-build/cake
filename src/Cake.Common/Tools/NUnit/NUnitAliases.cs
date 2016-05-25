using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// Contains functionality related to running NUnit unit tests.
    /// </summary>
    [CakeAliasCategory("NUnit")]
    public static class NUnitAliases
    {
        /// <summary>
        /// Runs all NUnit unit tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <example>
        /// <code>
        /// NUnit("./src/UnitTests/*.dll");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        [CakeMethodAlias]
        public static void NUnit(this ICakeContext context, string pattern)
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

            NUnit(context, assemblies, new NUnitSettings());
        }

        /// <summary>
        /// Runs all NUnit unit tests in the assemblies matching the specified pattern,
        /// using the specified settings.
        /// </summary>
        /// <example>
        /// <code>
        /// NUnit("./src/UnitTests/*.dll", new NUnitSettings {
        ///     Timeout = 4000, 
        ///     StopOnError = true
        ///     });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void NUnit(this ICakeContext context, string pattern, NUnitSettings settings)
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

            NUnit(context, assemblies, settings);
        }

        /// <summary>
        /// Runs all NUnit unit tests in the specified assemblies.
        /// </summary>
        /// <example>
        /// <code>
        /// var assemblies = new [] {
        ///     "UnitTests1.dll",
        ///     "UnitTests2.dll"
        /// };
        /// NUnit(assemblies);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        [CakeMethodAlias]
        public static void NUnit(this ICakeContext context, IEnumerable<string> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            var paths = assemblies.Select(p => new FilePath(p));
            NUnit(context, paths, new NUnitSettings());
        }

        /// <summary>
        /// Runs all NUnit unit tests in the specified assemblies.
        /// </summary>
        /// <example>
        /// <code>
        /// var assemblies = GetFiles("./src/UnitTests/*.dll");
        /// NUnit(assemblies);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        [CakeMethodAlias]
        public static void NUnit(this ICakeContext context, IEnumerable<FilePath> assemblies)
        {
            NUnit(context, assemblies, new NUnitSettings());
        }

        /// <summary>
        /// Runs all NUnit unit tests in the specified assemblies,
        /// using the specified settings.
        /// </summary>
        /// <example>
        /// <code>
        /// var assemblies = new [] {
        ///     "UnitTests1.dll",
        ///     "UnitTests2.dll"
        /// };
        /// NUnit(assemblies, new NUnitSettings {
        ///     Timeout = 4000,
        ///     StopOnError = true
        ///     });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void NUnit(this ICakeContext context, IEnumerable<string> assemblies, NUnitSettings settings)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            var paths = assemblies.Select(p => new FilePath(p));
            NUnit(context, paths, settings);
        }

        /// <summary>
        /// Runs all NUnit unit tests in the specified assemblies,
        /// using the specified settings.
        /// </summary>
        /// <example>
        /// <code>
        /// var assemblies = GetFiles(""./src/UnitTests/*.dll"");
        /// NUnit(assemblies, new NUnitSettings {
        ///     Timeout = 4000,
        ///     StopOnError = true
        ///     });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void NUnit(this ICakeContext context, IEnumerable<FilePath> assemblies, NUnitSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }

            var runner = new NUnitRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(assemblies, settings);
        }
    }
}