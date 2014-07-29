using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// Contains functionality related to running NUnit unit tests.
    /// </summary>
    [CakeAliasCategory("NUnit")]
    public static class NUnitExtensions
    {
        /// <summary>
        /// Runs all NUnit unit tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        [CakeMethodAlias]
        public static void NUnit(this ICakeContext context, string pattern)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            NUnit(context, assemblies, new NUnitSettings());
        }

        /// <summary>
        /// Runs all NUnit unit tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void NUnit(this ICakeContext context, string pattern, NUnitSettings settings)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            NUnit(context, assemblies, settings);
        }

        /// <summary>
        /// Runs all NUnit unit tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        [CakeMethodAlias]
        public static void NUnit(this ICakeContext context, IEnumerable<FilePath> assemblies)
        {
            NUnit(context, assemblies, new NUnitSettings());
        }

        /// <summary>
        /// Runs all NUnit unit tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void NUnit(this ICakeContext context, IEnumerable<FilePath> assemblies, NUnitSettings settings)
        {
            var runner = new NUnitRunner(context.FileSystem, context.Environment, context.Globber, context.ProcessRunner);            
            foreach (var assembly in assemblies)
            {
                runner.Run(assembly, settings);
            }
        }
    }
}
