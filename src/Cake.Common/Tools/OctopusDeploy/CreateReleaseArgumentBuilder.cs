// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    internal sealed class CreateReleaseArgumentBuilder : OctopusDeployArgumentBuilder<CreateReleaseSettings>
    {
        private readonly string _projectName;
        private readonly CreateReleaseSettings _settings;
        private readonly ICakeEnvironment _environment;

        public CreateReleaseArgumentBuilder(string projectName, CreateReleaseSettings settings, ICakeEnvironment environment) : base(environment, settings)
        {
            _projectName = projectName;
            _settings = settings;
            _environment = environment;
        }

        public ProcessArgumentBuilder Get()
        {
            Builder.Append("create-release");
            Builder.AppendSwitchQuoted("--project", _projectName);

            AppendCommonArguments();

            AppendArgumentIfNotNull("releaseNumber", _settings.ReleaseNumber);
            AppendArgumentIfNotNull("defaultpackageversion", _settings.DefaultPackageVersion);
            AppendPackages(_settings, Builder);
            AppendArgumentIfNotNull("packagesFolder", _settings.PackagesFolder);
            AppendArgumentIfNotNull("releasenotes", _settings.ReleaseNotes);
            AppendArgumentIfNotNull("releasenotesfile", _settings.ReleaseNotesFile);
            AppendArgumentIfNotNull("channel", _settings.Channel);
            AppendArgumentIfNotNull("excludemachines", _settings.ExcludeMachines);

            if (_settings.IgnoreChannelRules)
            {
                Builder.Append("--ignorechannelrules");
            }

            if (_settings.DeploymentProgress)
            {
                Builder.Append("--progress");
            }

            if (_settings.IgnoreExisting)
            {
                Builder.Append("--ignoreexisting");
            }

            AppendDeploymnetArguments();

            return Builder;
        }

        private void AppendDeploymnetArguments()
        {
            AppendConditionalFlag(_settings.ShowProgress, "--progress");
            AppendConditionalFlag(_settings.ForcePackageDownload, "--forcepackagedownload");
            AppendConditionalFlag(_settings.WaitForDeployment, "--waitfordeployment");

            AppendArgumentIfNotNull("deployto", _settings.DeployTo);

            if (_settings.DeployToMultiple != null)
            {
                foreach (var target in _settings.DeployToMultiple)
                {
                    AppendArgumentIfNotNull("deployto", target);
                }
            }

            if (_settings.DeploymentTimeout.HasValue)
            {
                Builder.AppendSwitchQuoted("--deploymenttimeout", "=", _settings.DeploymentTimeout.Value.ToString("hh\\:mm\\:ss"));
            }

            AppendConditionalFlag(_settings.CancelOnTimeout, "--cancelontimeout");

            if (_settings.DeploymentChecksLeepCycle.HasValue)
            {
                Builder.AppendSwitchQuoted("--deploymentchecksleepcycle", "=", _settings.DeploymentChecksLeepCycle.Value.ToString("hh\\:mm\\:ss"));
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

        private static void AppendPackages(CreateReleaseSettings settings, ProcessArgumentBuilder builder)
        {
            if (settings.Packages != null)
            {
                foreach (var package in settings.Packages)
                {
                    builder.Append("--package");
                    builder.AppendQuoted(string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "{0}:{1}",
                        package.Key,
                        package.Value));
                }
            }
        }
    }
}