﻿using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotCover.Cover
{
    /// <summary>
    /// DotCover Coverer builder.
    /// </summary>
    public sealed class DotCoverCoverer : DotCoverTool<DotCoverCoverSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotCoverCoverer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        public DotCoverCoverer(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs DotCover Cover with the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="action">The action.</param>
        /// <param name="outputPath">The output file path.</param>
        /// <param name="settings">The settings.</param>
        public void Cover(ICakeContext context,
            Action<ICakeContext> action,
            FilePath outputPath,
            DotCoverCoverSettings settings)
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

        private ProcessArgumentBuilder GetArguments(
            DotCoverContext context,
            DotCoverCoverSettings settings,
            FilePath outputPath)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("Cover");

            // The target application to call.
            builder.AppendSwitch("/TargetExecutable", "=", context.FilePath.FullPath.Quote());

            // The arguments to the target application.
            if (context.Settings != null && context.Settings.Arguments != null)
            {
                var arguments = context.Settings.Arguments.Render();
                if (!string.IsNullOrWhiteSpace(arguments))
                {
                    arguments = arguments.Replace("\"", "\\\"");
                    builder.AppendSwitch("/TargetArguments", "=", arguments.Quote());
                }
            }

            // Set the output file.
            outputPath = outputPath.MakeAbsolute(_environment);
            builder.AppendSwitch("/Output", "=", outputPath.FullPath.Quote());

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

            // Filters
            if (settings.AttributeFilters.Count > 0)
            {
                var attributeFilters = string.Join(";", settings.AttributeFilters);
                builder.AppendSwitch("/AttributeFilters", "=", attributeFilters.Quote());
            }

            // DisableDefaultFilters
            if (settings.DisableDefaultFilters)
            {
                builder.Append("/DisableDefaultFilters");
            }

            return builder;
        }
    }
}