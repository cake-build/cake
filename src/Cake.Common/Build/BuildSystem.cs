using System;
using Cake.Common.Build.AppVeyor;
using Cake.Common.Build.TeamCity;

namespace Cake.Common.Build
{
    /// <summary>
    /// Provides functionality for interacting with
    /// different build systems.
    /// </summary>
    public sealed class BuildSystem
    {
        private readonly IAppVeyorProvider _appVeyorProvider;
        private readonly ITeamCityProvider _teamCityProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildSystem"/> class.
        /// </summary>
        /// <param name="appVeyorProvider">The AppVeyor Provider.</param>
        /// <param name="teamCityProvider">The TeamCity Provider.</param>
        public BuildSystem(IAppVeyorProvider appVeyorProvider, ITeamCityProvider teamCityProvider)
        {
            if (appVeyorProvider == null)
            {
                throw new ArgumentNullException("appVeyorProvider");
            }
            if (teamCityProvider == null)
            {
                throw new ArgumentNullException("teamCityProvider");
            }
            _appVeyorProvider = appVeyorProvider;
            _teamCityProvider = teamCityProvider;
        }

        /// <summary>
        /// Gets a value indicating whether the current build is running on AppVeyor.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnAppVeyor)
        /// {
        ///     // Upload artifact to AppVeyor.
        ///     AppVeyor.UploadArtifact("./build/release_x86.zip");
        /// }
        /// </code>
        /// </example>
        /// <value>
        /// <c>true</c> if the build currently is running on AppVeyor; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnAppVeyor
        {
            get { return _appVeyorProvider.IsRunningOnAppVeyor; }
        }

        /// <summary>
        /// Gets the AppVeyor Provider.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnAppVeyor)
        /// {
        ///     // Upload artifact to AppVeyor.
        ///     BuildSystem.AppVeyor.UploadArtifact("./build/release_x86.zip");
        /// }
        /// </code>
        /// </example>
        public IAppVeyorProvider AppVeyor
        {
            get { return _appVeyorProvider; }
        }

        /// <summary>
        /// Gets a value indicating whether the current build is running on TeamCity.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.ProgressMessage("Doing an action...");
        ///     // Do action...
        /// }
        /// </code>
        /// </example>
        /// <value>
        /// <c>true</c> if the build currently is running on TeamCity; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnTeamCity
        {
            get { return _teamCityProvider.IsRunningOnTeamCity; }
        }

        /// <summary>
        /// Gets the AppVeyor Provider.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnAppVeyor)
        /// {
        ///     // Upload artifact to AppVeyor.
        ///     BuildSystem.AppVeyor.UploadArtifact("./build/release_x86.zip");
        /// }
        /// </code>
        /// </example>
        public ITeamCityProvider TeamCity
        {
            get { return _teamCityProvider; }
        }

        /// <summary>
        /// Gets a value indicating whether the current build is local build.
        /// </summary>
        /// <example>
        /// <code>
        /// // Get a flag telling us if this is a local build or not.
        /// var isLocal = BuildSystem.IsLocalBuild;
        ///
        /// // Define a task that only runs locally.
        /// Task("LocalOnly")
        ///   .WithCriteria(isLocal)
        ///   .Does(() =>
        /// {
        /// });
        /// </code>
        /// </example>
        /// <value>
        ///   <c>true</c> if the current build is local build; otherwise, <c>false</c>.
        /// </value>
        public bool IsLocalBuild
        {
            get { return !(IsRunningOnAppVeyor || IsRunningOnTeamCity); }
        }
    }
}
