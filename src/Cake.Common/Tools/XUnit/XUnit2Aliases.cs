using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.XUnit
{
    /// <summary>
    /// Contains functionality related to running xUnit.net tests.
    /// </summary>
    [CakeAliasCategory("xUnit v2")]
    public static class XUnit2Aliases
    {
        /// <summary>
        /// Runs all xUnit.net v2 tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        [CakeMethodAlias]
        public static void XUnit2(this ICakeContext context, string pattern)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var assemblies = context.Globber.GetFiles(pattern);
            XUnit2(context, assemblies, new XUnit2Settings());
        }

        /// <summary>
        /// Runs all xUnit.net v2 tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void XUnit2(this ICakeContext context, string pattern, XUnit2Settings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var assemblies = context.Globber.GetFiles(pattern);
            XUnit2(context, assemblies, settings);
        }

        /// <summary>
        /// Runs all xUnit.net v2 tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        [CakeMethodAlias]
        public static void XUnit2(this ICakeContext context, IEnumerable<string> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            var paths = assemblies.Select(p => new FilePath(p));
            XUnit2(context, paths, new XUnit2Settings());
        }

        /// <summary>
        /// Runs all xUnit.net tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        [CakeMethodAlias]
        public static void XUnit2(this ICakeContext context, IEnumerable<FilePath> assemblies)
        {
            XUnit2(context, assemblies, new XUnit2Settings());
        }

        /// <summary>
        /// Runs all xUnit.net v2 tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void XUnit2(this ICakeContext context, IEnumerable<string> assemblies, XUnit2Settings settings)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            var paths = assemblies.Select(p => new FilePath(p));
            XUnit2(context, paths, settings);
        }

        /// <summary>
        /// Runs all xUnit.net v2 tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void XUnit2(this ICakeContext context, IEnumerable<FilePath> assemblies, XUnit2Settings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }

            var runner = new XUnit2Runner(context.FileSystem, context.Environment, context.Globber, context.ProcessRunner);
            foreach (var assembly in assemblies)
            {
                runner.Run(assembly, settings);
            }    
        }
    }
}
