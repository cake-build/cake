// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.InspectCode
{
    /// <summary>
    /// <para>Contains functionality related to ReSharper's <see href="https://www.jetbrains.com/help/resharper/2016.1/InspectCode.html">InspectCode</see> tool.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from NuGet.org, or specify the ToolPath within the <see cref="InspectCodeSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=JetBrains.ReSharper.CommandLineTools"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("ReSharper")]
    public static class InspectCodeAliases
    {
        /// <summary>
        /// Analyses the specified solution with Resharper's InspectCode.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution.</param>
        /// <example>
        /// <code>
        /// InspectCode("./src/MySolution.sln");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("InspectCode")]
        public static void InspectCode(this ICakeContext context, FilePath solution)
        {
            InspectCode(context, solution, new InspectCodeSettings());
        }

        /// <summary>
        /// Analyses the specified solution with Resharper's InspectCode,
        /// using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var buildOutputDirectory = Directory("./.build");
        /// var resharperReportsDirectory = buildOutputDirectory + Directory("_ReSharperReports");
        ///
        /// var msBuildProperties = new Dictionary&lt;string, string&gt;();
        /// msBuildProperties.Add("configuration", configuration);
        /// msBuildProperties.Add("platform", "AnyCPU");
        ///
        /// InspectCode("./MySolution.sln", new InspectCodeSettings {
        ///     SolutionWideAnalysis = true;
        ///     Profile = "./MySolution.sln.DotSettings";
        ///     MsBuildProperties = msBuildProperties;
        ///     OutputFile = resharperReportsDirectory + File("inspectcode-output.xml");
        ///     ThrowExceptionOnFindingViolations = true;
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("InspectCode")]
        public static void InspectCode(this ICakeContext context, FilePath solution, InspectCodeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new InspectCodeRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, context.Log);
            runner.Run(solution, settings);
        }

        /// <summary>
        /// Runs ReSharper's InspectCode using the specified config file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configFile">The config file.</param>
        /// <example>
        /// <code>
        /// InspectCodeFromConfig("./src/inspectcode.config");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("InspectCode")]
        public static void InspectCodeFromConfig(this ICakeContext context, FilePath configFile)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new InspectCodeRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, context.Log);
            runner.RunFromConfig(configFile);
        }
    }
}
