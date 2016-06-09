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
    /// GitLink runner
    /// </summary>
    public sealed class GitLinkRunner : Tool<GitLinkSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitLinkRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public GitLinkRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Update pdb files to link all sources.
        /// </summary>
        /// <param name="repositoryRootPath">The path to the Root of the Repository to analyze.</param>
        /// <param name="settings">The settings.</param>
        public void Run(DirectoryPath repositoryRootPath, GitLinkSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (repositoryRootPath == null)
            {
                throw new ArgumentNullException("repositoryRootPath");
            }

            Run(settings, GetArguments(repositoryRootPath, settings));
        }

        private ProcessArgumentBuilder GetArguments(DirectoryPath repositoryRootPath, GitLinkSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.AppendQuoted(repositoryRootPath.MakeAbsolute(_environment).FullPath);

            if (!string.IsNullOrWhiteSpace(settings.RepositoryUrl))
            {
                builder.Append("-u");
                builder.AppendQuoted(settings.RepositoryUrl);
            }

            if (!string.IsNullOrWhiteSpace(settings.SolutionFileName))
            {
                builder.Append("-f");
                builder.AppendQuoted(settings.SolutionFileName);
            }

            if (!string.IsNullOrWhiteSpace(settings.Configuration))
            {
                builder.Append("-c");
                builder.AppendQuoted(settings.Configuration);
            }

            if (!string.IsNullOrWhiteSpace(settings.Platform))
            {
                builder.Append("-p");
                builder.AppendQuoted(settings.Platform);
            }

            if (!string.IsNullOrWhiteSpace(settings.Branch))
            {
                builder.Append("-b");
                builder.AppendQuoted(settings.Branch);
            }

            if (settings.LogFilePath != null)
            {
                builder.Append("-l");
                builder.AppendQuoted(settings.LogFilePath.MakeAbsolute(_environment).FullPath);
            }

            if (!string.IsNullOrWhiteSpace(settings.ShaHash))
            {
                builder.Append("-s");
                builder.AppendQuoted(settings.ShaHash);
            }

            if (settings.PdbDirectoryPath != null)
            {
                builder.Append("-d");
                builder.AppendQuoted(settings.PdbDirectoryPath.MakeAbsolute(_environment).FullPath);
            }

            if (settings.UsePowerShell)
            {
                builder.Append("-powershell");
            }

            if (settings.ErrorsAsWarnings)
            {
                builder.Append("-errorsaswarnings");
            }

            if (settings.SkipVerify)
            {
                builder.Append("-skipverify");
            }

            if (settings.IsDebug)
            {
                builder.Append("-debug");
            }

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
