// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// Contains settings used by <see cref="OctopusDeployReleaseCreator.CreateRelease"/>.
    /// See Octopus Deploy documentation <see href="http://docs.octopusdeploy.com/display/OD/Creating+releases">here</see>.
    /// </summary>
    public sealed class CreateReleaseSettings : OctopusDeployCommonToolSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateReleaseSettings"/> class.
        /// </summary>
        public CreateReleaseSettings()
        {
            Variables = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Gets or sets the release number to use for the new release.
        /// </summary>
        public string ReleaseNumber { get; set; }

        /// <summary>
        /// Gets or sets the default version number of all packages to use the new release.
        /// </summary>
        public string DefaultPackageVersion { get; set; }

        /// <summary>
        /// Gets or sets the version number to use for a package in the release.
        /// </summary>
        public Dictionary<string, string> Packages { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets or sets the folder containing NuGet packages.
        /// </summary>
        public FilePath PackagesFolder { get; set; }

        /// <summary>
        /// Gets or sets the release notes for the new release.
        /// </summary>
        public string ReleaseNotes { get; set; }

        /// <summary>
        /// Gets or sets the path to a file that contains Release Notes for the new release.
        /// </summary>
        public FilePath ReleaseNotesFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Ignore Existing release flag.
        /// </summary>
        public bool IgnoreExisting { get; set; }

        /// <summary>
        /// Gets or sets environment to automatically deploy to, e.g., Production.
        /// </summary>
        public string DeployTo { get; set; }

        /// <summary>
        /// Gets or sets multiple environments to automatically deploy to, e.g., Production, Staging, etc.
        /// </summary>
        public string[] DeployToMultiple { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether progress of the deployment should be followed. (Sets --waitfordeployment and --norawlog to true.)
        /// </summary>
        public bool ShowProgress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force downloading of already installed packages. Default false.
        /// </summary>
        public bool ForcePackageDownload { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to wait synchronously for deployment to finish.
        /// </summary>
        public bool WaitForDeployment { get; set; }

        /// <summary>
        /// Gets or sets maximum time (timespan format) that the console session will wait for the deployment to finish (default 00:10:00).
        /// This will not stop the deployment. Requires WaitForDeployment parameter set.
        /// </summary>
        public TimeSpan? DeploymentTimeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to cancel the deployment if the deployment timeout is reached(default false).
        /// </summary>
        public bool CancelOnTimeout { get; set; }

        /// <summary>
        /// Gets or sets how much time should elapse between deployment status checks(default 00:00:10).
        /// </summary>
        public TimeSpan? DeploymentChecksLeepCycle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use Guided Failure mode. If not specified, will use default setting from environment.
        /// </summary>
        public bool? GuidedFailure { get; set; }

        /// <summary>
        /// Gets or sets list of machines names to target in the deployed environment.If not specified all machines in the environment will be considered.
        /// </summary>
        public string[] SpecificMachines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a project is configured to skip packages with already-installed versions, override this setting to force re-deployment (flag, default false).
        /// </summary>
        public bool Force { get; set; }

        /// <summary>
        /// Gets or sets a list of steps to be skipped. Takes step names.
        /// </summary>
        public string[] SkipSteps { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether print the raw log of failed tasks or not.
        /// </summary>
        public bool NoRawLog { get; set; }

        /// <summary>
        /// Gets or sets a file where to redirect the raw log of failed tasks.
        /// </summary>
        public FilePath RawLogFile { get; set; }

        /// <summary>
        /// Gets or sets values for any prompted variables.
        /// </summary>
        public List<KeyValuePair<string, string>> Variables { get; set; }

        /// <summary>
        /// Gets or sets time at which deployment should start (scheduled deployment), specified as any valid DateTimeOffset format, and assuming the time zone is the current local time zone.
        /// </summary>
        public DateTimeOffset? DeployAt { get; set; }

        /// <summary>
        /// Gets or sets a tenant the deployment will be performed for; specify this argument multiple times to add multiple tenants or use `*` wildcard to deploy to tenants able to deploy.
        /// </summary>
        public string[] Tenant { get; set; }

        /// <summary>
        /// Gets or sets a tenant tags used to match tenants that the deployment will be performed for; specify this argument multiple times to add multiple tenant tags.
        /// </summary>
        public string[] TenantTags { get; set; }

        /// <summary>
        /// Gets or sets the octopus channel for the new release.
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether octopus channel rules should be ignored.
        /// </summary>
        public bool IgnoreChannelRules { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether progress of the deployment will be shown.
        /// </summary>
        public bool DeploymentProgress { get; set; }

        /// <summary>
        ///  Gets or sets the comma-separated list of machine names to exclude in the deployed environment.If not specified all machines in the environment will be considered.
        /// </summary>
        public string ExcludeMachines { get; set; }
    }
}