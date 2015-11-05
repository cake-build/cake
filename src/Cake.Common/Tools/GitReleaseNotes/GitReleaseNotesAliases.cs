using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.GitReleaseNotes
{
    /// <summary>
    ///  Contains functionality related to GitReleaseNotes
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

            var runner = new GitReleaseNotesRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run(outputFile, settings);
        }
    }
}