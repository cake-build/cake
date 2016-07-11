// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.ReportUnit
{
    /// <summary>
    /// ReportUnit runner.
    /// </summary>
    public sealed class ReportUnitRunner : Tool<ReportUnitSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportUnitRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public ReportUnitRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Converts the reports from specified folder into human readable form according to the specified settings.
        /// </summary>
        /// <param name="inputFolder">The input folder.</param>
        /// <param name="outputFolder">The output folder.</param>
        /// <param name="settings">The ReportUnit settings.</param>
        public void Run(DirectoryPath inputFolder, DirectoryPath outputFolder, ReportUnitSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (inputFolder == null)
            {
                throw new ArgumentNullException("inputFolder");
            }

            Run(settings, GetArguments(inputFolder, outputFolder));
        }

        /// <summary>
        /// Converts the specified report into human readable form according to the specified settings.
        /// </summary>
        /// <param name="inputFile">The input file.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="settings">The ReportUnit settings.</param>
        public void Run(FilePath inputFile, FilePath outputFile, ReportUnitSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (inputFile == null)
            {
                throw new ArgumentNullException("inputFile");
            }

            if (outputFile == null)
            {
                throw new ArgumentNullException("outputFile");
            }

            Run(settings, GetArguments(inputFile, outputFile));
        }

        private ProcessArgumentBuilder GetArguments(DirectoryPath inputFolder, DirectoryPath outputFolder)
        {
            var builder = new ProcessArgumentBuilder();

            builder.AppendQuoted(inputFolder.MakeAbsolute(_environment).FullPath);

            if (outputFolder != null)
            {
                builder.AppendQuoted(outputFolder.MakeAbsolute(_environment).FullPath);
            }

            return builder;
        }

        private ProcessArgumentBuilder GetArguments(FilePath inputFile, FilePath outputFile)
        {
            var builder = new ProcessArgumentBuilder();

            builder.AppendQuoted(inputFile.MakeAbsolute(_environment).FullPath);

            builder.AppendQuoted(outputFile.MakeAbsolute(_environment).FullPath);

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "ReportUnit";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "ReportUnit.exe" };
        }
    }
}
