// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.XUnit
{
    /// <summary>
    /// The xUnit.net (v1) test runner.
    /// </summary>
    public sealed class XUnitRunner : Tool<XUnitSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="runner">The runner.</param>
        /// <param name="tools">The tool locator.</param>
        public XUnitRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner runner,
            IToolLocator tools) : base(fileSystem, environment, runner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs the tests in the specified assembly.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath assemblyPath, XUnitSettings settings)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException("assemblyPath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            // Make sure we got output directory set when generating reports.
            if (settings.OutputDirectory == null || string.IsNullOrWhiteSpace(settings.OutputDirectory.FullPath))
            {
                if (settings.HtmlReport)
                {
                    throw new CakeException("Cannot generate HTML report when no output directory has been set.");
                }
                if (settings.XmlReport)
                {
                    throw new CakeException("Cannot generate XML report when no output directory has been set.");
                }
            }

            Run(settings, GetArguments(assemblyPath, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath assemblyPath, XUnitSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Get the absolute path to the assembly.
            assemblyPath = assemblyPath.MakeAbsolute(_environment);

            // Add the assembly to build.
            builder.AppendQuoted(assemblyPath.FullPath);

            // No shadow copy?
            if (!settings.ShadowCopy)
            {
                builder.AppendQuoted("/noshadow");
            }

            // Generate HTML report?
            if (settings.HtmlReport)
            {
                var assemblyFilename = assemblyPath.GetFilename().AppendExtension(".html");
                var outputPath = settings.OutputDirectory.MakeAbsolute(_environment).GetFilePath(assemblyFilename);

                builder.AppendQuoted("/html");
                builder.AppendQuoted(outputPath.FullPath);
            }

            // Generate XML report?
            if (settings.XmlReport)
            {
                var assemblyFilename = assemblyPath.GetFilename().AppendExtension(".xml");
                var outputPath = settings.OutputDirectory.MakeAbsolute(_environment).GetFilePath(assemblyFilename);

                builder.AppendQuoted("/xml");
                builder.AppendQuoted(outputPath.FullPath);
            }

            // Silent mode?
            if (settings.Silent)
            {
                builder.Append("/silent");
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "xUnit.net (v1)";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "xunit.console.clr4.exe" };
        }
    }
}
