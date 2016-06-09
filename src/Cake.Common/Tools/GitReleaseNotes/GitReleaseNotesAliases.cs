// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.GitReleaseNotes
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/gittools/gitreleasenotes">GitReleaseNotes</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from NuGet.org, or specify the ToolPath within the <see cref="GitReleaseNotesSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=GitReleaseNotes"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("GitReleaseNotes")]
    public static class GitReleaseNotesAliases
    {
        /// <summary>
        /// Generates a set of release notes based on the commit history of the repository and specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseNotes("c:/temp/releasenotes.md", new GitReleaseNotesSettings {
        ///     WorkingDirectory         = "c:/temp",
        ///     Verbose                  = true,
        ///     IssueTracker             = IssueTracker.GitHub,
        ///     AllTags                  = true,
        ///     RepoUserName             = "bob",
        ///     RepoPassword             = "password",
        ///     RepoUrl                  = "http://myrepo.co.uk",
        ///     RepoBranch               = "master",
        ///     IssueTrackerUrl          = "http://myissuetracker.co.uk",
        ///     IssueTrackerUserName     = "bob",
        ///     IssueTrackerPassword     = "password",
        ///     IssueTrackerProjectId    = "1234",
        ///     Categories               = "Category1",
        ///     Version                  = "1.2.3.4",
        ///     AllLabels                = true
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("GitReleaseNotes")]
        public static void GitReleaseNotes(this ICakeContext context, FilePath outputFile, GitReleaseNotesSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new GitReleaseNotesRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(outputFile, settings);
        }
    }
}
