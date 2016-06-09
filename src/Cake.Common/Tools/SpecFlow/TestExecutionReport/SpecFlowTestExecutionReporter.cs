// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.SpecFlow.TestExecutionReport
{
    /// <summary>
    /// SpecFlow MSTest execution report runner.
    /// </summary>
    public sealed class SpecFlowTestExecutionReporter : SpecFlowTool<SpecFlowTestExecutionReportSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecFlowTestExecutionReporter" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public SpecFlowTestExecutionReporter(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs SpecFlow Test Execution Report with the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="action">The action.</param>
        /// <param name="projectFile">The project file path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(ICakeContext context,
            Action<ICakeContext> action,
            FilePath projectFile,
            SpecFlowTestExecutionReportSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            if (projectFile == null)
            {
                throw new ArgumentNullException("projectFile");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            // Run the tool using the interceptor.
            var interceptor = InterceptAction(context, action);

            // Get / Verify Arguments
            var builder = GetArguments(interceptor, settings, projectFile);

            // Execute the action
            try
            {
                action(context);
            }
            catch (CakeException e)
            {
                // Write warning to log
                context.Warning(e.Message);
            }

            // Run the tool.
            Run(settings, builder);
        }

        private static SpecFlowContext InterceptAction(
            ICakeContext context,
            Action<ICakeContext> action)
        {
            var interceptor = new SpecFlowContext(context);

            action(interceptor);

            // Validate arguments.
            if (interceptor.FilePath == null)
            {
                throw new CakeException("No tool was started.");
            }

            return interceptor;
        }

        private ProcessArgumentBuilder GetArguments(
            SpecFlowContext context,
            SpecFlowTestExecutionReportSettings settings,
            FilePath projectFile)
        {
            var builder = context.GetArguments(projectFile, _environment);

            // Get the SpecFlowSettings arguments
            AppendArguments(settings, builder);

            return builder;
        }
    }
}
