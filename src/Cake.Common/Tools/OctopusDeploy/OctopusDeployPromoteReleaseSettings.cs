// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// Possible arguments to pass to Octo.exe for promoting a release. See <see href="https://octopus.com/docs/api-and-integration/octo.exe-command-line/promoting-releases">Octopus Deploy documentation</see>.
    /// </summary>
    public sealed class OctopusDeployPromoteReleaseSettings : OctopusDeployCommonToolSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OctopusDeployPromoteReleaseSettings"/> class.
        /// </summary>
        public OctopusDeployPromoteReleaseSettings()
        {
            Variables = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether overwrite the variable snapshot for the release by re-importing the variables from the project.
        /// </summary>
        public bool UpdateVariables { get; set; }

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
        public IDictionary<string, string> Variables { get; set; }

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
    }
}
