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

namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// <para>Contains functionality related to running <see href="https://github.com/nunit/nunit">NUnit</see> tests.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from nuget.org, or specify the ToolPath within the <see cref="NUnitSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=NUnit.Runners&amp;version=2.6.4"
    /// </code>
    /// </para>
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
        public static void NUnit(this ICakeContext context, GlobPattern pattern)
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
        public static void NUnit(this ICakeContext context, GlobPattern pattern, NUnitSettings settings)
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
                throw new ArgumentNullException(nameof(assemblies));
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
                throw new ArgumentNullException(nameof(assemblies));
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
                throw new ArgumentNullException(nameof(context));
            }
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var runner = new NUnitRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(assemblies, settings);
        }
    }
}