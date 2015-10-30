using System;
using Cake.Common.Build.AppVeyor;
using Cake.Common.Build.MyGet;
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
        private readonly IMyGetProvider _myGetProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildSystem"/> class.
        /// </summary>
        /// <param name="appVeyorProvider">The AppVeyor Provider.</param>
        /// <param name="teamCityProvider">The TeamCity Provider.</param>
        /// <param name="myGetProvider">The MyGet Provider.</param>
        public BuildSystem(IAppVeyorProvider appVeyorProvider, ITeamCityProvider teamCityProvider, IMyGetProvider myGetProvider)
        {
            if (appVeyorProvider == null)
            {
                throw new ArgumentNullException("appVeyorProvider");
            }
            if (teamCityProvider == null)
            {
                throw new ArgumentNullException("teamCityProvider");
            }
            if (myGetProvider == null)
            {
                throw new ArgumentNullException("myGetProvider");
            }
            _appVeyorProvider = appVeyorProvider;
            _teamCityProvider = teamCityProvider;
            _myGetProvider = myGetProvider;
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
        /// Gets the TeamCity Provider.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnTeamCiy)
        /// {
        ///     // Set the build number.
        ///     BuildSystem.TeamCity.SetBuildNumber("1.2.3.4");
        /// }
        /// </code>
        /// </example>
        public ITeamCityProvider TeamCity
        {
            get { return _teamCityProvider; }
        }

        /// <summary>
        /// Gets a value indicating whether the current build is running on MyGet.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnMyGet)
        /// {
        ///     MyGet.BuildProblem("Something went wrong...");
        ///     // Do action...
        /// }
        /// </code>
        /// </example>
        /// <value>
        /// <c>true</c> if the build currently is running on MyGet; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnMyGet
        {
            get { return _myGetProvider.IsRunningOnMyGet; }
        }

        /// <summary>
        /// Gets the MyGet Provider.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnMyGet)
        /// {
        ///     // Set the build number.
        ///     BuildSystem.MyGet.SetBuildNumber("1.2.3.4");
        /// }
        /// </code>
        /// </example>
        public IMyGetProvider MyGet
        {
            get { return _myGetProvider; }
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
            get { return !(IsRunningOnAppVeyor || IsRunningOnTeamCity || IsRunningOnMyGet); }
        }
    }
}