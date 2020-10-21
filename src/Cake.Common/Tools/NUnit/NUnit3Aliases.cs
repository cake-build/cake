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
    /// <para>Contains functionality related to running <see href="https://github.com/nunit/nunit">NUnit</see> v2 and v3 unit tests.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from nuget.org, or specify the ToolPath within the <see cref="NUnit3Settings" /> class:
    /// <code>
    /// #tool "nuget:?package=NUnit.ConsoleRunner"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("NUnit v3")]
    public static class NUnit3Aliases
    {
        /// <summary>
        /// Runs all NUnit unit tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <example>
        /// <code>
        /// NUnit3("./src/**/bin/Release/*.Tests.dll");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void NUnit3(this ICakeContext context, GlobPattern pattern)
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

            NUnit3(context, assemblies, new NUnit3Settings());
        }

        /// <summary>
        /// Runs all NUnit unit tests in the assemblies matching the specified pattern,
        /// using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// NUnit3("./src/**/bin/Release/*.Tests.dll", new NUnit3Settings {
        ///     NoResults = true
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void NUnit3(this ICakeContext context, GlobPattern pattern, NUnit3Settings settings)
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

            NUnit3(context, assemblies, settings);
        }

        /// <summary>
        /// Runs all NUnit unit tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <example>
        /// <code>
        /// NUnit3(new [] { "./src/Example.Tests/bin/Release/Example.Tests.dll" });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void NUnit3(this ICakeContext context, IEnumerable<string> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            var paths = assemblies.Select(p => new FilePath(p));
            NUnit3(context, paths, new NUnit3Settings());
        }

        /// <summary>
        /// Runs all NUnit unit tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <example>
        /// <code>
        /// var testAssemblies = GetFiles("./src/**/bin/Release/*.Tests.dll");
        /// NUnit3(testAssemblies);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void NUnit3(this ICakeContext context, IEnumerable<FilePath> assemblies)
        {
            NUnit3(context, assemblies, new NUnit3Settings());
        }

        /// <summary>
        /// Runs all NUnit unit tests in the specified assemblies,
        /// using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// NUnit3(new [] { "./src/Example.Tests/bin/Release/Example.Tests.dll" }, new NUnit3Settings {
        ///     NoResults = true
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void NUnit3(this ICakeContext context, IEnumerable<string> assemblies, NUnit3Settings settings)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            var paths = assemblies.Select(p => new FilePath(p));
            NUnit3(context, paths, settings);
        }

        /// <summary>
        /// Runs all NUnit unit tests in the specified assemblies,
        /// using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var testAssemblies = GetFiles("./src/**/bin/Release/*.Tests.dll");
        /// NUnit3(testAssemblies, new NUnit3Settings {
        ///     NoResults = true
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void NUnit3(this ICakeContext context, IEnumerable<FilePath> assemblies, NUnit3Settings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var runner = new NUnit3Runner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(assemblies, settings);
        }
    }
}