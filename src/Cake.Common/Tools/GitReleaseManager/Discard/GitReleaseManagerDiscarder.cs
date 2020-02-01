// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitReleaseManager.Discard
{
    /// <summary>
    /// The GitReleaseManager Release Discarder used to discard releases.
    /// </summary>
    public sealed class GitReleaseManagerDiscarder : GitReleaseManagerTool<GitReleaseManagerDiscardSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitReleaseManagerDiscarder"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public GitReleaseManagerDiscarder(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Discards a Release using the specified settings.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="milestone">The milestone.</param>
        /// <param name="settings">The settings.</param>
        public void Discard(string token, string owner, string repository, string milestone, GitReleaseManagerDiscardSettings settings)
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

        private ProcessArgumentBuilder GetArguments(string token, string owner, string repository, string milestone, GitReleaseManagerDiscardSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("discard");

            builder.Append("--token");
            builder.AppendQuotedSecret(token);

            builder.Append("-o");
            builder.AppendQuoted(owner);

            builder.Append("-r");
            builder.AppendQuoted(repository);

            builder.Append("-m");
            builder.AppendQuoted(milestone);

            AddBaseArguments(settings, builder);

            return builder;
        }
    }
}
