// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.MSpec
{
    /// <summary>
    /// The MSpec unit test runner.
    /// </summary>
    public sealed class MSpecRunner : Tool<MSpecSettings>
    {
        private readonly ICakeEnvironment _environment;
        private bool _useX86;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSpec.MSpecRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public MSpecRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs the tests in the specified assemblies, using the specified settings.
        /// </summary>
        /// <param name="assemblyPaths">The assembly paths.</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> assemblyPaths, MSpecSettings settings)
        {
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException(nameof(assemblyPaths));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(settings.OutputDirectory?.FullPath))
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

            _useX86 = settings.UseX86;

            var paths = assemblyPaths as FilePath[] ?? assemblyPaths.ToArray();

            Run(settings, GetArguments(paths, settings));
        }

        private ProcessArgumentBuilder GetArguments(IReadOnlyList<FilePath> assemblyPaths, MSpecSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            FilterTests(settings, builder);

            GeneralSettings(settings, builder);

            BuildServerSettings(settings, builder);

            ReportSettings(assemblyPaths, settings, builder);

            AssembliesToTest(assemblyPaths, builder);

            return builder;
        }

        private static void GeneralSettings(MSpecSettings settings, ProcessArgumentBuilder builder)
        {
            if (settings.TimeInfo)
            {
                builder.Append("-t");
            }

            if (settings.Silent)
            {
                builder.Append("-s");
            }

            if (settings.Progress)
            {
                builder.Append("-p");
            }

            if (settings.NoColor)
            {
                builder.Append("-c");
            }

            if (settings.Wait)
            {
                builder.Append("-w");
            }
        }

        private static void BuildServerSettings(MSpecSettings settings, ProcessArgumentBuilder builder)
        {
            if (settings.TeamCity)
            {
                builder.Append("--teamcity");
            }

            if (settings.NoTeamCity)
            {
                builder.Append("--no-teamcity-autodetect");
            }

            if (settings.AppVeyor)
            {
                builder.Append("--appveyor");
            }

            if (settings.NoAppVeyor)
            {
                builder.Append("--no-appveyor-autodetect");
            }
        }

        private void FilterTests(MSpecSettings settings, ProcessArgumentBuilder builder)
        {
            if (settings.Filters != null)
            {
                builder.Append("-f");
                builder.AppendQuoted(settings.Filters.MakeAbsolute(_environment).FullPath);
            }

            if (!string.IsNullOrWhiteSpace(settings.Include))
            {
                builder.Append("-i");
                builder.AppendQuoted(settings.Include);
            }

            if (!string.IsNullOrWhiteSpace(settings.Exclude))
            {
                builder.Append("-x");
                builder.AppendQuoted(settings.Exclude);
            }
        }

        private void AssembliesToTest(IReadOnlyList<FilePath> assemblyPaths, ProcessArgumentBuilder builder)
        {
            foreach (var assemblyPath in assemblyPaths)
            {
                builder.AppendQuoted(assemblyPath.MakeAbsolute(_environment).FullPath);
            }
        }

        private void ReportSettings(IReadOnlyList<FilePath> assemblyPaths, MSpecSettings settings, ProcessArgumentBuilder builder)
        {
            if (settings.HtmlReport)
            {
                var reportFileName = MSpecRunnerUtilities.GetReportFileName(assemblyPaths, settings);
                var assemblyFilename = reportFileName.AppendExtension(".html");
                var outputPath = settings.OutputDirectory.MakeAbsolute(_environment).GetFilePath(assemblyFilename);

                builder.Append("--html");
                builder.AppendQuoted(outputPath.FullPath);
            }

            if (settings.XmlReport)
            {
                var reportFileName = MSpecRunnerUtilities.GetReportFileName(assemblyPaths, settings);
                var assemblyFilename = reportFileName.AppendExtension(".xml");
                var outputPath = settings.OutputDirectory.MakeAbsolute(_environment).GetFilePath(assemblyFilename);

                builder.Append("--xml");
                builder.AppendQuoted(outputPath.FullPath);
            }
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "MSpec";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return _useX86 ? new[] { "mspec-x86-clr4.exe" } : new[] { "mspec-clr4.exe" };
        }
    }
}
