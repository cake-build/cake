// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://octopus.com/">Octopus Deploy</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from nuget.org, or specify the ToolPath within the appropriate settings class:
    /// <code>
    /// #tool "nuget:?package=OctopusTools"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("Octopus Deploy")]
    public static class OctopusDeployAliases
    {
        /// <summary>
        /// Creates a release for the specified Octopus Deploy Project.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="projectName">The name of the project.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     // Minimum required
        ///     OctoCreateRelease(projectNameOnServer, new CreateReleaseSettings {
        ///         Server = "http://octopus-deploy.example",
        ///         ApiKey = "API-XXXXXXXXXXXXXXXXXXXX"
        ///     });
        ///
        ///     OctoCreateRelease(projectNameOnServer, new CreateReleaseSettings {
        ///         Server = "http://octopus-deploy.example",
        ///         Username = "DeployUser",
        ///         Password = "a-very-secure-password"
        ///     });
        ///
        ///     OctoCreateRelease(projectNameOnServer, new CreateReleaseSettings {
        ///         ConfigurationFile = @"C:\OctopusDeploy.config"
        ///     });
        ///
        ///     // Additional Options
        ///     OctoCreateRelease(projectNameOnServer, new CreateReleaseSettings {
        ///         ToolPath = "./tools/OctopusTools/Octo.exe"
        ///         EnableDebugLogging = true,
        ///         IgnoreSslErrors = true,
        ///         EnableServiceMessages = true, // Enables teamcity services messages when logging
        ///         ReleaseNumber = "1.8.2",
        ///         DefaultPackageVersion = "1.0.0.0", // All packages in the release should be 1.0.0.0
        ///         Packages = new Dictionary&lt;string, string&gt;
        ///                     {
        ///                         { "PackageOne", "1.0.2.3" },
        ///                         { "PackageTwo", "5.2.3" }
        ///                     },
        ///         PackagesFolder = @"C:\MyOtherNuGetFeed",
        ///
        ///         // One or the other
        ///         ReleaseNotes = "Version 2.0 \n What a milestone we have ...",
        ///         ReleaseNotesFile = "./ReleaseNotes.md",
        ///
        ///         IgnoreExisting = true // if this release number already exists, ignore it
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void OctoCreateRelease(this ICakeContext context, string projectName, CreateReleaseSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var packer = new OctopusDeployReleaseCreator(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            packer.CreateRelease(projectName, settings);
        }

        /// <summary>
        /// Pushes the specified package to the Octopus Deploy repository.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="server">The Octopus server URL.</param>
        /// <param name="apiKey">The user's API key.</param>
        /// <param name="packagePath">Path to the package.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void OctoPush(this ICakeContext context, string server, string apiKey, FilePath packagePath, OctopusPushSettings settings)
        {
            OctoPush(context, server, apiKey, new[] { packagePath }, settings);
        }

        /// <summary>
        /// Pushes the specified packages to the Octopus Deploy repository.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="server">The Octopus server URL.</param>
        /// <param name="apiKey">The user's API key.</param>
        /// <param name="packagePaths">Paths to the packages.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void OctoPush(this ICakeContext context, string server, string apiKey, IEnumerable<FilePath> packagePaths, OctopusPushSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (packagePaths == null)
            {
                throw new ArgumentNullException(nameof(packagePaths));
            }

            var pusher = new OctopusDeployPusher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            pusher.PushPackage(server, apiKey, packagePaths.ToArray(), settings);
        }

        /// <summary>
        /// Packs the specified folder into an Octopus Deploy package.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="id">The package ID.</param>
        [CakeMethodAlias]
        public static void OctoPack(this ICakeContext context, string id)
        {
            OctoPack(context, id, null);
        }

        /// <summary>
        /// Packs the specified folder into an Octopus Deploy package.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="id">The package ID.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void OctoPack(this ICakeContext context, string id, OctopusPackSettings settings = null)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var packer = new OctopusDeployPacker(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            packer.Pack(id, settings);
        }

        /// <summary>
        /// Deploys the specified already existing release into a specified environment
        /// See <see href="http://docs.octopusdeploy.com/display/OD/Deploying+releases">Octopus Documentation</see> for more details.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="server">The Octopus server URL.</param>
        /// <param name="apiKey">The user's API key.</param>
        /// <param name="projectName">Name of the target project.</param>
        /// <param name="deployTo">Target environment name.</param>
        /// <param name="releaseNumber">Version number of the release to deploy. Specify "latest" for the latest release.</param>
        /// <param name="settings">Deployment settings.</param>
        /// <example>
        /// <code>
        ///     // bare minimum
        ///     OctoDeployRelease("http://octopus-deploy.example", "API-XXXXXXXXXXXXXXXXXXXX", "MyGreatProject", "Testing", "2.1.15-RC" new OctopusDeployReleaseDeploymentSettings());
        ///
        ///     // All of deployment arguments
        ///     OctoDeployRelease("http://octopus-deploy.example", "API-XXXXXXXXXXXXXXXXXXXX", "MyGreatProject", "Testing", "2.1.15-RC" new OctopusDeployReleaseDeploymentSettings {
        ///         ShowProgress = true,
        ///         ForcePackageDownload = true,
        ///         WaitForDeployment = true,
        ///         DeploymentTimeout = TimeSpan.FromMinutes(1),
        ///         CancelOnTimeout = true,
        ///         DeploymentChecksLeapCycle = TimeSpan.FromMinutes(77),
        ///         GuidedFailure = true,
        ///         SpecificMachines = new string[] { "Machine1", "Machine2" },
        ///         Force = true,
        ///         SkipSteps = new[] { "Step1", "Step2" },
        ///         NoRawLog = true,
        ///         RawLogFile = "someFile.txt",
        ///         DeployAt = new DateTime(2010, 6, 15).AddMinutes(1),
        ///         Tenant = new[] { "Tenant1", "Tenant2" },
        ///         TenantTags = new[] { "Tag1", "Tag2" },
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void OctoDeployRelease(this ICakeContext context, string server, string apiKey, string projectName, string deployTo, string releaseNumber, OctopusDeployReleaseDeploymentSettings settings)
        {
            OctoDeployRelease(context, server, apiKey, projectName, new string[] { deployTo }, releaseNumber, settings);
        }

        /// <summary>
        /// Deploys the specified already existing release into a specified environment
        /// See <see href="http://docs.octopusdeploy.com/display/OD/Deploying+releases">Octopus Documentation</see> for more details.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="server">The Octopus server URL.</param>
        /// <param name="apiKey">The user's API key.</param>
        /// <param name="projectName">Name of the target project.</param>
        /// <param name="deployToMultiple">Multiple target environment names.</param>
        /// <param name="releaseNumber">Version number of the release to deploy. Specify "latest" for the latest release.</param>
        /// <param name="settings">Deployment settings.</param>
        /// <example>
        /// <code>
        ///     // bare minimum
        ///     OctoDeployRelease("http://octopus-deploy.example", "API-XXXXXXXXXXXXXXXXXXXX", "MyGreatProject", "Testing", "2.1.15-RC" new OctopusDeployReleaseDeploymentSettings());
        ///
        ///     // All of deployment arguments
        ///     OctoDeployRelease("http://octopus-deploy.example", "API-XXXXXXXXXXXXXXXXXXXX", "MyGreatProject", new string[] {"Testing", "Testing2"}, "2.1.15-RC" new OctopusDeployReleaseDeploymentSettings {
        ///         ShowProgress = true,
        ///         ForcePackageDownload = true,
        ///         WaitForDeployment = true,
        ///         DeploymentTimeout = TimeSpan.FromMinutes(1),
        ///         CancelOnTimeout = true,
        ///         DeploymentChecksLeapCycle = TimeSpan.FromMinutes(77),
        ///         GuidedFailure = true,
        ///         SpecificMachines = new string[] { "Machine1", "Machine2" },
        ///         Force = true,
        ///         SkipSteps = new[] { "Step1", "Step2" },
        ///         NoRawLog = true,
        ///         RawLogFile = "someFile.txt",
        ///         DeployAt = new DateTime(2010, 6, 15).AddMinutes(1),
        ///         Tenant = new[] { "Tenant1", "Tenant2" },
        ///         TenantTags = new[] { "Tag1", "Tag2" },
        ///
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void OctoDeployRelease(this ICakeContext context, string server, string apiKey, string projectName, string[] deployToMultiple, string releaseNumber, OctopusDeployReleaseDeploymentSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var releaseDeployer = new OctopusDeployReleaseDeployer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            releaseDeployer.DeployRelease(server, apiKey, projectName, deployToMultiple, releaseNumber, settings);
        }

        /// <summary>
        /// Promotes the specified already existing release into a specified environment
        /// See <see href="https://octopus.com/docs/api-and-integration/octo.exe-command-line/promoting-releases">Octopus Documentation</see> for more details.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="server">The Octopus server URL.</param>
        /// <param name="apiKey">The user's API key.</param>
        /// <param name="projectName">Name of the target project.</param>
        /// <param name="deployFrom">Source environment name.</param>
        /// <param name="deployTo">Target environment name.</param>
        /// <param name="settings">Deployment settings.</param>
        /// <example>
        /// <code>
        ///     // bare minimum
        ///     OctoPromoteRelease("http://octopus-deploy.example", "API-XXXXXXXXXXXXXXXXXXXX", "MyGreatProject", "Testing", "Staging", new OctopusDeployPromoteReleaseSettings());
        ///
        ///     // All of deployment arguments
        ///     OctoPromoteRelease("http://octopus-deploy.example", "API-XXXXXXXXXXXXXXXXXXXX", "MyGreatProject", "Testing", "Staging", new OctopusDeployPromoteReleaseSettings {
        ///         ShowProgress = true,
        ///         ForcePackageDownload = true,
        ///         WaitForDeployment = true,
        ///         DeploymentTimeout = TimeSpan.FromMinutes(1),
        ///         CancelOnTimeout = true,
        ///         DeploymentChecksLeapCycle = TimeSpan.FromMinutes(77),
        ///         GuidedFailure = true,
        ///         SpecificMachines = new string[] { "Machine1", "Machine2" },
        ///         Force = true,
        ///         SkipSteps = new[] { "Step1", "Step2" },
        ///         NoRawLog = true,
        ///         RawLogFile = "someFile.txt",
        ///         DeployAt = new DateTime(2010, 6, 15).AddMinutes(1),
        ///         Tenant = new[] { "Tenant1", "Tenant2" },
        ///         TenantTags = new[] { "Tag1", "Tag2" },
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void OctoPromoteRelease(this ICakeContext context, string server, string apiKey, string projectName, string deployFrom, string deployTo, OctopusDeployPromoteReleaseSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var releasePromoter = new OctopusDeployReleasePromoter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            releasePromoter.PromoteRelease(server, apiKey, projectName, deployFrom, deployTo, settings);
        }
    }
}
