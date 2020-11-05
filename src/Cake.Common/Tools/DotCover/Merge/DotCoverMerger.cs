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
            if (outputFile == null)
            {
                throw new ArgumentNullException("outputFile");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
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

            builder.Append("Merge");

            // Set configuration file if exists.
            GetConfigurationFileArgument(settings).CopyTo(builder);

            // Set the Source files.
            var source = string.Join(";", sourceFiles.Select(s => s.MakeAbsolute(_environment).FullPath));
            builder.AppendSwitch("/Source", "=", source.Quote());

            // Set the Output file.
            outputFile = outputFile.MakeAbsolute(_environment);
            builder.AppendSwitch("/Output", "=", outputFile.FullPath.Quote());

            // Get Global settings
            GetArguments(settings).CopyTo(builder);

            return builder;
        }
    }
}
