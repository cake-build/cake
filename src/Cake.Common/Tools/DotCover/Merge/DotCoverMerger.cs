// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotCover.Merge
{
    /// <summary>
    /// DotCover Merge merger.
    /// </summary>
    public sealed class DotCoverMerger : DotCoverTool<DotCoverMergeSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotCoverMerger" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotCoverMerger(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs DotCover Merge with the new parameter Syntax.
        /// </summary>
        /// <param name="sourceFiles">The list of DotCover coverage snapshot files.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="outputFile">The merged output file (optional).</param>
        public void Merge(IEnumerable<FilePath> sourceFiles, DotCoverMergeSettings settings, FilePath outputFile = null)
        {
            Merge(sourceFiles, outputFile, settings);
        }

        /// <summary>
        /// Runs DotCover Merge with the specified settings.
        /// </summary>
        /// <param name="sourceFiles">The list of DotCover coverage snapshot files.</param>
        /// <param name="outputFile">The merged output file.</param>
        /// <param name="settings">The settings.</param>
        public void Merge(
            IEnumerable<FilePath> sourceFiles,
            FilePath outputFile,
            DotCoverMergeSettings settings)
        {
            if (sourceFiles == null || !sourceFiles.Any())
            {
                throw new ArgumentNullException("sourceFiles");
            }
            ArgumentNullException.ThrowIfNull(settings);
            if (settings.UseLegacySyntax)
            {
                ArgumentNullException.ThrowIfNull(outputFile);
            }

            // Run the tool.
            Run(settings, GetArguments(sourceFiles, outputFile, settings));
        }

        private ProcessArgumentBuilder GetArguments(
            IEnumerable<FilePath> sourceFiles,
            FilePath outputFile,
            DotCoverMergeSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Command name - always lowercase 'merge' for both formats
            builder.Append("merge");

            // Set configuration file if exists.
            GetConfigurationFileArgument(settings).CopyTo(builder);

            if (settings.UseLegacySyntax)
            {
                BuildLegacyArguments(sourceFiles, outputFile, builder);
            }
            else
            {
                BuildNewArguments(sourceFiles, outputFile, builder, settings);
            }

            // Get Global settings
            GetArguments(settings).CopyTo(builder);

            return builder;
        }

        private void BuildNewArguments(IEnumerable<FilePath> sourceFiles, FilePath outputFile, ProcessArgumentBuilder builder, DotCoverMergeSettings settings)
        {
            // Set the Source files.
            var source = string.Join(',', sourceFiles.Select(s => s.MakeAbsolute(_environment).FullPath));
            builder.AppendSwitch("--snapshot-source", source.Quote());

            // Set the Output file.
            if (outputFile != null)
            {
                outputFile = outputFile.MakeAbsolute(_environment);
                builder.AppendSwitch("--snapshot-output", outputFile.FullPath.Quote());
            }

            // Set the Temporary directory.
            if (settings.TemporaryDirectory != null)
            {
                settings.TemporaryDirectory = settings.TemporaryDirectory.MakeAbsolute(_environment);
                builder.AppendSwitch("--temporary-directory", settings.TemporaryDirectory.FullPath.Quote());
            }
        }

        private void BuildLegacyArguments(IEnumerable<FilePath> sourceFiles, FilePath outputFile, ProcessArgumentBuilder builder)
        {
            // Set the Source files.
            var source = string.Join(';', sourceFiles.Select(s => s.MakeAbsolute(_environment).FullPath));
            builder.AppendSwitch("/Source", "=", source.Quote());

            // Set the Output file.
            outputFile = outputFile.MakeAbsolute(_environment);
            builder.AppendSwitch("/Output", "=", outputFile.FullPath.Quote());
        }
    }
}
