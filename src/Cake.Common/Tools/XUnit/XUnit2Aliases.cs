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

namespace Cake.Common.Tools.XUnit
{
    /// <summary>
    /// <para>Contains functionality related to running <see href="https://github.com/xunit/xunit">xunit</see> tests.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from nuget.org, or specify the ToolPath within the <see cref="XUnit2Settings" /> class:
    /// <code>
    /// #tool "nuget:?package=xunit.runner.console"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("xUnit v2")]
    public static class XUnit2Aliases
    {
        /// <summary>
        /// Runs all xUnit.net v2 tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <example>
        /// <code>
        /// XUnit2("./src/**/bin/Release/*.Tests.dll");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void XUnit2(this ICakeContext context, GlobPattern pattern)
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

            XUnit2(context, assemblies, new XUnit2Settings());
        }

        /// <summary>
        /// Runs all xUnit.net v2 tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// XUnit2("./src/**/bin/Release/*.Tests.dll",
        ///      new XUnit2Settings {
        ///         Parallelism = ParallelismOption.All,
        ///         HtmlReport = true,
        ///         NoAppDomain = true,
        ///         OutputDirectory = "./build"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void XUnit2(this ICakeContext context, GlobPattern pattern, XUnit2Settings settings)
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

            XUnit2(context, assemblies, settings);
        }

        /// <summary>
        /// Runs all xUnit.net v2 tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <example>
        /// <code>
        /// XUnit2(new []{
        ///     "./src/Cake.Common.Tests/bin/Release/Cake.Common.Tests.dll",
        ///     "./src/Cake.Core.Tests/bin/Release/Cake.Core.Tests.dll",
        ///     "./src/Cake.NuGet.Tests/bin/Release/Cake.NuGet.Tests.dll",
        ///     "./src/Cake.Tests/bin/Release/Cake.Tests.dll"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void XUnit2(this ICakeContext context, IEnumerable<string> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            var paths = assemblies.Select(p => new FilePath(p));
            XUnit2(context, paths, new XUnit2Settings());
        }

        /// <summary>
        /// Runs all xUnit.net tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <example>
        /// <code>
        /// var testAssemblies = GetFiles("./src/**/bin/Release/*.Tests.dll");
        /// XUnit2(testAssemblies);
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// XUnit2(new []{
        ///     "./src/Cake.Common.Tests/bin/Release/Cake.Common.Tests.dll",
        ///     "./src/Cake.Core.Tests/bin/Release/Cake.Core.Tests.dll",
        ///     "./src/Cake.NuGet.Tests/bin/Release/Cake.NuGet.Tests.dll",
        ///     "./src/Cake.Tests/bin/Release/Cake.Tests.dll"
        ///      },
        ///      new XUnit2Settings {
        ///         Parallelism = ParallelismOption.All,
        ///         HtmlReport = true,
        ///         NoAppDomain = true,
        ///         OutputDirectory = "./build"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void XUnit2(this ICakeContext context, IEnumerable<string> assemblies, XUnit2Settings settings)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
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
        /// <example>
        /// <code>
        /// var testAssemblies = GetFiles("./src/**/bin/Release/*.Tests.dll");
        /// XUnit2(testAssemblies,
        ///      new XUnit2Settings {
        ///         Parallelism = ParallelismOption.All,
        ///         HtmlReport = true,
        ///         NoAppDomain = true,
        ///         OutputDirectory = "./build"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void XUnit2(this ICakeContext context, IEnumerable<FilePath> assemblies, XUnit2Settings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var runner = new XUnit2Runner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(assemblies, settings);
        }
    }
}