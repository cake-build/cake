using System;
using Cake.Common.Tools.GitReleaseManager.AddAssets;
using Cake.Common.Tools.GitReleaseManager.Close;
using Cake.Common.Tools.GitReleaseManager.Create;
using Cake.Common.Tools.GitReleaseManager.Export;
using Cake.Common.Tools.GitReleaseManager.Publish;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.GitReleaseManager
{
    /// <summary>
    /// Contains functionality for working with GitReleaseManager.
    /// </summary>
    [CakeAliasCategory("GitReleaseManager")]
    public static class GitReleaseManagerAliases
    {
        /// <summary>
        /// Creates a Package Release using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerCreate("user", "password", "owner", "repo", new GitReleaseManagerCreateSettings {
        ///     Milestone         = "0.1.0",
        ///     Prerelease        = false,
        ///     Assets            = "c:/temp/asset1.txt,c:/temp/asset2.txt",
        ///     TargetCommitish   = "master",
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        /// <example>
        /// <code>
        /// GitReleaseManagerCreate("user", "password", "owner", "repo", new GitReleaseManagerCreateSettings {
        ///     Name              = "0.1.0",
        ///     InputFilePath     = "c:/repo/releasenotes.md",
        ///     Prerelease        = false,
        ///     Assets            = "c:/temp/asset1.txt,c:/temp/asset2.txt",
        ///     TargetCommitish   = "master",
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Create")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Create")]
        public static void GitReleaseManagerCreate(this ICakeContext context, string userName, string password, string owner, string repository, GitReleaseManagerCreateSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new GitReleaseManagerToolResolver(context.FileSystem, context.Environment, context.Globber);
            var creator = new GitReleaseManagerCreator(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            creator.Create(userName, password, owner, repository, settings);
        }

        /// <summary>
        /// Add Assets to an existing release using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="tagName">The tag name.</param>
        /// <param name="assets">The assets.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerAddAssets("user", "password", "owner", "repo", "0.1.0", "c:/temp/asset1.txt,c:/temp/asset2.txt" new GitReleaseManagerAddAssetsSettings {
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("AddAssets")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.AddAssets")]
        public static void GitReleaseManagerAddAssets(this ICakeContext context, string userName, string password, string owner, string repository, string tagName, string assets, GitReleaseManagerAddAssetsSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new GitReleaseManagerToolResolver(context.FileSystem, context.Environment, context.Globber);
            var assetsAdder = new GitReleaseManagerAssetsAdder(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            assetsAdder.AddAssets(userName, password, owner, repository, tagName, assets, settings);
        }

        /// <summary>
        /// Closes the milestone associated with a release using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="milestone">The milestone.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerClose("user", "password", "owner", "repo", "0.1.0", new GitReleaseManagerCloseMilestoneSettings {
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Close")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Close")]
        public static void GitReleaseManagerClose(this ICakeContext context, string userName, string password, string owner, string repository, string milestone, GitReleaseManagerCloseMilestoneSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new GitReleaseManagerToolResolver(context.FileSystem, context.Environment, context.Globber);
            var milestoneCloser = new GitReleaseManagerMilestoneCloser(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            milestoneCloser.Close(userName, password, owner, repository, milestone, settings);
        }

        /// <summary>
        /// Publishes the release using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="tagName">The tag name.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerPublish("user", "password", "owner", "repo", "0.1.0", new GitReleaseManagerPublishSettings {
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Publish")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Publish")]
        public static void GitReleaseManagerPublish(this ICakeContext context, string userName, string password, string owner, string repository, string tagName, GitReleaseManagerPublishSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new GitReleaseManagerToolResolver(context.FileSystem, context.Environment, context.Globber);
            var publisher = new GitReleaseManagerPublisher(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            publisher.Publish(userName, password, owner, repository, tagName, settings);
        }

        /// <summary>
        /// Exports the release notes using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="fileOutputPath">The output file path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerExport("user", "password", "owner", "repo", "c:/temp/releasenotes.md", new GitReleaseManagerExportSettings {
        ///     TagName           = "0.1.0",
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Export")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Export")]
        public static void GitReleaseManagerExport(this ICakeContext context, string userName, string password, string owner, string repository, FilePath fileOutputPath, GitReleaseManagerExportSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var resolver = new GitReleaseManagerToolResolver(context.FileSystem, context.Environment, context.Globber);
            var publisher = new GitReleaseManagerExporter(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber, resolver);
            publisher.Export(userName, password, owner, repository, fileOutputPath, settings);
        }
    }
}