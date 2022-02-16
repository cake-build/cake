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

namespace Cake.Common.Tools.Fixie
{
    /// <summary>
    /// <para>Contains functionality related to running <see href="https://github.com/fixie/fixie">Fixie</see> tests.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from nuget.org, or specify the ToolPath within the <see cref="FixieSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=Fixie"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("Fixie")]
    public static class FixieAliases
    {
        /// <summary>
        /// Runs all Fixie tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <example>
        /// <code>
        /// Fixie("./src/UnitTests/*.dll");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        [CakeMethodAlias]
        public static void Fixie(this ICakeContext context, GlobPattern pattern)
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

            Fixie(context, assemblies, new FixieSettings());
        }

        /// <summary>
        /// Runs all Fixie tests in the assemblies matching the specified pattern,
        /// using the specified settings.
        /// </summary>
        /// <example>
        /// <code>
        /// Fixie("./src/UnitTests/*.dll", new FixieSettings {
        ///     NUnitXml = TestResult.xml
        ///     });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void Fixie(this ICakeContext context, GlobPattern pattern, FixieSettings settings)
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

            Fixie(context, assemblies, settings);
        }

        /// <summary>
        /// Runs all Fixie tests in the specified assemblies.
        /// </summary>
        /// <example>
        /// <code>
        /// var assemblies = new [] {
        ///     "UnitTests1.dll",
        ///     "UnitTests2.dll"
        /// };
        /// Fixie(assemblies);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        [CakeMethodAlias]
        public static void Fixie(this ICakeContext context, IEnumerable<string> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            var paths = assemblies.Select(p => new FilePath(p));
            Fixie(context, paths, new FixieSettings());
        }

        /// <summary>
        /// Runs all Fixie tests in the specified assemblies.
        /// </summary>
        /// <example>
        /// <code>
        /// var assemblies = GetFiles("./src/UnitTests/*.dll");
        /// Fixie(assemblies);
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// var assemblies = new [] {
        ///     "UnitTests1.dll",
        ///     "UnitTests2.dll"
        /// };
        /// Fixie(assemblies, new FixieSettings {
        ///     NUnitXml = TestResult.xml
        ///     });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void Fixie(this ICakeContext context, IEnumerable<string> assemblies, FixieSettings settings)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            var paths = assemblies.Select(p => new FilePath(p));
            Fixie(context, paths, settings);
        }

        /// <summary>
        /// Runs all Fixie tests in the specified assemblies,
        /// using the specified settings.
        /// </summary>
        /// <example>
        /// <code>
        /// var assemblies = GetFiles("./src/UnitTests/*.dll");
        /// Fixie(assemblies, new FixieSettings {
        ///     NUnitXml = TestResult.xml
        ///     });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void Fixie(this ICakeContext context, IEnumerable<FilePath> assemblies, FixieSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var runner = new FixieRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(assemblies, settings);
        }
    }
}