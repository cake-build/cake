// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.XUnit
{
    /// <summary>
    /// <para>Contains functionality related to running <see href="https://github.com/xunit/xunit">xunit</see> tests.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from NuGet.org, or specify the ToolPath within the <see cref="XUnitSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=xunit.runner.console&amp;version=2.2.0"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("xUnit")]
    public static class XUnitAliases
    {
        /// <summary>
        /// Runs all xUnit.net tests in the assemblies matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <example>
        /// <code>
        /// XUnit("./src/**/bin/Release/*.Tests.dll");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void XUnit(this ICakeContext context, string pattern)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
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
        /// <example>
        /// <code>
        /// XUnit("./src/**/bin/Release/*.Tests.dll",
        ///      new XUnitSettings {
        ///         HtmlReport = true,
        ///         OutputDirectory = "./build"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void XUnit(this ICakeContext context, string pattern, XUnitSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var assemblies = context.Globber.GetFiles(pattern);
            XUnit(context, assemblies, settings);
        }

        /// <summary>
        /// Runs all xUnit.net tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <example>
        /// <code>
        /// XUnit(new []{
        ///     "./src/Cake.Common.Tests/bin/Release/Cake.Common.Tests.dll",
        ///     "./src/Cake.Core.Tests/bin/Release/Cake.Core.Tests.dll",
        ///     "./src/Cake.NuGet.Tests/bin/Release/Cake.NuGet.Tests.dll",
        ///     "./src/Cake.Tests/bin/Release/Cake.Tests.dll"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void XUnit(this ICakeContext context, IEnumerable<string> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            var paths = assemblies.Select(p => new FilePath(p));
            XUnit(context, paths, new XUnitSettings());
        }

        /// <summary>
        /// Runs all xUnit.net tests in the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <example>
        /// <code>
        /// var testAssemblies = GetFiles("./src/**/bin/Release/*.Tests.dll");
        /// XUnit(testAssemblies);
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// XUnit(new []{
        ///     "./src/Cake.Common.Tests/bin/Release/Cake.Common.Tests.dll",
        ///     "./src/Cake.Core.Tests/bin/Release/Cake.Core.Tests.dll",
        ///     "./src/Cake.NuGet.Tests/bin/Release/Cake.NuGet.Tests.dll",
        ///     "./src/Cake.Tests/bin/Release/Cake.Tests.dll"
        ///      },
        ///      new XUnitSettings {
        ///         HtmlReport = true,
        ///         OutputDirectory = "./build"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void XUnit(this ICakeContext context, IEnumerable<string> assemblies, XUnitSettings settings)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
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
        /// <example>
        /// <code>
        /// var testAssemblies = GetFiles("./src/**/bin/Release/*.Tests.dll");
        /// XUnit(testAssemblies,
        ///      new XUnitSettings {
        ///         HtmlReport = true,
        ///         OutputDirectory = "./build"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void XUnit(this ICakeContext context, IEnumerable<FilePath> assemblies, XUnitSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var runner = new XUnitRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            foreach (var assembly in assemblies)
            {
                runner.Run(assembly, settings);
            }
        }
    }
}
