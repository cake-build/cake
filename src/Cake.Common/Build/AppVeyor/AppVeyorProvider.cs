// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Cake.Common.Build.AppVeyor.Data;
using Cake.Common.Net;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Build.AppVeyor
{
    /// <summary>
    /// Responsible for communicating with AppVeyor.
    /// </summary>
    public sealed class AppVeyorProvider : IAppVeyorProvider
    {
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _processRunner;
        private readonly ICakeLog _cakeLog;

        /// <summary>
        /// Gets a value indicating whether the current build is running on AppVeyor.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on AppVeyor.; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnAppVeyor => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("APPVEYOR"));

        /// <summary>
        /// Gets the AppVeyor environment.
        /// </summary>
        /// <value>
        /// The AppVeyor environment.
        /// </value>
        public AppVeyorEnvironmentInfo Environment { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVeyorProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="cakeLog">The cake log.</param>
        public AppVeyorProvider(ICakeEnvironment environment, IProcessRunner processRunner, ICakeLog cakeLog)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (processRunner == null)
            {
                throw new ArgumentNullException(nameof(processRunner));
            }
            if (cakeLog == null)
            {
                throw new ArgumentNullException(nameof(cakeLog));
            }
            _environment = environment;
            _processRunner = processRunner;
            _cakeLog = cakeLog;
            Environment = new AppVeyorEnvironmentInfo(environment);
        }

        /// <summary>
        /// Uploads an AppVeyor artifact.
        /// </summary>
        /// <param name="path">The file path of the artifact to upload.</param>
        public void UploadArtifact(FilePath path)
        {
            UploadArtifact(path, settings => settings.SetArtifactType(AppVeyorUploadArtifactType.Auto));
        }

        /// <summary>
        /// Uploads an AppVeyor artifact.
        /// </summary>
        /// <param name="path">The file path of the artifact to upload.</param>
        /// <param name="settings">The settings to apply when uploading an artifact</param>
        public void UploadArtifact(FilePath path, AppVeyorUploadArtifactsSettings settings)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (!IsRunningOnAppVeyor)
            {
                throw new CakeException("The current build is not running on AppVeyor.");
            }

            // Make path absolute.
            path = path.IsRelative ? path.MakeAbsolute(_environment) : path;

            // Build the arguments.
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("PushArtifact");
            arguments.AppendQuoted(path.FullPath);
            arguments.Append("-Type");
            arguments.Append(settings.ArtifactType.ToString());
            if (!string.IsNullOrEmpty(settings.DeploymentName))
            {
                if (settings.DeploymentName.Contains(" "))
                {
                    throw new CakeException("The deployment name can not contain spaces");
                }
                arguments.Append("-DeploymentName");
                arguments.AppendQuoted(settings.DeploymentName);
            }

            // Start the process.
            _processRunner.Start("appveyor", new ProcessSettings { Arguments = arguments });
        }

        /// <summary>
        /// Uploads an AppVeyor artifact.
        /// </summary>
        /// <param name="path">The file path of the artifact to upload.</param>
        /// <param name="settingsAction">The settings to apply when uploading an artifact</param>
        public void UploadArtifact(FilePath path, Action<AppVeyorUploadArtifactsSettings> settingsAction)
        {
            if (settingsAction == null)
            {
                throw new ArgumentNullException(nameof(settingsAction));
            }

            var settings = new AppVeyorUploadArtifactsSettings();
            settingsAction(settings);
            UploadArtifact(path, settings);
        }

        /// <summary>
        /// Uploads test results XML file to AppVeyor. Results type can be one of the following: mstest, xunit, nunit, nunit3, junit.
        /// </summary>
        /// <param name="path">The file path of the test results XML to upload.</param>
        /// <param name="resultsType">The results type. Can be mstest, xunit, nunit, nunit3 or junit.</param>
        public void UploadTestResults(FilePath path, AppVeyorTestResultsType resultsType)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!IsRunningOnAppVeyor)
            {
                throw new CakeException("The current build is not running on AppVeyor.");
            }

            var baseUri = _environment.GetEnvironmentVariable("APPVEYOR_URL").TrimEnd('/');

            if (string.IsNullOrWhiteSpace(baseUri))
            {
                throw new CakeException("Failed to get AppVeyor API url.");
            }

            var url = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}/api/testresults/{1}/{2}", baseUri, resultsType, Environment.JobId).ToLowerInvariant());

            _cakeLog.Write(Verbosity.Diagnostic, LogLevel.Verbose, "Uploading [{0}] to [{1}]", path.FullPath, url);
            Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    var response = await client.UploadFileAsync(url, path.FullPath, "text/xml");
                    var content = await response.Content.ReadAsStringAsync();
                    _cakeLog.Write(Verbosity.Diagnostic, LogLevel.Verbose, "Server response [{0}:{1}]:\n\r{2}", response.StatusCode, response.ReasonPhrase, content);
                }
            }).Wait();
        }

        /// <summary>
        /// Updates the build version.
        /// </summary>
        /// <param name="version">The new build version.</param>
        public void UpdateBuildVersion(string version)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new CakeException("The build version cannot be empty.");
            }

            if (!IsRunningOnAppVeyor)
            {
                throw new CakeException("The current build is not running on AppVeyor.");
            }

            // Build the arguments.
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("UpdateBuild");
            arguments.Append("-Version");
            arguments.AppendQuoted(version);

            // Start the process.
            _processRunner.Start("appveyor", new ProcessSettings { Arguments = arguments });
        }

        /// <summary>
        /// Adds a message to the AppVeyor build.  Messages can be categorised as: Information, Warning or Error
        /// </summary>
        /// <param name="message">A short message to display</param>
        /// <param name="category">The category of the message</param>
        /// <param name="details">Additional message details</param>
        public void AddMessage(string message, AppVeyorMessageCategoryType category = AppVeyorMessageCategoryType.Information, string details = null)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new CakeException("The message cannot be empty.");
            }

            if (!IsRunningOnAppVeyor)
            {
                throw new CakeException("The current build is not running on AppVeyor.");
            }

            // Build the arguments.
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("AddMessage");
            arguments.AppendQuoted(message);
            arguments.Append("-Category");
            arguments.AppendQuoted(category.ToString());

            if (!string.IsNullOrWhiteSpace(details))
            {
                arguments.Append("-Details");
                arguments.AppendQuoted(details);
            }

            // Start the process.
            _processRunner.Start("appveyor", new ProcessSettings { Arguments = arguments });
        }
    }
}