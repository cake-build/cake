﻿using System;
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
    [CakeAliasCategory("xUnit")]
    public static class XUnitAliases
    {
        /// <summary>
        /// Runs all xUnit.net tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        [CakeMethodAlias]
        public static void XUnit(this ICakeContext context, string pattern)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var assemblies = context.Globber.GetFiles(pattern);
            XUnit(context, assemblies, new XUnitSettings());
        }

        /// <summary>
        /// Runs all xUnit.net tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void XUnit(this ICakeContext context, string pattern, XUnitSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var assemblies = context.Globber.GetFiles(pattern);
            XUnit(context, assemblies, settings);
        }

        /// <summary>
        /// Runs all xUnit.net tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        [CakeMethodAlias]
        public static void XUnit(this ICakeContext context, IEnumerable<string> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            var paths = assemblies.Select(p => new FilePath(p));
            XUnit(context, paths, new XUnitSettings());
        }

        /// <summary>
        /// Runs all xUnit.net tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        [CakeMethodAlias]
        public static void XUnit(this ICakeContext context, IEnumerable<FilePath> assemblies)
        {
            XUnit(context, assemblies, new XUnitSettings());
        }

        /// <summary>
        /// Runs all xUnit.net tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void XUnit(this ICakeContext context, IEnumerable<string> assemblies, XUnitSettings settings)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            var paths = assemblies.Select(p => new FilePath(p));
            XUnit(context, paths, settings);
        }

        /// <summary>
        /// Runs all xUnit.net tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void XUnit(this ICakeContext context, IEnumerable<FilePath> assemblies, XUnitSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }

            var runner = new XUnitRunner(context.FileSystem, context.Environment, context.Globber, context.ProcessRunner);
            foreach (var assembly in assemblies)
            {
                runner.Run(assembly, settings);
            }
        }
    }
}