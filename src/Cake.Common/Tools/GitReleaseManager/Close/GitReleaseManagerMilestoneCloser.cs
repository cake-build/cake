// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitReleaseManager.Close
{
    /// <summary>
    /// The GitReleaseManager Milestone Closer used to close milestones.
    /// </summary>
    public sealed class GitReleaseManagerMilestoneCloser : GitReleaseManagerTool<GitReleaseManagerCloseMilestoneSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitReleaseManagerMilestoneCloser"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public GitReleaseManagerMilestoneCloser(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Closes a milestone using the specified settings.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="milestone">The milestone.</param>
        /// <param name="settings">The settings.</param>
        public void Close(string userName, string password, string owner, string repository, string milestone, GitReleaseManagerCloseMilestoneSettings settings)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(owner))
            {
                throw new ArgumentNullException(nameof(owner));
            }

            if (string.IsNullOrWhiteSpace(repository))
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (string.IsNullOrWhiteSpace(milestone))
            {
                throw new ArgumentNullException(nameof(milestone));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(userName, password, owner, repository, milestone, settings));
        }

        /// <summary>
        /// Closes a milestone using the specified settings.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="milestone">The milestone.</param>
        /// <param name="settings">The settings.</param>
        public void Close(string token, string owner, string repository, string milestone, GitReleaseManagerCloseMilestoneSettings settings)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (string.IsNullOrWhiteSpace(owner))
            {
                throw new ArgumentNullException(nameof(owner));
            }

            if (string.IsNullOrWhiteSpace(repository))
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (string.IsNullOrWhiteSpace(milestone))
            {
                throw new ArgumentNullException(nameof(milestone));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(token, owner, repository, milestone, settings));
        }

        private ProcessArgumentBuilder GetArguments(string userName, string password, string owner, string repository, string milestone, GitReleaseManagerCloseMilestoneSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("close");

            builder.Append("-u");
            builder.AppendQuoted(userName);

            builder.Append("-p");
            builder.AppendQuotedSecret(password);

            ParseCommonArguments(builder, owner, repository, milestone);

            AddBaseArguments(settings, builder);

            return builder;
        }

        private ProcessArgumentBuilder GetArguments(string token, string owner, string repository, string milestone, GitReleaseManagerCloseMilestoneSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("close");

            builder.Append("--token");
            builder.AppendQuotedSecret(token);

            ParseCommonArguments(builder, owner, repository, milestone);

            AddBaseArguments(settings, builder);

            return builder;
        }

        private void ParseCommonArguments(ProcessArgumentBuilder builder, string owner, string repository, string milestone)
        {
            builder.Append("-o");
            builder.AppendQuoted(owner);

            builder.Append("-r");
            builder.AppendQuoted(repository);

            builder.Append("-m");
            builder.AppendQuoted(milestone);
        }
    }
}
