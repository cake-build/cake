using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.GitLink
{
    /// <summary>
    /// Contains functionality for working with GitLink.
    /// </summary>
    [CakeAliasCategory("GitTools")]
    public static class GitLinkAliases
    {
        /// <summary>
        /// Update pdb files to link all sources.
        /// This will allow anyone to step through the source code while debugging without a symbol source server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryRootPath">The Solution File to analyze.</param>
        /// <example>
        /// <code>
        /// GitLink("C:/temp/solution");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("GitLink")]
        public static void GitLink(this ICakeContext context, DirectoryPath repositoryRootPath)
        {
            GitLink(context, repositoryRootPath, new GitLinkSettings());
        }

        /// <summary>
        /// Update pdb files to link all sources, using specified settings.
        /// This will allow anyone to step through the source code while debugging without a symbol source server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryRootPath">The path to the Root of the Repository to analyze.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitLink("C:/temp/solution", new GitLinkSettings {
        ///     RepositoryUrl = new Uri("http://mydomain.com"),
        ///     Branch        = "master",
        ///     ShaHash       = "abcdef",
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("GitLink")]
        public static void GitLink(this ICakeContext context, DirectoryPath repositoryRootPath, GitLinkSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new GitLinkRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, context.Log);
            runner.Run(repositoryRootPath, settings);
        }
    }
}