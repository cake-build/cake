﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Tools.MSTest
{
    /// <summary>
    /// Contains functionality related to running MSTest unit tests.
    /// </summary>
    [CakeAliasCategory("MSTest")]
    public static class MSTestAliases
    {
        /// <summary>
        /// Runs all MSTest unit tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <example>
        /// <code>
        /// MSTest("./Tests/*.UnitTests.dll");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        [CakeMethodAlias]
        public static void MSTest(this ICakeContext context, string pattern)
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

            MSTest(context, assemblies);
        }

        /// <summary>
        /// Runs all MSTest unit tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <example>
        /// <code>
        /// MSTest("./Tests/*.UnitTests.dll", new MSTestSettings() { NoIsolation = false });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void MSTest(this ICakeContext context, string pattern, MSTestSettings settings)
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

            MSTest(context, assemblies, settings);
        }

        /// <summary>
        /// Runs all MSTest unit tests in the specified assemblies.
        /// </summary>
        /// <example>
        /// <code>
        /// var paths = new List&lt;FilePath&gt;() { "./assemblydir1", "./assemblydir2" };
        /// MSTest(paths);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="assemblyPaths">The assembly paths.</param>
        [CakeMethodAlias]
        public static void MSTest(this ICakeContext context, IEnumerable<FilePath> assemblyPaths)
        {
            MSTest(context, assemblyPaths, new MSTestSettings());
        }

        /// <summary>
        /// Runs all MSTest unit tests in the specified assemblies.
        /// </summary>
        /// <example>
        /// <code>
        /// var paths = new List&lt;FilePath&gt;() { "./assemblydir1", "./assemblydir2" };
        /// MSTest(paths, new MSTestSettings() { NoIsolation = false });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="assemblyPaths">The assembly paths.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void MSTest(this ICakeContext context, IEnumerable<FilePath> assemblyPaths, MSTestSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException("assemblyPaths");
            }

            var runner = new MSTestRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run(assemblyPaths, settings);
        }
    }
}