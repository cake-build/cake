// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.OpenCover
{
    /// <summary>
    /// The OpenCover runner.
    /// </summary>
    public sealed class OpenCoverRunner : Tool<OpenCoverSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenCoverRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public OpenCoverRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs OpenCover with the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="action">The action.</param>
        /// <param name="outputPath">The output file path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(
            ICakeContext context,
            Action<ICakeContext> action,
            FilePath outputPath,
            OpenCoverSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            if (outputPath == null)
            {
                throw new ArgumentNullException("outputPath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            // Run the tool using the interceptor.
            var interceptor = InterceptAction(context, action);

            // Run the tool.
            Run(settings, GetArguments(interceptor, settings, outputPath));
        }

        private static OpenCoverContext InterceptAction(
            ICakeContext context,
            Action<ICakeContext> action)
        {
            var interceptor = new OpenCoverContext(context);
            action(interceptor);

            // Validate arguments.
            if (interceptor.FilePath == null)
            {
                throw new CakeException("No tool was started.");
            }

            return interceptor;
        }

        private ProcessArgumentBuilder GetArguments(
            OpenCoverContext context,
            OpenCoverSettings settings,
            FilePath outputPath)
        {
            var builder = new ProcessArgumentBuilder();

            // The target application to call.
            builder.AppendSwitch("-target", ":", context.FilePath.FullPath.Quote());

            // The arguments to the target application.
            if (context.Settings != null && context.Settings.Arguments != null)
            {
                var arguments = context.Settings.Arguments.Render();
                if (!string.IsNullOrWhiteSpace(arguments))
                {
                    arguments = arguments.Replace("\"", "\\\"");
                    builder.AppendSwitch("-targetargs", ":", arguments.Quote());
                }
            }

            // Filters
            if (settings.Filters.Count > 0)
            {
                var filters = string.Join(" ", settings.Filters);
                builder.AppendSwitch("-filter", ":", filters.Quote());
            }

            // Exclude by attribute
            if (settings.ExcludedAttributeFilters.Count > 0)
            {
                var filters = string.Join(";", settings.ExcludedAttributeFilters);
                builder.AppendSwitch("-excludebyattribute", ":", filters.Quote());
            }

            // Exclude by file
            if (settings.ExcludedFileFilters.Count > 0)
            {
                var filters = string.Join(";", settings.ExcludedFileFilters);
                builder.AppendSwitch("-excludebyfile", ":", filters.Quote());
            }

            if (settings.SkipAutoProps)
            {
                builder.Append("-skipautoprops");
            }

            builder.AppendSwitch("-register", ":", settings.Register);

            if (settings.ReturnTargetCodeOffset != null)
            {
                builder.AppendSwitch("-returntargetcode", ":", settings.ReturnTargetCodeOffset.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }

            // Set the output file.
            outputPath = outputPath.MakeAbsolute(_environment);
            builder.AppendSwitch("-output", ":", outputPath.FullPath.Quote());

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "OpenCover";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "OpenCover.Console.exe" };
        }
    }
}
