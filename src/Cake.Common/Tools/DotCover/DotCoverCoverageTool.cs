// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotCover
{
    /// <summary>
    /// DotCover Coverage tool.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public abstract class DotCoverCoverageTool<TSettings> : DotCoverTool<TSettings> where TSettings : DotCoverCoverageSettings
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotCoverCoverageTool{TSettings}" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotCoverCoverageTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Get arguments from the target executable.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="action">The action to run DotCover for.</param>
        /// <returns>The process arguments.</returns>
        protected ProcessArgumentBuilder GetTargetArguments(ICakeContext context, Action<ICakeContext> action)
        {
            // Run the tool using the interceptor.
            var targetContext = InterceptAction(context, action);

            var builder = new ProcessArgumentBuilder();

            // The target application to call.
            builder.AppendSwitch("/TargetExecutable", "=", targetContext.FilePath.FullPath.Quote());

            // The arguments to the target application.
            if (targetContext.Settings != null && targetContext.Settings.Arguments != null)
            {
                var arguments = targetContext.Settings.Arguments.Render();
                if (!string.IsNullOrWhiteSpace(arguments))
                {
                    arguments = arguments.Replace("\"", "\\\"");
                    builder.AppendSwitch("/TargetArguments", "=", arguments.Quote());
                }
            }

            return builder;
        }

        /// <summary>
        /// Get arguments from coverage settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The process arguments.</returns>
        protected ProcessArgumentBuilder GetCoverageArguments(DotCoverCoverageSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // TargetWorkingDir
            if (settings.TargetWorkingDir != null)
            {
                builder.AppendSwitch("/TargetWorkingDir", "=", settings.TargetWorkingDir.MakeAbsolute(_environment).FullPath.Quote());
            }

            // Scope
            if (settings.Scope.Count > 0)
            {
                var scope = string.Join(";", settings.Scope);
                builder.AppendSwitch("/Scope", "=", scope.Quote());
            }

            // Filters
            if (settings.Filters.Count > 0)
            {
                var filters = string.Join(";", settings.Filters);
                builder.AppendSwitch("/Filters", "=", filters.Quote());
            }

            // AttributeFilters
            if (settings.AttributeFilters.Count > 0)
            {
                var attributeFilters = string.Join(";", settings.AttributeFilters);
                builder.AppendSwitch("/AttributeFilters", "=", attributeFilters.Quote());
            }

            // Filters
            if (settings.ProcessFilters.Count > 0)
            {
                var processFilters = string.Join(";", settings.ProcessFilters);
                builder.AppendSwitch("/ProcessFilters", "=", processFilters.Quote());
            }

            // DisableDefaultFilters
            if (settings.DisableDefaultFilters)
            {
                builder.Append("/DisableDefaultFilters");
            }

            return builder;
        }

        private static DotCoverContext InterceptAction(
            ICakeContext context,
            Action<ICakeContext> action)
        {
            var interceptor = new DotCoverContext(context);
            action(interceptor);

            // Validate arguments.
            if (interceptor.FilePath == null)
            {
                throw new CakeException("No tool was started.");
            }

            return interceptor;
        }
    }
}