// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    internal sealed class DeployReleaseArgumentBuilder : OctopusDeployArgumentBuilder<OctopusDeployReleaseDeploymentSettings>
    {
        private readonly ICakeEnvironment _environment;

        private readonly string _projectName;
        private readonly string[] _deployTo;
        private readonly string _releaseNumber;
        private readonly OctopusDeployReleaseDeploymentSettings _settings;

        public DeployReleaseArgumentBuilder(string server, string apiKey, string projectName, string[] deployTo, string releaseNumber, OctopusDeployReleaseDeploymentSettings settings, ICakeEnvironment environment)
            : base(server, apiKey, environment, settings)
        {
            _projectName = projectName;
            _deployTo = deployTo;
            _releaseNumber = releaseNumber;

            _environment = environment;
            _settings = settings;
        }

        public ProcessArgumentBuilder Get()
        {
            Builder.Append("deploy-release");

            Builder.AppendSwitchQuoted("--project", "=", _projectName);
            Builder.AppendSwitchQuoted("--releasenumber", "=", _releaseNumber);

            foreach (var environment in _deployTo)
            {
                Builder.AppendSwitchQuoted("--deployto", "=", environment);
            }

            AppendCommonArguments();

            AppendDeploymentParameters();

            return Builder;
        }

        private void AppendDeploymentParameters()
        {
            AppendConditionalFlag(_settings.ShowProgress, "--progress");
            AppendConditionalFlag(_settings.ForcePackageDownload, "--forcepackagedownload");
            AppendConditionalFlag(_settings.WaitForDeployment, "--waitfordeployment");

            if (_settings.DeploymentTimeout.HasValue)
            {
                Builder.AppendSwitchQuoted("--deploymenttimeout", "=",
                    _settings.DeploymentTimeout.Value.ToString("hh\\:mm\\:ss"));
            }

            AppendConditionalFlag(_settings.CancelOnTimeout, "--cancelontimeout");

            if (_settings.DeploymentChecksLeepCycle.HasValue)
            {
                Builder.AppendSwitchQuoted("--deploymentchecksleepcycle", "=",
                    _settings.DeploymentChecksLeepCycle.Value.ToString("hh\\:mm\\:ss"));
            }

            if (_settings.GuidedFailure.HasValue)
            {
                Builder.AppendSwitch("--guidedfailure", "=", _settings.GuidedFailure.ToString());
            }

            if (_settings.SpecificMachines != null && _settings.SpecificMachines.Length > 0)
            {
                Builder.AppendSwitchQuoted("--specificmachines", "=", string.Join(",", _settings.SpecificMachines));
            }

            AppendConditionalFlag(_settings.Force, "--force");

            AppendMultipleTimes("skip", _settings.SkipSteps);

            AppendConditionalFlag(_settings.NoRawLog, "--norawlog");

            AppendArgumentIfNotNull("rawlogfile", _settings.RawLogFile);

            if (_settings.Variables != null && _settings.Variables.Count > 0)
            {
                foreach (var pair in _settings.Variables)
                {
                    Builder.AppendSwitchQuoted("--variable", "=", $"{pair.Key}:{pair.Value}");
                }
            }

            if (_settings.DeployAt.HasValue)
            {
                Builder.AppendSwitchQuoted("--deployat", "=", _settings.DeployAt.Value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
            }

            AppendMultipleTimes("tenant", _settings.Tenant);

            AppendMultipleTimes("tenanttag", _settings.TenantTags);

            AppendArgumentIfNotNull("channel", _settings.Channel);

            AppendArgumentIfNotNull("excludemachines", _settings.ExcludeMachines);
        }
    }
}
