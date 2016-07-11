// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Common.Tools.SpecFlow.StepDefinitionReport;
using Cake.Common.Tools.SpecFlow.TestExecutionReport;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.SpecFlow
{
    /// <summary>
    /// <para>Contains functionality related to <see href="http://www.specflow.org/">SpecFlow</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from NuGet.org, or specify the ToolPath within the appropriate settings class:
    /// <code>
    /// #tool "nuget:?package=SpecFlow"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("SpecFlow")]
    public static class SpecFlowAliases
    {
        /// <summary>
        /// Creates a report that shows the usage and binding status of the steps for the entire project.
        /// You can use this report to find both unused code in the automation layer and scenario steps that have no definition yet.
        /// See <see href="https://github.com/techtalk/SpecFlow/wiki/Reporting#step-definition-report">SpecFlow Documentation</see> for more information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectFile">The path of the project file containing the feature files.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("StepDefinitionReport")]
        [CakeNamespaceImport("Cake.Common.Tools.SpecFlow.StepDefinitionReport")]
        public static void SpecFlowStepDefinitionReport(this ICakeContext context, FilePath projectFile)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            SpecFlowStepDefinitionReport(context, projectFile, new SpecFlowStepDefinitionReportSettings());
        }

        /// <summary>
        /// Creates a report that shows the usage and binding status of the steps for the entire project.
        /// You can use this report to find both unused code in the automation layer and scenario steps that have no definition yet.
        /// See <see href="https://github.com/techtalk/SpecFlow/wiki/Reporting#step-definition-report">SpecFlow Documentation</see> for more information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectFile">The path of the project file containing the feature files.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("StepDefinitionReport")]
        [CakeNamespaceImport("Cake.Common.Tools.SpecFlow.StepDefinitionReport")]
        public static void SpecFlowStepDefinitionReport(this ICakeContext context, FilePath projectFile, SpecFlowStepDefinitionReportSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new SpecFlowStepDefinitionReporter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(projectFile, settings ?? new SpecFlowStepDefinitionReportSettings());
        }

        /// <summary>
        /// Creates a formatted HTML report of a test execution.
        /// The report contains a summary about the executed tests and the result and also a detailed report for the individual scenario executions.
        /// See <see href="https://github.com/techtalk/SpecFlow/wiki/Reporting#test-execution-report">SpecFlow Documentation</see> for more information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="action">The action to run SpecFlow for. Supported actions are: MSTest, NUnit3 and XUnit2</param>
        /// <param name="projectFile">The path of the project file containing the feature files.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("TestExecutionReport")]
        [CakeNamespaceImport("Cake.Common.Tools.SpecFlow.TestExecutionReport")]
        public static void SpecFlowTestExecutionReport(
            this ICakeContext context,
            Action<ICakeContext> action,
            FilePath projectFile)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            SpecFlowTestExecutionReport(context, action, projectFile, new SpecFlowTestExecutionReportSettings());
        }

        /// <summary>
        /// Creates a formatted HTML report of a test execution.
        /// The report contains a summary about the executed tests and the result and also a detailed report for the individual scenario executions.
        /// See <see href="https://github.com/techtalk/SpecFlow/wiki/Reporting#test-execution-report">SpecFlow Documentation</see> for more information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="action">The action to run SpecFlow for. Supported actions are: MSTest, NUNit, NUNit3, XUnit and XUnit2</param>
        /// <param name="projectFile">The path of the project file containing the feature files.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("TestExecutionReport")]
        [CakeNamespaceImport("Cake.Common.Tools.SpecFlow.TestExecutionReport")]
        public static void SpecFlowTestExecutionReport(
            this ICakeContext context,
            Action<ICakeContext> action,
            FilePath projectFile,
            SpecFlowTestExecutionReportSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (settings == null)
            {
                settings = new SpecFlowTestExecutionReportSettings();
            }

            // Create the DotCover analyser.
            var runner = new SpecFlowTestExecutionReporter(
                context.FileSystem, context.Environment,
                context.ProcessRunner, context.Tools);

            // Run DotCover analyse.
            runner.Run(context, action, projectFile, settings);
        }
    }
}
