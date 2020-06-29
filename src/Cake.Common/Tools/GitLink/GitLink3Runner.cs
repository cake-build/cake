// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitLink
{
    /// <summary>
    /// GitLink runner.
    /// </summary>
    public sealed class GitLink3Runner : Tool<GitLink3Settings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitLink3Runner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public GitLink3Runner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Update pdb file to link all sources.
        /// </summary>
        /// <param name="pdbFile">The path to the pdb file to link.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath pdbFile, GitLink3Settings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (pdbFile == null)
            {
                throw new ArgumentNullException(nameof(pdbFile));
            }

            Run(settings, GetArguments(pdbFile, settings));
        }

        /// <summary>
        /// Update pdb files to link all sources.
        /// </summary>
        /// <param name="pdbFiles">The file path collection for the pdb files to link.</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> pdbFiles, GitLink3Settings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (pdbFiles == null)
            {
                throw new ArgumentNullException(nameof(pdbFiles));
            }

            foreach (var pdbFile in pdbFiles)
            {
                Run(settings, GetArguments(pdbFile, settings));
            }
        }

        private ProcessArgumentBuilder GetArguments(FilePath pdbFilePath, GitLink3Settings settings)
        {
            var builder = new ProcessArgumentBuilder();

            if (!string.IsNullOrWhiteSpace(settings.RepositoryUrl))
            {
                builder.Append("-u");
                builder.AppendQuoted(settings.RepositoryUrl);
            }

            if (!string.IsNullOrWhiteSpace(settings.ShaHash))
            {
                builder.Append("--commit");
                builder.AppendQuoted(settings.ShaHash);
            }

            if (settings.UsePowerShell)
            {
                builder.Append("-m");
                builder.Append("Powershell");
            }

            if (settings.SkipVerify)
            {
                builder.Append("-s");
            }

            if (settings.BaseDir != null)
            {
                builder.Append("--baseDir");
                builder.AppendQuoted(settings.BaseDir.MakeAbsolute(_environment).FullPath);
            }

            builder.AppendQuoted(pdbFilePath.MakeAbsolute(_environment).FullPath);

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "GitLink";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "gitlink.exe" };
        }
    }
}