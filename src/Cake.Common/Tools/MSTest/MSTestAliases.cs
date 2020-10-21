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

namespace Cake.Common.Tools.MSTest
{
    /// <summary>
    /// <para>Contains functionality related to running <see href="https://msdn.microsoft.com/en-us/library/ms182486.aspx">MSTest</see> unit tests.</para>
    /// <para>
    /// In order to use the commands for this alias, MSTest will need to be installed on the machine where
    /// the Cake script is being executed.  This is typically achieved by having either Visual Studio installed, or by
    /// using the Microsoft Build Tools, for example, for <see href="https://www.microsoft.com/en-us/download/details.aspx?id=48159">2015</see>.
    /// </para>
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
        public static void MSTest(this ICakeContext context, GlobPattern pattern)
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
        public static void MSTest(this ICakeContext context, GlobPattern pattern, MSTestSettings settings)
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
                throw new ArgumentNullException(nameof(context));
            }
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException(nameof(assemblyPaths));
            }

            var runner = new MSTestRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(assemblyPaths, settings);
        }
    }
}