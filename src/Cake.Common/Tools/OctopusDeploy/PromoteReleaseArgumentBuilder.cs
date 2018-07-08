// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    internal sealed class PromoteReleaseArgumentBuilder : OctopusDeployArgumentBuilder<OctopusDeployPromoteReleaseSettings>
    {
        private readonly string _projectName;
        private readonly string _deployFrom;
        private readonly string _deployTo;
        private readonly OctopusDeployPromoteReleaseSettings _settings;
        private readonly ICakeEnvironment _environment;

        public PromoteReleaseArgumentBuilder(string server, string apiKey, string projectName, string deployFrom, string deployTo, OctopusDeployPromoteReleaseSettings settings, ICakeEnvironment environment)
            : base(server, apiKey, environment, settings)
        {
            _projectName = projectName;
            _deployFrom = deployFrom;
            _deployTo = deployTo;
            _settings = settings;
            _environment = environment;
        }

        public ProcessArgumentBuilder Get()
        {
            Builder.Append("promote-release");

            Builder.AppendSwitchQuoted("--project", "=", _projectName);
            Builder.AppendSwitchQuoted("--from", "=", _deployFrom);
            Builder.AppendSwitchQuoted("--to", "=", _deployTo);

            AppendConditionalFlag(_settings.UpdateVariables, "--force");
            AppendCommonAndDeploymentParameters();

            return Builder;
        }

        private void AppendCommonAndDeploymentParameters()
        {
            AppendCommonArguments();
            AppendDeploymentParameters();
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
        }
    }
}
