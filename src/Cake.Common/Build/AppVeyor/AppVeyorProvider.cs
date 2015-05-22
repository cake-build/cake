using System;
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
            var settings = new ProcessSettings()
                    .Append("PushArtifact")
                    .AppendNamedQuoted("Path", path.FullPath)
                    .AppendNamedQuoted("FileName", path.GetFilename().FullPath);

            // Start the process.
            _processRunner.Start("appveyor", settings);
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
            var settings = new ProcessSettings()
                    .Append("UpdateBuild")
                    .AppendNamedQuoted("Version", version);

            // Start the process.
            _processRunner.Start("appveyor", settings);
        }
    }
}
