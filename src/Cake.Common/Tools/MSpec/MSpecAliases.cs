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

namespace Cake.Common.Tools.MSpec
{
    /// <summary>
    /// <para>Contains functionality related to running <see href="https://github.com/machine/machine.specifications">Machine.Specifications</see> tests.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from nuget.org, or specify the ToolPath within the <see cref="MSpecSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=Machine.Specifications.Runner.Console"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("MSpec")]
    public static class MSpecAliases
    {
        /// <summary>
        /// Runs all MSpec tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <example>
        /// <code>
        /// MSpec("./src/**/bin/Release/*.Tests.dll");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void MSpec(this ICakeContext context, GlobPattern pattern)
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

            MSpec(context, assemblies, new MSpecSettings());
        }

        /// <summary>
        /// Runs all MSpec tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// MSpec("./src/**/bin/Release/*.Tests.dll",
        ///      new MSpecSettings {
        ///         Parallelism = ParallelismOption.All,
        ///         HtmlReport = true,
        ///         NoAppDomain = true,
        ///         OutputDirectory = "./build"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void MSpec(this ICakeContext context, GlobPattern pattern, MSpecSettings settings)
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

            MSpec(context, assemblies, settings);
        }

        /// <summary>
        /// Runs all MSpec tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <example>
        /// <code>
        /// MSpec(new []{
        ///     "./src/Cake.Common.Tests/bin/Release/Cake.Common.Tests.dll",
        ///     "./src/Cake.Core.Tests/bin/Release/Cake.Core.Tests.dll",
        ///     "./src/Cake.NuGet.Tests/bin/Release/Cake.NuGet.Tests.dll",
        ///     "./src/Cake.Tests/bin/Release/Cake.Tests.dll"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void MSpec(this ICakeContext context, IEnumerable<string> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            var paths = assemblies.Select(p => new FilePath(p));
            MSpec(context, paths, new MSpecSettings());
        }

        /// <summary>
        /// Runs all MSpec tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <example>
        /// <code>
        /// var testAssemblies = GetFiles("./src/**/bin/Release/*.Tests.dll");
        /// MSpec(testAssemblies);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void MSpec(this ICakeContext context, IEnumerable<FilePath> assemblies)
        {
            MSpec(context, assemblies, new MSpecSettings());
        }

        /// <summary>
        /// Runs all MSpec tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// MSpec(new []{
        ///     "./src/Cake.Common.Tests/bin/Release/Cake.Common.Tests.dll",
        ///     "./src/Cake.Core.Tests/bin/Release/Cake.Core.Tests.dll",
        ///     "./src/Cake.NuGet.Tests/bin/Release/Cake.NuGet.Tests.dll",
        ///     "./src/Cake.Tests/bin/Release/Cake.Tests.dll"
        ///      },
        ///      new MSpecSettings {
        ///         Parallelism = ParallelismOption.All,
        ///         HtmlReport = true,
        ///         NoAppDomain = true,
        ///         OutputDirectory = "./build"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void MSpec(this ICakeContext context, IEnumerable<string> assemblies, MSpecSettings settings)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            var paths = assemblies.Select(p => new FilePath(p));
            MSpec(context, paths, settings);
        }

        /// <summary>
        /// Runs all MSpec tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var testAssemblies = GetFiles("./src/**/bin/Release/*.Tests.dll");
        /// MSpec(testAssemblies,
        ///      new MSpecSettings {
        ///         Parallelism = ParallelismOption.All,
        ///         HtmlReport = true,
        ///         NoAppDomain = true,
        ///         OutputDirectory = "./build"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void MSpec(this ICakeContext context, IEnumerable<FilePath> assemblies, MSpecSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var runner = new MSpecRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(assemblies, settings);
        }
    }
}
