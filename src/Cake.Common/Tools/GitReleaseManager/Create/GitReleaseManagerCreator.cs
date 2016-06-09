// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitReleaseManager.Create
{
    /// <summary>
    /// The GitReleaseManager Release Creator used to create releases.
    /// </summary>
    public sealed class GitReleaseManagerCreator : GitReleaseManagerTool<GitReleaseManagerCreateSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitReleaseManagerCreator"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public GitReleaseManagerCreator(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Creates a Release using the specified and settings.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="settings">The settings.</param>
        public void Create(string userName, string password, string owner, string repository, GitReleaseManagerCreateSettings settings)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("userName");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(owner))
            {
                throw new ArgumentNullException("owner");
            }

            if (string.IsNullOrWhiteSpace(repository))
            {
                throw new ArgumentNullException("repository");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(userName, password, owner, repository, settings));
        }

        private ProcessArgumentBuilder GetArguments(string userName, string password, string owner, string repository, GitReleaseManagerCreateSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("create");

            builder.Append("-u");
            builder.AppendQuoted(userName);

            builder.Append("-p");
            builder.AppendQuotedSecret(password);

            builder.Append("-o");
            builder.AppendQuoted(owner);

            builder.Append("-r");
            builder.AppendQuoted(repository);

            // Milestone
            if (!string.IsNullOrWhiteSpace(settings.Milestone))
            {
                builder.Append("-m");
                builder.AppendQuoted(settings.Milestone);
            }

            // Name
            if (!string.IsNullOrWhiteSpace(settings.Name))
            {
                builder.Append("-n");
                builder.AppendQuoted(settings.Name);
            }

            // Input File Path
            if (settings.InputFilePath != null)
            {
                builder.Append("-i");
                builder.AppendQuoted(settings.InputFilePath.MakeAbsolute(_environment).FullPath);
            }

            // Prerelease?
            if (settings.Prerelease)
            {
                builder.Append("-e");
            }

            // Assets
            if (!string.IsNullOrWhiteSpace(settings.Assets))
            {
                builder.Append("-a");
                builder.AppendQuoted(settings.Assets);
            }

            // Target Commitish
            if (!string.IsNullOrWhiteSpace(settings.TargetCommitish))
            {
                builder.Append("-c");
                builder.AppendQuoted(settings.TargetCommitish);
            }

            // Target Directory
            if (settings.TargetDirectory != null)
            {
                builder.Append("-d");
                builder.AppendQuoted(settings.TargetDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Log File Path
            if (settings.LogFilePath != null)
            {
                builder.Append("-l");
                builder.AppendQuoted(settings.LogFilePath.MakeAbsolute(_environment).FullPath);
            }

            return builder;
        }
    }
}
