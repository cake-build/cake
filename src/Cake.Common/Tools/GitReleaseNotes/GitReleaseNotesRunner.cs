// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitReleaseNotes
{
    /// <summary>
    /// The GitReleaseNotes runner.
    /// </summary>
    public sealed class GitReleaseNotesRunner : Tool<GitReleaseNotesSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitReleaseNotesRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public GitReleaseNotesRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs GitVersion and processes the results.
        /// </summary>
        /// <param name="outputFile">The output file.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath outputFile, GitReleaseNotesSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (outputFile == null)
            {
                throw new ArgumentNullException("outputFile");
            }

            Run(settings, GetArguments(outputFile, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath outputFile, GitReleaseNotesSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            if (settings.WorkingDirectory != null)
            {
                builder.AppendQuoted(settings.WorkingDirectory.FullPath);
            }

            builder.Append("/OutputFile");
            builder.AppendQuoted(outputFile.MakeAbsolute(_environment).FullPath);

            if (settings.Verbose)
            {
                builder.Append("/Verbose");
            }

            if (settings.IssueTracker.HasValue)
            {
                builder.Append("/IssueTracker");
                switch (settings.IssueTracker.Value)
                {
                    case GitReleaseNotesIssueTracker.BitBucket:
                        builder.Append("BitBucket");
                        break;
                    case GitReleaseNotesIssueTracker.GitHub:
                        builder.Append("GitHub");
                        break;
                    case GitReleaseNotesIssueTracker.Jira:
                        builder.Append("Jira");
                        break;
                    case GitReleaseNotesIssueTracker.YouTrack:
                        builder.Append("YouTrack");
                        break;
                }
            }

            if (settings.AllTags)
            {
                builder.Append("/AllTags");
            }

            if (!string.IsNullOrWhiteSpace(settings.RepoUserName))
            {
                builder.Append("/RepoUsername");
                builder.AppendQuoted(settings.RepoUserName);
            }

            if (!string.IsNullOrWhiteSpace(settings.RepoPassword))
            {
                builder.Append("/RepoPassword");
                builder.AppendQuoted(settings.RepoPassword);
            }

            if (!string.IsNullOrWhiteSpace(settings.RepoToken))
            {
                builder.Append("/RepoToken");
                builder.AppendQuoted(settings.RepoToken);
            }

            if (!string.IsNullOrWhiteSpace(settings.RepoUrl))
            {
                builder.Append("/RepoUrl");
                builder.AppendQuoted(settings.RepoUrl);
            }

            if (!string.IsNullOrWhiteSpace(settings.RepoBranch))
            {
                builder.Append("/RepoBranch");
                builder.AppendQuoted(settings.RepoBranch);
            }

            if (!string.IsNullOrWhiteSpace(settings.IssueTrackerUrl))
            {
                builder.Append("/IssueTrackerUrl");
                builder.AppendQuoted(settings.IssueTrackerUrl);
            }

            if (!string.IsNullOrWhiteSpace(settings.IssueTrackerUserName))
            {
                builder.Append("/IssueTrackerUsername");
                builder.AppendQuoted(settings.IssueTrackerUserName);
            }

            if (!string.IsNullOrWhiteSpace(settings.IssueTrackerPassword))
            {
                builder.Append("/IssueTrackerPassword");
                builder.AppendQuoted(settings.IssueTrackerPassword);
            }

            if (!string.IsNullOrWhiteSpace(settings.IssueTrackerToken))
            {
                builder.Append("/IssueTrackerToken");
                builder.AppendQuoted(settings.IssueTrackerToken);
            }

            if (!string.IsNullOrWhiteSpace(settings.IssueTrackerProjectId))
            {
                builder.Append("/IssueTrackerProjectId");
                builder.AppendQuoted(settings.IssueTrackerProjectId);
            }

            if (!string.IsNullOrWhiteSpace(settings.IssueTrackerFilter))
            {
                builder.Append("/IssueTrackerFilter");
                builder.AppendQuoted(settings.IssueTrackerFilter);
            }

            if (!string.IsNullOrWhiteSpace(settings.Categories))
            {
                builder.Append("/Categories");
                builder.AppendQuoted(settings.Categories);
            }

            if (!string.IsNullOrWhiteSpace(settings.Version))
            {
                builder.Append("/Version");
                builder.AppendQuoted(settings.Version);
            }

            if (settings.AllLabels)
            {
                builder.Append("/AllLabels");
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "GitReleaseNotes";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "GitReleaseNotes.exe", "grn.exe" };
        }
    }
}
