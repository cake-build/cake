// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using Cake.Common.Build.AppVeyor.Data;
using Cake.Common.Net;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Build.AppVeyor
{
    /// <summary>
    /// Responsible for communicating with AppVeyor.
    /// </summary>
    public sealed class AppVeyorProvider : Tool<AppVeyorToolSettings>, IAppVeyorProvider
    {
        private readonly ICakeEnvironment _environment;
        private readonly AppVeyorEnvironmentInfo _environmentInfo;
        private readonly AppVeyorToolSettings _settings;

        /// <summary>
        /// Gets a value indicating whether the current build is running on AppVeyor.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on AppVeyor.; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnAppVeyor
        {
            get { return !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("APPVEYOR")); }
        }

        /// <summary>
        /// Gets the AppVeyor environment.
        /// </summary>
        /// <value>
        /// The AppVeyor environment.
        /// </value>
        public AppVeyorEnvironmentInfo Environment
        {
            get { return _environmentInfo; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVeyorProvider"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="locator">The tool locator.</param>
        public AppVeyorProvider(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator locator) : base(fileSystem, environment, processRunner, locator)
        {
            _environment = environment;
            _environmentInfo = new AppVeyorEnvironmentInfo(environment);
            _settings = new AppVeyorToolSettings();
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
                throw new ArgumentNullException("path");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
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
            arguments.Append("-Path");
            arguments.AppendQuoted(path.FullPath);
            arguments.Append("-FileName");
            arguments.AppendQuoted(path.GetFilename().FullPath);
            arguments.Append("-ArtifactType");
            arguments.AppendQuoted(settings.ArtifactType.ToString());
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
            Run(_settings, arguments);
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
                throw new ArgumentNullException("settingsAction");
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
                throw new ArgumentNullException("path");
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

            var url = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}/api/testresults/{1}/{2}", baseUri, resultsType, Environment.JobId));

            using (var client = new HttpClient())
            {
                client.UploadFileAsync(url, path.FullPath).Wait();
            }
        }

        /// <summary>
        /// Updates the build version.
        /// </summary>
        /// <param name="version">The new build version.</param>
        public void UpdateBuildVersion(string version)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
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
            Run(_settings, arguments);
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
                throw new ArgumentNullException("message");
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
            Run(_settings, arguments);
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "AppVeyor";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            yield return "appveyor";
        }
    }
}