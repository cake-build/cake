// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Tools.VSTest
{
    /// <summary>
    /// Contains functionality related to running VSTest unit tests.
    /// </summary>
    [CakeAliasCategory("VSTest")]
    public static class VSTestAliases
    {
        /// <summary>
        /// Runs all VSTest unit tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <example>
        /// <code>
        /// VSTest("./Tests/*.UnitTests.dll");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        [CakeMethodAlias]
        public static void VSTest(this ICakeContext context, GlobPattern pattern)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var assemblies = context.Globber.GetFiles(pattern).ToArray();
            if (assemblies.Length == 0)
            {
                context.Log.Verbose("The provided pattern did not match any files.");
                return;
            }

            VSTest(context, assemblies);
        }

        /// <summary>
        /// Runs all VSTest unit tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <example>
        /// <code>
        /// VSTest("./Tests/*.UnitTests.dll", new VSTestSettings() { Logger = VSTestLogger.Trx });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void VSTest(this ICakeContext context, GlobPattern pattern, VSTestSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var assemblies = context.Globber.GetFiles(pattern).ToArray();
            if (assemblies.Length == 0)
            {
                context.Log.Verbose("The provided pattern did not match any files.");
                return;
            }

            VSTest(context, assemblies, settings);
        }

        /// <summary>
        /// Runs all VSTest unit tests in the specified assemblies.
        /// </summary>
        /// <example>
        /// <code>
        /// var paths = new List&lt;FilePath&gt;() { "./assemblydir1", "./assemblydir2" };
        /// VSTest(paths);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="assemblyPaths">The assembly paths.</param>
        [CakeMethodAlias]
        public static void VSTest(this ICakeContext context, IEnumerable<FilePath> assemblyPaths)
        {
            VSTest(context, assemblyPaths, new VSTestSettings());
        }

        /// <summary>
        /// Runs all VSTest unit tests in the specified assemblies.
        /// </summary>
        /// <example>
        /// <code>
        /// var paths = new List&lt;FilePath&gt;() { "./assemblydir1", "./assemblydir2" };
        /// VSTest(paths, new VSTestSettings() { InIsolation = true });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="assemblyPaths">The assembly paths.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void VSTest(this ICakeContext context, IEnumerable<FilePath> assemblyPaths, VSTestSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException(nameof(assemblyPaths));
            }

            var runner = new VSTestRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(assemblyPaths, settings);
        }
    }
}