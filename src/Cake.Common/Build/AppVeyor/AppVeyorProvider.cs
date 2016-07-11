// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using Cake.Common.Build.AppVeyor.Data;
using Cake.Core;
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
        private readonly AppVeyorEnvironmentInfo _environmentInfo;

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
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        public AppVeyorProvider(ICakeEnvironment environment, IProcessRunner processRunner)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (processRunner == null)
            {
                throw new ArgumentNullException("processRunner");
            }
            _environment = environment;
            _processRunner = processRunner;
            _environmentInfo = new AppVeyorEnvironmentInfo(environment);
        }

        /// <summary>
        /// Uploads an AppVeyor artifact.
        /// </summary>
        /// <param name="path">The file path of the artifact to upload.</param>
        public void UploadArtifact(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
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

            // Start the process.
            _processRunner.Start("appveyor", new ProcessSettings { Arguments = arguments });
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

            var url = string.Format(CultureInfo.InvariantCulture, "{0}/api/testresults/{1}/{2}", baseUri, resultsType, Environment.JobId);

            using (var stream = File.OpenRead(path.FullPath))
            using (var client = new HttpClient())
            {
                client.PostAsync(url, new StreamContent(stream)).Wait();
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
            _processRunner.Start("appveyor", new ProcessSettings { Arguments = arguments });
        }
    }
}
